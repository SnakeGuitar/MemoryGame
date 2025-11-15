using Server.Utilities;
using System;
using System.Linq;

namespace Server.SessionService
{
    internal class UserService : IUserService
    {
        public ResponseDTO StartRegistration(string email, string password)
        {

            if (!UserServiceValidation.IsValidPassword(password))
            {
                return new ResponseDTO { Success = false, MessageKey = "Global_Error_PasswordInvalid" };
            }

            string hashedPassword = UserServiceUtilities.HashPassword(password);

            try
            {
                using (var db = new memoryGameDBEntities())
                {

                    if (db.user.Any(u => u.email == email))
                    {
                        return new ResponseDTO { Success = false, MessageKey = "Global_Error_EmailInUse" };
                    }

                    var existingPending = db.pendingRegistration
                        .FirstOrDefault(p => p.email == email && p.expirationTime > DateTime.Now);
                    if (existingPending != null)
                    {
                        db.pendingRegistration.Remove(existingPending);
                    }

                    string pin = UserServiceUtilities.GeneratePin();
                    var pendingRegistration = new pendingRegistration
                    {
                        email = email,
                        pin = pin,
                        hashedPassword = hashedPassword,
                        expirationTime = DateTime.Now.AddMinutes(15),
                        createdAt = DateTime.Now
                    };

                    db.pendingRegistration.Add(pendingRegistration);

                    if (!UserServiceUtilities.SendVerificationEmail(email, pin))
                    {
                        return new ResponseDTO { Success = false, MessageKey = "Global_Error_EmailSendFailed" };
                    }

                    db.SaveChanges();

                    return new ResponseDTO { Success = true };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"StartRegistration Error: {ex.Message}");
                return new ResponseDTO { Success = false, MessageKey = "Global_ServiceError_Unknown" };
                throw;
            }
        }
        public ResponseDTO VerifyRegistration(string email, string pin)
        {
            try
            {
                using (var db = new memoryGameDBEntities())
                {
                    var pending = db.pendingRegistration
                        .FirstOrDefault(p => p.email == email && 
                        p.pin == pin &&
                        p.expirationTime > DateTime.Now);

                    if (pending == null)
                    {
                        return new ResponseDTO { Success = false, MessageKey = "Global_Error_CodeInvalid" };
                    }

                    if (pending.expirationTime <= DateTime.Now)
                    {
                        return new ResponseDTO { Success = false, MessageKey = "Global_Error_CodeExpired" };
                    }

                    var newUser = new user
                    {
                        email = email,
                        password = pending.hashedPassword,
                        username = email.Split('@')[0],
                        isGuest = false,
                    };

                    db.user.Add(newUser);
                    db.SaveChanges();

                    UserServiceUtilities.RemovePendingRegistration(email);
                    db.SaveChanges();
                    return new ResponseDTO { Success = true };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"VerifyRegistration Error: {ex.Message}");
                return new ResponseDTO { Success = false, MessageKey = "Global_ServiceError_Unknown" };
                throw;
            }
        }

