using Library_WebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Library_WebApplication.Busniness_Object
{

    public class BooksBO
    {
        private readonly AppDbContext _context;
        public BooksBO(AppDbContext context)
        {
            _context = context;
        }
        public bool GetCreated([Bind("Id,IdAuthor, IdPubblisher, IdTipology, Name, Description, PubblishingDate, Price")] Book book)
        {
            if (book != null) 
            { 
                _context.Add(book);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }

        }
        public IQueryable<Book> GetAllBooks()
        {
            var books = _context.Books;
            return books;
        }
        public async Task<Book> GetDetails(int id)
        {
            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id);
            return book;
        }
        public async Task<Book> GetBookById(int? id)
        {
            var book = _context.Books.FindAsync((int)id);
            return await book;
        }
        public bool GetEdited([Bind("Id,IdAuthor, IdPubblisher, IdTipology, Name, Description, PubblishingDate, Price")] Book books)
        {
            try
            {
                _context.Update(books);
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
            var book = _context.Books.Find(id);
            if (book != null)
            {
                _context.Books.Remove(book);
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
