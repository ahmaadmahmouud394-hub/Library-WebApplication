using Library_WebApplication.Busniness_Object;
using Library_WebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Library_WebApplication.Controllers
{
    [Area("Employee")]
    [Authorize(Roles = "Employee")]
    public class EmployeeBooksController : Controller
    {
        private readonly AppDbContext _context;
        private readonly BooksBO _booksBO;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EmployeeBooksController(AppDbContext context, BooksBO booksbo, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _booksBO = booksbo;
            _webHostEnvironment = webHostEnvironment;
        }
        [Route("Employee/Books")]
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
        public async Task<IActionResult> Create(Book book, [FromServices] IWebHostEnvironment webHostEnvironmen, IFormFile? CoverFile)
        {

            if (CoverFile != null && CoverFile.Length > 0)

            {

                try

                {

                    var fileName = Path.GetFileNameWithoutExtension(CoverFile.FileName);

                    var extension = Path.GetExtension(CoverFile.FileName);

                    var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{extension}";

                    // ✅ Use proper webroot path

                    var uploadPath = Path.Combine(webHostEnvironmen.WebRootPath, "images", "covers");

                    Directory.CreateDirectory(uploadPath);

                    var filePath = Path.Combine(uploadPath, uniqueFileName);

                    // Write file

                    using (var stream = new FileStream(filePath, FileMode.Create))

                    {

                        await CoverFile.CopyToAsync(stream);


                    }

                    book.ImageUrl = $"/images/covers/{uniqueFileName}";

                }

                catch (Exception ex)

                {

                    Console.WriteLine($"[ERROR] Upload failed: {ex.Message}");

                }
                bool created = _booksBO.GetCreated(book);
                if (created) { return RedirectToAction("Index"); }
                else { return NotFound(); }
            }
            return RedirectToAction("Index");
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
        public async Task<IActionResult> Edit(Book book, [FromServices] IWebHostEnvironment webHostEnvironmen, IFormFile? CoverFile)
        {

            if (CoverFile != null && CoverFile.Length > 0)

            {

                try

                {

                    var fileName = Path.GetFileNameWithoutExtension(CoverFile.FileName);

                    var extension = Path.GetExtension(CoverFile.FileName);

                    var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{extension}";

                    // ✅ Use proper webroot path

                    var uploadPath = Path.Combine(webHostEnvironmen.WebRootPath, "images", "covers");

                    Directory.CreateDirectory(uploadPath);

                    var filePath = Path.Combine(uploadPath, uniqueFileName);

                    // Write file

                    using (var stream = new FileStream(filePath, FileMode.Create))

                    {

                        await CoverFile.CopyToAsync(stream);


                    }

                    book.ImageUrl = $"/images/covers/{uniqueFileName}";

                }

                catch (Exception ex)

                {

                    Console.WriteLine($"[ERROR] Upload failed: {ex.Message}");

                }

                bool Edited = _booksBO.GetEdited(book);
                if (Edited)
                {
                    return RedirectToAction("Index");
                }
                else return NotFound();

            }
            return NotFound();

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
