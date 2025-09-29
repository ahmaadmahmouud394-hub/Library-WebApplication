using Library_WebApplication.Models;
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
        public bool GetCreated(Book book)
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
    }
}
