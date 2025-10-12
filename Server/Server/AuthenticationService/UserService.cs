using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

// DATABASE VARIABLES ARE IN SPANISH
// CHANGE TO ENGLISH LATER

// BASE DATOS ESTÁ EN ESPAÑOL
// CAMBIAR A INGLÉS MÁS TARDE

namespace Server.AuthenticationService
{
    internal class UserService: IUserService
    {
        public bool Register(string email, string password)
        {

            if (!Utilities.UserServiceValidation.IsValidEmail(email) || !Utilities.UserServiceValidation.IsValidPassword(password))
            {
                return false; // Invalid email or password format
            }

            using (var db = new memoryGameEntities())
            {
                bool userExists = db.usuario.Any(u => u.correo == email);
                if (userExists)
                {
                    return false; // User already exists
                }

                string hashedPassword = HashPassword(password);
                var newUser = new usuario
                {
                    correo = email,
                    contrasenia = hashedPassword
                };
                db.usuario.Add(newUser);
                db.SaveChanges();
                return true; // Registration successful

            }
        }
        public bool Login(string email, string password)
        {
            using (var db = new memoryGameEntities())
            {
                string hashedPassword = HashPassword(password);
                var user = db.usuario.FirstOrDefault(u => u.correo == email && u.contrasenia == hashedPassword);
                return user != null; // Return true if user is found, otherwise false
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }

        }
    }
}
