using Library_WebApplication.Models;
using Library_WebApplication.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Pkcs;

namespace Library_WebApplication.Busniness_Object
{
    public class AuthenticationBO
    {
        private readonly AppDbContext _context;
        private readonly Encryption _encryption;
        private readonly EmailValidation _emailValidation;
        
        public AuthenticationBO(AppDbContext context, Encryption encryption, EmailValidation emailValidation)
        {
            _context = context;
            _encryption = encryption;
            _emailValidation = emailValidation;
        }
        public User? GetAuthenticated(User user)
        {

            bool Email = _emailValidation.IsEmailValid(user.UserName);
            user.Password = _encryption.Encrypt(user.Password);
            User? CheckAuth;

                if (Email)
                {
                    user.Email = (user.UserName).ToString();
                    user.UserName = null;
                    CheckAuth = _context.User.Where(e => e.Email == user.Email && e.Password == user.Password).Include(p => p.Role).SingleOrDefault();
                }
                else
                {
                    CheckAuth = _context.User.Where(e => e.UserName == user.UserName && e.Password == user.Password).Include(p => p.Role).SingleOrDefault();

                }
                return CheckAuth;


        }

    }
}
