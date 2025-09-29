using Library_WebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Library_WebApplication.Busniness_Object;

namespace Library_WebApplication.Controllers
{
    public class BooksController : Controller
    {
        private readonly AppDbContext _context;
        private readonly BooksBO _booksBO;

        public BooksController(AppDbContext context, BooksBO booksbo)
        {
            _context = context;
            _booksBO = booksbo;
        }
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Books.Include(b => b.Author)
                                .Include(b => b.Pubblisher)
                                .Include(b => b.Tipology)
                                .Include(b => b.Invoices);

            return View(await appDbContext.ToListAsync());
        }
        // Get Method
        public IActionResult Create() { 
            
            return View();
        }
        [HttpPost]
        public IActionResult Create(Book book)
        {
           bool created = _booksBO.GetCreated(book);
            if (created) { return RedirectToAction("Index"); }
            else {return NotFound();}
        }

    }
}
