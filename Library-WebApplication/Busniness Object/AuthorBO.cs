using Library_WebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library_WebApplication.Busniness_Object
{
    public class AuthorBO
    {
        private readonly AppDbContext _context;
        public AuthorBO(AppDbContext context)
        {
            _context = context;
        } 
        public IQueryable<Author> GetAllAuthors()
        {
            var Authors = _context.Authors;
            return  Authors;
        }
        public async Task<Author> GetDetails(int id)
        {
            var author = await _context.Authors
                .FirstOrDefaultAsync(m => m.Id == id);
            return author;
        }
        public async Task<bool> GetCreated([Bind("Id,FirstName,LastName,DateOfBirth,DateOfDeath")] Author author)
        {
            _context.Add(author);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<Author> GetAuthorById(int? id)
        {
            var Element = _context.Authors.FindAsync((int)id);
            return await Element;
        }
        public bool GetEdited([Bind("Id,FirstName,LastName,DateOfBirth,DateOfDeath")] Author author)
        {
            try
            {
                _context.Update(author);
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
            var author = _context.Authors.Find(id);
            if (author != null)
            {
                _context.Authors.Remove(author);
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
