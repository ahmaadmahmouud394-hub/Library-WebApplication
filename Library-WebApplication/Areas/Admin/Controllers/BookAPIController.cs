using Library_WebApplication.Busniness_Object;
using Library_WebApplication.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Library_WebApplication.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class BookAPIController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly BooksBO _booksBO;
        public BookAPIController(AppDbContext context, BooksBO booksbo)
        {
            _context = context;
            _booksBO = booksbo;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            var books = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Pubblisher)
                .Include(b => b.Tipology)
                .Include(b => b.Invoices)
                .ToListAsync();

            return Ok(books); // ✅ Returns JSON response
        }
    }
}
