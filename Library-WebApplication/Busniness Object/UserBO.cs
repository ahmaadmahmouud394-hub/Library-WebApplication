using Library_WebApplication.Models;
using Library_WebApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library_WebApplication.Busniness_Object
{
    public class UserBO
    {
        private readonly AppDbContext _context;
        private readonly Encryption _encryption;

        public UserBO(AppDbContext context, Encryption encryption)
        {
            _context = context;
            _encryption = encryption;
        }
        public bool GetSignedUp(User user)
        {
            if (user != null) { user.IdRole = 1; user.Password = _encryption.Encrypt(user.Password); _context.Add(user); _context.SaveChanges(); return true; }
            else { return false; }
        }
        public bool GetCreated(User user)
        {
            if (user != null) {user.Password = _encryption.Encrypt(user.Password); _context.Add(user); _context.SaveChanges(); return true; }
            else { return false; }
        }
        public async Task<User?> FindUser(int id)
        {
            var UserFound = await _context.User.FindAsync(id);
            return UserFound;
        }
        public bool GetEdited(User user)
        {
            _context.Update(user);
            _context.SaveChanges();
            return true;
        }
        public async Task<User> GetDetails(int id)
        {
            var user = await _context.User.Include(a => a.Role).Include(p => p.Invoices)
                .FirstOrDefaultAsync(m => m.Id == id);
            return user;
        }
    }
}
