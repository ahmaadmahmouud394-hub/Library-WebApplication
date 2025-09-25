using Library_WebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace Library_WebApplication.Busniness_Object
{
    public class AuthenticationBO : Controller
    {
        private readonly AppDbContext _context;
        public AuthenticationBO(AppDbContext context)
        {
            _context = context;
        }
        public bool GetAuthenticated(User user) 
        { 
            bool Email = IsEmailValid(user.UserName);
            user.Password = Encrypt(user.Password);
            bool CheckAuth = false;
            try
            {

                if (Email)
                {
                    user.Email = user.UserName;
                    user.UserName = null;
                    CheckAuth = _context.Users.Where(e => e.Email == user.Email && e.Password == user.Password).SingleOrDefault() != null;
                }
                else
                {
                    CheckAuth = _context.Users.Where(e => e.UserName == user.UserName && e.Password == user.Password).SingleOrDefault() != null;

                }
                return CheckAuth;
            }
            catch 
            {
                return false;
            }

        }

        private bool IsEmailValid(string mail)
        {
            try
            {
                MailAddress eMailAddress = new MailAddress(mail);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        public static class Global
        {
            // set password
            public const string strPassword = "LetMeIn99$";

            // set permutations
            public const String strPermutation = "ouiveyxaqtd";
            public const Int32 bytePermutation1 = 0x19;
            public const Int32 bytePermutation2 = 0x59;
            public const Int32 bytePermutation3 = 0x17;
            public const Int32 bytePermutation4 = 0x41;
        }

        public static string Encrypt(string strData)
        {

            return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(strData)));
            // reference https://msdn.microsoft.com/en-us/library/ds4kkd55(v=vs.110).aspx

        }


        // decoding
        public static string Decrypt(string strData)
        {
            return Encoding.UTF8.GetString(Decrypt(Convert.FromBase64String(strData)));
            // reference https://msdn.microsoft.com/en-us/library/system.convert.frombase64string(v=vs.110).aspx

        }

        // encrypt
        public static byte[] Encrypt(byte[] strData)
        {
            PasswordDeriveBytes passbytes =
            new PasswordDeriveBytes(Global.strPermutation,
            new byte[] { Global.bytePermutation1,
                         Global.bytePermutation2,
                         Global.bytePermutation3,
                         Global.bytePermutation4
            });

            MemoryStream memstream = new MemoryStream();
            Aes aes = new AesManaged();
            aes.Key = passbytes.GetBytes(aes.KeySize / 8);
            aes.IV = passbytes.GetBytes(aes.BlockSize / 8);


            CryptoStream cryptostream = new CryptoStream(memstream,
            aes.CreateEncryptor(), CryptoStreamMode.Write);
            cryptostream.Write(strData, 0, strData.Length);
            cryptostream.Close();
            return memstream.ToArray();
        }

        // decrypt
        public static byte[] Decrypt(byte[] strData)
        {
            PasswordDeriveBytes passbytes =
            new PasswordDeriveBytes(Global.strPermutation,
            new byte[] { Global.bytePermutation1,
                         Global.bytePermutation2,
                         Global.bytePermutation3,
                         Global.bytePermutation4
            });

            MemoryStream memstream = new MemoryStream();
            Aes aes = new AesManaged();
            aes.Key = passbytes.GetBytes(aes.KeySize / 8);
            aes.IV = passbytes.GetBytes(aes.BlockSize / 8);

            CryptoStream cryptostream = new CryptoStream(memstream,
            aes.CreateDecryptor(), CryptoStreamMode.Write);
            cryptostream.Write(strData, 0, strData.Length);
            cryptostream.Close();
            return memstream.ToArray();
        }
    }
}