        public ResponseDTO ResendVerificationCode(string email)
        {
            try
            {
                using (var db = new memoryGameDBEntities())
                {
                    if (db.user.Any(u => u.email == email))
                    {
                        return new ResponseDTO { Success = false, MessageKey = "Global_Error_EmailInUse" };
                    }

                    var pending = db.pendingRegistration.FirstOrDefault(p => p.email == email);
                    if (pending == null)
                    {
                        return new ResponseDTO { Success = false, MessageKey = "Global_Error_RegistrationNotFound" };
                    }

                    string newPin = UserServiceUtilities.GeneratePin();
                    pending.pin = newPin;
                    pending.expirationTime = DateTime.Now.AddMinutes(15);

                    if (!UserServiceUtilities.SendVerificationEmail(email, newPin))
                    {
                        return new ResponseDTO { Success = false, MessageKey = "Global_Error_EmailSendFailed" };
                    }

                    db.SaveChanges();
                    return new ResponseDTO { Success = true };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ResendVerificationCode Error: {ex.Message}");
                return new ResponseDTO { Success = false, MessageKey = "Global_ServiceError_Unknown" };
            }
        }
        public LoginResponse FinalizeRegistration(string email, string username, byte[] avatar)
        {
            try
            {
                using (var db = new memoryGameDBEntities())
                {
                    var user = db.user.FirstOrDefault(u => u.email == email);

                    if (db.user.Any(u => u.username == username))
                    {
                        return new LoginResponse { Success = false, MessageKey = "Global_Error_InvalidUsername" };
                    }

                    if (!UserServiceValidation.IsValidUsername(username))
                    {
                        return new LoginResponse { Success = false, MessageKey = "Global_ValidationUsername_InvalidChars" };
                    }

                    user.username = username;
                    user.avatar = avatar;
                    db.SaveChanges();

                    var utilities = new Utilities.SessionCreation();
                    string token = utilities.CreateSessionToken(user.userId);

                    var userDto = new UserDTO
                    {
                        UserId = user.userId,
                        Username = user.username,
                        Email = user.email
                    };

                    return new LoginResponse
                    {
                        Success = true,
                        SessionToken = token,
                        User = userDto
                    };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"FinalizeRegistration Error {ex.Message}");
                return new LoginResponse { Success = false, MessageKey = "Global_ServiceError_Unknown" };
                throw;
            }
        }

        public LoginResponse Login(string email, string password)
        {
            try
            {
                using (var db = new memoryGameDBEntities())
                {
                    var utilites = new Utilities.SessionCreation();
                    var user = db.user.FirstOrDefault(u => u.email == email);
                    if (user == null)
                    {
                        System.Threading.Thread.Sleep(100);
                        return new LoginResponse { Success = false, MessageKey = "Global_Error_InvalidCredentials" };
                    }

                    if (!BCrypt.Net.BCrypt.Verify(password, user.password))
                    {
                        System.Threading.Thread.Sleep(100);
                        return new LoginResponse { Success = false, MessageKey = "Global_Error_InvalidCredentials"};
                    }
                    ;
                    string token = utilites.CreateSessionToken(user.userId);

                    var userDto = new UserDTO
                    {
                        UserId = user.userId,
                        Username = user.username,
                        Email = user.email
                    };

                    return new LoginResponse
                    {
                        Success = true,
                        SessionToken = token,
                        User = userDto
                    };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Login Error: {ex.Message}");
                return new LoginResponse { Success = false, MessageKey = "Global_ServiceError_Unknown" };
                throw;
            }
        }

        public byte[] GetUserAvatar(string email)
        {
            try
            {
                using (var db = new memoryGameDBEntities())
                {
                    var user = db.user.FirstOrDefault(u => u.email == email);
                    return user?.avatar;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetUserAvatar Error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"InnerException: {ex.InnerException?.Message}");
                throw;
            }
        }

        public ResponseDTO UpdateUserAvatar(string token, byte[] avatar)
        {
            var userId = new SessionCreation().GetUserIdFromToken(token);
            if (userId == null)
            {
                return new ResponseDTO { Success = false, MessageKey = "Global_Error_InvalidToken" };
            }

            if (avatar != null && !ImageValidation.IsValidImage(avatar))
            {
                return new ResponseDTO { Success = false, MessageKey = "Global_Error_ImageInvalid" };
            }

            using (var db = new memoryGameDBEntities())
            {
                var user = db.user.FirstOrDefault(u => u.userId == userId.Value);
                if (user == null)
                {
                    return new ResponseDTO { Success = false, MessageKey = "Global_Error_UserNotFound" };
                }
                user.avatar = avatar;
                db.SaveChanges();
                return new ResponseDTO { Success = true };
            }
        }

        public LoginResponse LoginAsGuest(string guestUsername)
        {
            try
            {
                using (var db = new memoryGameDBEntities())
                {

                    if (!UserServiceValidation.IsValidUsername(guestUsername))
                    {
                        return new LoginResponse { Success = false, MessageKey = "Global_Error_InvalidUsername" };
                    }

                    if (db.user.Any(u => u.username == guestUsername))
                    {
                        return new LoginResponse { Success = false, MessageKey = "Global_Error_UsernameInUse" };
                    }

                    var guestUser = new user
                    {
                        email = $"{Guid.NewGuid()}@guest.local",
                        password = UserServiceUtilities.HashPassword(
                            UserServiceUtilities.GenerateGuestPassword()),
                        username = guestUsername,
                        avatar = null,
                        isGuest = true,
                        verifiedEmail = false
                    };

                    db.user.Add(guestUser);
                    db.SaveChanges();

                    var sessionCreator = new Utilities.SessionCreation();
                    string token = sessionCreator.CreateSessionToken(guestUser.userId);

                    var userDto = new UserDTO
                    {
                        UserId = guestUser.userId,
                        Username = guestUser.username,
                        Email = guestUser.email
                    };

                    return new LoginResponse
                    {
                        Success = true,
                        SessionToken = token,
                        User = userDto
                    };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoginAsGuest Error: {ex.Message}");
                return new LoginResponse { Success = false, MessageKey = "Global_ServiceError_Unknown" };
            }
        }

        public void LogoutGuest(string sessionToken)
        {
            try
            {
                var userId = new Utilities.SessionCreation().GetUserIdFromToken(sessionToken);
                if (userId == null)
                {
                    return;
                }

                using (var db = new memoryGameDBEntities())
                {
                    var guestUser = db.user.FirstOrDefault(u => u.userId == userId.Value && u.isGuest == true);

                    if (guestUser != null)
                    {
                        db.user.Remove(guestUser);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LogoutGuest Error: {ex.Message}");
            }
        }
    }
}