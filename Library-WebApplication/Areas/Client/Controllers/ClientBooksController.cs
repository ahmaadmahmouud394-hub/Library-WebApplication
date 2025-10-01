using Library_WebApplication.Busniness_Object;
using Library_WebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Library_WebApplication.Controllers
{
    [Area("Client")]
    [Authorize(Roles = "Client")]
    public class ClientBooksController : Controller
    {
        private readonly AppDbContext _context;
        private readonly BooksBO _booksBO;

        public ClientBooksController(AppDbContext context, BooksBO booksbo)
        {
            _context = context;
            _booksBO = booksbo;
        }
        [Route("Client/Books")]
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
        public async Task<IActionResult> Details(int id)
        {
            var book = await _booksBO.GetDetails(id);
            return View(book);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = _booksBO.GetBookById((int)id);
            if (book == null)
            {
                return NotFound();
            }

            return View(await book);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _booksBO.GetDeleted(id);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult Edit(Book book)
        {
            if (book == null)
            {
                return View();
            }
            bool Edited = _booksBO.GetEdited(book);
            if (Edited)
            {
                return RedirectToAction("Index");
            }
            else return NotFound();
        }

        public IActionResult Edit(int id)
        {
            ViewData["TipologyId"] = new SelectList(_context.Tipologys, "Id", "Name");
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "FirstName");
            ViewData["PubblisherId"] = new SelectList(_context.Publishers, "Id", "Name");
            return View();
        }

    }
}
