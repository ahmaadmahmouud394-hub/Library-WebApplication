using Library_WebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;

namespace Library_WebApplication.Busniness_Object
{
    public class PubblisherBO
    {
        private readonly AppDbContext _context;
        
        public PubblisherBO(AppDbContext context)
        {
            _context = context;
        }
        public IQueryable<Pubblisher> GetAllPublishers()
        {
            var publisher = _context.Publishers;
            return publisher;
        }
        public async Task<Pubblisher> GetDetails(int id)
        {
            var publisher = await _context.Publishers
                .FirstOrDefaultAsync(m => m.Id == id);
            return publisher;
        }
        public async Task<bool> GetCreated([Bind("Id,Name")] Pubblisher publisher)
        {
            _context.Add(publisher);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<Pubblisher> GetPubblisherById(int? id)
        {
            var Element = _context.Publishers.FindAsync((int)id);
            return await Element;
        }
        public bool GetEdited([Bind("Id,Name")] Pubblisher pubblisher)
        {
            try
            {
                _context.Update(pubblisher);
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }
        public bool GetDeleted(int id)
        {
            var pubblisher = _context.Publishers.Find(id);
            if (pubblisher != null)
            {
                _context.Publishers.Remove(pubblisher);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
