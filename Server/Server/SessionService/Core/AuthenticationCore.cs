using Server.Shared;
using Server.Validator;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.SessionService.Core
{
    public class AuthenticationCore
    {
        private readonly IDbContextFactory _dbFactory;
        private readonly ISecurityService _securityService;
        private readonly ISessionManager _sessionManager;
        private readonly INotificationService _notificationService;
        private readonly ILoggerManager _logger;
        private readonly IUserServiceValidator _validator;

        public AuthenticationCore(
            IDbContextFactory dbFactory,
            ISecurityService securityService,
            ISessionManager sessionManager,
            INotificationService notificationService,
            ILoggerManager logger)
        {
            _dbFactory = dbFactory;
            _securityService = securityService;
            _sessionManager = sessionManager;
            _notificationService = notificationService;
            _logger = logger;
            _validator = new UserServiceValidator();
        }

        public ResponseDTO StartRegistration(string email, string password)
        {

            if (!_validator.IsValidPassword(password))
            {
                _logger.LogInfo($"Invalid password attempt during registration for email: {email}");
                return new ResponseDTO { Success = false, MessageKey = "Global_Error_PasswordInvalid" };
            }

            string hashedPassword = _securityService.HashPassword(password);

            try
            {
                using (var db = _dbFactory.Create())
                {

                    if (db.user.Any(u => u.email == email))
                    {
                        _logger.LogInfo($"Attempt to register with already used email: {email}");
                        return new ResponseDTO { Success = false, MessageKey = "Global_Error_EmailInUse" };
                    }

                    var existingPending = db.pendingRegistration
                        .FirstOrDefault(p => p.email == email && p.expirationTime > DateTime.Now);

                    if (existingPending != null)
                    {
                        _logger.LogInfo($"Removing existing pending registration for email: {email}");
                        db.pendingRegistration.Remove(existingPending);
                    }

                    string pin = _securityService.GeneratePin();

                    var pendingRegistration = new pendingRegistration
                    {
                        email = email,
                        pin = pin,
                        hashedPassword = hashedPassword,
                        expirationTime = DateTime.Now.AddMinutes(15),
                        createdAt = DateTime.Now
                    };

                    _logger.LogInfo($"Creating pending registration for email: {email}");
                    db.pendingRegistration.Add(pendingRegistration);

                    if (!_notificationService.SendVerificationEmail(email, pin))
                    {
                        _logger.LogError($"Failed to send verification email to: {email}");
                        return new ResponseDTO { Success = false, MessageKey = "Global_Error_EmailSendFailed" };
                    }

                    db.SaveChanges();

                    _logger.LogInfo($"Registration started successfully for email: {email}");
                    return new ResponseDTO { Success = true };
                }
            }
            catch (EntityException ex)
            {
                _logger.LogError($"StartRegistration Database Error for email {email}: {ex.Message}");
                return new ResponseDTO { Success = false, MessageKey = "Global_ServiceError_Database" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"StartRegistration Error for email {email}: {ex.Message}");
                return new ResponseDTO { Success = false, MessageKey = "Global_ServiceError_Unknown" };
            }
        }

        public ResponseDTO ResendVerificationCode(string email)
        {
            try
            {
                using (var db = _dbFactory.Create())
                {
                    if (db.user.Any(u => u.email == email))
                    {
                        _logger.LogInfo($"ResendVerificationCode attempt for already registered email: {email}");
                        return new ResponseDTO { Success = false, MessageKey = "Global_Error_EmailInUse" };
                    }

                    var pending = db.pendingRegistration.FirstOrDefault(p => p.email == email);
                    if (pending == null)
                    {
                        _logger.LogInfo($"ResendVerificationCode attempt with no pending registration for email: {email}");
                        return new ResponseDTO { Success = false, MessageKey = "Global_Error_RegistrationNotFound" };
                    }

                    string newPin = _securityService.GeneratePin();
                    pending.pin = newPin;
                    pending.expirationTime = DateTime.Now.AddMinutes(15);

                    if (!_notificationService.SendVerificationEmail(email, newPin))
                    {
                        _logger.LogError($"Failed to resend verification email to: {email}");
                        return new ResponseDTO { Success = false, MessageKey = "Global_Error_EmailSendFailed" };
                    }

                    db.SaveChanges();

                    _logger.LogInfo($"Resent verification code to email: {email}");
                    return new ResponseDTO { Success = true };
                }
            }
            catch (EntityException ex)
            {
                _logger.LogError($"ResendVerificationCode Database Error for email {email}: {ex.Message}");
                return new ResponseDTO { Success = false, MessageKey = "Global_ServiceError_Database" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"ResendVerificationCode Error for email {email}: {ex.Message}");
                return new ResponseDTO { Success = false, MessageKey = "Global_ServiceError_Unknown" };
            }
        }

        public ResponseDTO VerifyRegistration(string email, string pin)
        {
            try
            {
                using (var db = _dbFactory.Create())
                {
                    var pending = db.pendingRegistration
                        .FirstOrDefault(p => p.email == email &&
                        p.pin == pin &&
                        p.expirationTime > DateTime.Now);

                    if (pending == null)
                    {
                        _logger.LogInfo($"Invalid or expired verification attempt for email: {email}");
                        return new ResponseDTO { Success = false, MessageKey = "Global_Error_CodeInvalid" };
                    }

                    if (pending.expirationTime <= DateTime.Now)
                    {
                        _logger.LogInfo($"Expired verification code attempt for email: {email}");
                        return new ResponseDTO { Success = false, MessageKey = "Global_Error_CodeExpired" };
                    }

                    var newUser = new user
                    {
                        email = email,
                        password = pending.hashedPassword,
                        username = email.Split('@')[0],
                        isGuest = false,
                        verifiedEmail = true,
                        registrationDate = DateTime.Now
                    };

                    db.user.Add(newUser);
                    db.SaveChanges();

                    _logger.LogInfo($"User registration verified and created for email: {email}");

                    _securityService.RemovePendingRegistration(email);
                    db.SaveChanges();

                    _logger.LogInfo($"Pending registration removed for email: {email}");
                    return new ResponseDTO { Success = true };
                }
            }
            catch (EntityException ex)
            {
                _logger.LogError($"VerifyRegistration Database Error for email {email}: {ex.Message}");
                return new ResponseDTO { Success = false, MessageKey = "Global_ServiceError_Database" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"VerifyRegistration Error for email {email}: {ex.Message}");
                return new ResponseDTO { Success = false, MessageKey = "Global_ServiceError_Unknown" };
            }
        }

        public LoginResponse FinalizeRegistration(string email, string username, byte[] avatar)
        {
            try
            {
                using (var db = _dbFactory.Create())
                {
                    var user = db.user.FirstOrDefault(u => u.email == email);

                    if (db.user.Any(u => u.username == username))
                    {
                        _logger.LogInfo($"Attempt to finalize registration with already used username: {username}");
                        return new LoginResponse { Success = false, MessageKey = "Global_Error_InvalidUsername" };
                    }

                    if (!_validator.IsValidUsername(username))
                    {
                        _logger.LogInfo($"Attempt to finalize registration with invalid username: {username}");
                        return new LoginResponse { Success = false, MessageKey = "Global_ValidationUsername_InvalidChars" };
                    }

                    user.username = username;
                    user.avatar = avatar;
                    db.SaveChanges();

                    string token = _sessionManager.CreateSessionToken(user.userId);

                    var userDto = new UserDTO
                    {
                        UserId = user.userId,
                        Username = user.username,
                        Email = user.email,
                        Name = user.name,
                        LastName = user.lastName,
                        RegistrationDate = user.registrationDate,
                        SocialNetworks = user.socialNetwork.Select(sn => new SocialNetworkDTO
                        {
                            SocialNetworkId = sn.socialNetworkId,
                            Account = sn.account
                        }).ToList()
                    };

                    _logger.LogInfo($"User registration finalized for email: {email}");
                    return new LoginResponse
                    {
                        Success = true,
                        SessionToken = token,
                        User = userDto
                    };
                }
            }
            catch (EntityException ex)
            {
                _logger.LogError($"FinalizeRegistration Database Error for email {email}: {ex.Message}");
                return new LoginResponse { Success = false, MessageKey = "Global_ServiceError_Database" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"FinalizeRegistration Error for email {email}: {ex.Message}");
                return new LoginResponse { Success = false, MessageKey = "Global_ServiceError_Unknown" };
            }
        }

        public LoginResponse Login(string email, string password)
        {
            try
            {
                using (var db = _dbFactory.Create())
                {
                    var user = db.user.FirstOrDefault(u => u.email == email);
                    if (user == null)
                    {
                        _logger.LogInfo($"Invalid login attempt for non-existent email: {email}");
                        System.Threading.Thread.Sleep(100);
                        return new LoginResponse { Success = false, MessageKey = "Global_Error_InvalidCredentials" };
                    }

                    if (!BCrypt.Net.BCrypt.Verify(password, user.password))
                    {
                        _logger.LogInfo($"Invalid password attempt for email: {email}");
                        System.Threading.Thread.Sleep(100);
                        return new LoginResponse { Success = false, MessageKey = "Global_Error_InvalidCredentials" };
                    }

                    if (user.penaltyId != null && user.penalty != null)
                    {
                        if (user.penalty.duration > DateTime.UtcNow)
                        {
                            _logger.LogInfo($"Login attempt blocked due to active penalty for email: {email}");
                            return new LoginResponse
                            {
                                Success = false,
                                MessageKey = "Global_Error_AccountPenalized"
                            };
                        }
                        else
                        {
                            user.penaltyId = null;
                            user.penalty = null;
                            db.SaveChanges();
                            _logger.LogInfo($"Expired penalty cleared for email: {email}");
                        }
                    }

                    string token = _sessionManager.CreateSessionToken(user.userId);

                    var userDto = new UserDTO
                    {
                        UserId = user.userId,
                        Username = user.username,
                        Email = user.email,
                        Name = user.name,
                        LastName = user.lastName,
                        RegistrationDate = user.registrationDate,
                        SocialNetworks = user.socialNetwork.Select(sn => new SocialNetworkDTO
                        {
                            SocialNetworkId = sn.socialNetworkId,
                            Account = sn.account
                        }).ToList()
                    };

                    _logger.LogInfo($"User logged in successfully: {email}");
                    return new LoginResponse
                    {
                        Success = true,
                        SessionToken = token,
                        User = userDto
                    };
                }
            }
            catch (EntityException ex)
            {
                _logger.LogError($"Login Database Error for email {email}: {ex.Message}");
                return new LoginResponse { Success = false, MessageKey = "Global_ServiceError_Database" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Login Error for email {email}: {ex.Message}");
                return new LoginResponse { Success = false, MessageKey = "Global_ServiceError_Unknown" };
            }
        }

        public LoginResponse LoginAsGuest(string guestUsername)
        {
            try
            {
                using (var db = _dbFactory.Create())
                {

                    if (!_validator.IsValidUsername(guestUsername))
                    {
                        _logger.LogInfo($"Invalid guest username attempt: {guestUsername}");
                        return new LoginResponse { Success = false, MessageKey = "Global_Error_InvalidUsername" };
                    }

                    if (db.user.Any(u => u.username == guestUsername))
                    {
                        _logger.LogInfo($"Guest username already in use attempt: {guestUsername}");
                        return new LoginResponse { Success = false, MessageKey = "Global_Error_UsernameInUse" };
                    }

                    string guestPasswordRaw = _securityService.GenerateGuestPassword();

                    var guestUser = new user
                    {
                        email = $"{Guid.NewGuid()}@guest.local",
                        password = _securityService.HashPassword(guestPasswordRaw),
                        username = guestUsername,
                        avatar = null,
                        isGuest = true,
                        verifiedEmail = false,
                        registrationDate = DateTime.Now
                    };

                    db.user.Add(guestUser);
                    db.SaveChanges();

                    _logger.LogInfo($"Guest user created with username: {guestUsername}");

                    string token = _sessionManager.CreateSessionToken(guestUser.userId);

                    _logger.LogInfo($"Guest user logged in with username: {guestUsername}");
                    return new LoginResponse
                    {
                        Success = true,
                        SessionToken = token,
                        User = new UserDTO
                        {
                            UserId = guestUser.userId,
                            Username = guestUser.username,
                            Email = guestUser.email
                        }
                    };
                }
            }
            catch (EntityException ex)
            {
                _logger.LogError($"LoginAsGuest Database Error for username {guestUsername}: {ex.Message}");
                return new LoginResponse { Success = false, MessageKey = "Global_ServiceError_Database" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"LoginAsGuest Error for username {guestUsername}: {ex.Message}");
                return new LoginResponse { Success = false, MessageKey = "Global_ServiceError_Unknown" };
            }
        }

        public void LogoutGuest(string sessionToken)
        {
            try
            {
                var userId = _sessionManager.GetUserIdFromToken(sessionToken);
                if (userId == null)
                {
                    _logger.LogInfo($"LogoutGuest called with invalid session token.");
                    return;
                }

                using (var db = _dbFactory.Create())
                {
                    var guestUser = db.user.FirstOrDefault(u => u.userId == userId.Value && u.isGuest == true);

                    if (guestUser != null)
                    {
                        db.user.Remove(guestUser);
                        db.SaveChanges();

                        _logger.LogInfo($"Guest user with userId: {userId.Value} logged out and removed.");
                    }
                }
            }
            catch (EntityException ex)
            {
                _logger.LogError($"LogoutGuest Database Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"LogoutGuest Error: {ex.Message}");
            }
        }

        public ResponseDTO InitiateGuestRegistration(int guestUserId, string newEmail, string newPassword)
        {
            if (!_validator.IsValidPassword(newPassword))
            {
                _logger.LogInfo($"Invalid password attempt during guest to user update for userId: {guestUserId}");
                return new ResponseDTO { Success = false, MessageKey = "Global_Error_PasswordInvalid" };
            }
            string hashedPassword = _securityService.HashPassword(newPassword);

            try
            {
                using (var db = new memoryGameDBEntities())
                {
                    var guestUser = db.user.Find(guestUserId);
                    if (guestUser == null)
                    {
                        return new ResponseDTO { Success = false, MessageKey = "Global_Error_UserNotFound" };
                    }
                        

                    if (guestUser.isGuest != true)
                    {
                        return new ResponseDTO { Success = false, MessageKey = "Global_Error_AlreadyRegistered" };
                    }
                        

                    if (db.user.Any(u => u.email == newEmail && u.userId != guestUserId))
                    {
                        return new ResponseDTO { Success = false, MessageKey = "Global_Error_EmailInUse" };
                    }
                        

                    var oldPending = db.pendingRegistration.FirstOrDefault(p => p.email == newEmail);
                    if (oldPending != null) db.pendingRegistration.Remove(oldPending);

                    string pin = _securityService.GeneratePin();

                    var pending = new pendingRegistration
                    {
                        email = newEmail,
                        hashedPassword = hashedPassword,
                        pin = pin,
                        expirationTime = DateTime.Now.AddMinutes(15),
                        createdAt = DateTime.Now
                    };
                    db.pendingRegistration.Add(pending);
                    db.SaveChanges();

                    if (!_notificationService.SendVerificationEmail(newEmail, pin))
                    {
                        _logger.LogError($"Failed to send verification email to: {newEmail}");
                        return new ResponseDTO { Success = false, MessageKey = "Global_Error_EmailSendFailed" };
                    }
                    else
                    {
                        return new ResponseDTO { Success = true };
                    }
                }
            }
            catch (EntityException ex)
            {
                _logger.LogError($"StartRegistration Database Error for email {newEmail}: {ex.Message}");
                return new ResponseDTO { Success = false, MessageKey = "Global_ServiceError_Database" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"StartRegistration Error for email {newEmail}: {ex.Message}");
                return new ResponseDTO { Success = false, MessageKey = "Global_ServiceError_Unknown" };
            }
        }

        public ResponseDTO VerifyGuestRegistration(int guestUserId, string email, string pin)
        {
            try
            {
                using (var db = new memoryGameDBEntities())
                {
                    var pending = db.pendingRegistration
                        .FirstOrDefault(p => p.email == email && p.pin == pin && p.expirationTime > DateTime.Now);

                    if (pending == null)
                        return new ResponseDTO { Success = false, MessageKey = "Global_Error_CodeInvalid" };

                    var userToUpdate = db.user.Find(guestUserId);
                    if (userToUpdate == null || userToUpdate.isGuest != true)
                        return new ResponseDTO { Success = false, MessageKey = "Global_Error_UserNotFound" };

                    userToUpdate.email = pending.email;
                    userToUpdate.password = pending.hashedPassword;
                    userToUpdate.isGuest = false;
                    userToUpdate.verifiedEmail = true;

                    db.pendingRegistration.Remove(pending);
                    db.SaveChanges();

                    return new ResponseDTO { Success = true };
                }
            }
            catch (EntityException ex)
            {
                _logger.LogError($"VerifyRegistration Database Error for email {email}: {ex.Message}");
                return new ResponseDTO { Success = false, MessageKey = "Global_ServiceError_Database" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"VerifyRegistration Error for email {email}: {ex.Message}");
                return new ResponseDTO { Success = false, MessageKey = "Global_ServiceError_Unknown" };
            }
        }
    }
}
