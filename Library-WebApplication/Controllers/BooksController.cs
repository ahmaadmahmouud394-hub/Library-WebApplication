using Library_WebApplication.Busniness_Object;
using Library_WebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
            ViewData["TipologyId"] = new SelectList(_context.Tipologys, "Id", "Name");
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "FirstName");
            ViewData["PubblisherId"] = new SelectList(_context.Publishers, "Id", "Name");
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
