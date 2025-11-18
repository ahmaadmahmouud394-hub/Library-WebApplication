using Library_WebApplication.Busniness_Object;
using Library_WebApplication.DTO;
using Library_WebApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Library_WebApplication.Controllers
{
    [Route("api/Admin/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class BooksController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly BooksBO _booksBO;
        public BooksController(AppDbContext context, BooksBO booksbo)
        {
            _context = context;
            _booksBO = booksbo;
        }
        [HttpGet("GetBooks")]
        public async Task<IActionResult> GetBooks()
        {
            var books = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Pubblisher)
                .Include(b => b.Tipology)
                .ToListAsync();

            var json = JsonSerializer.Serialize(books, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            });

            return Content(json, "application/json");
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateBook([FromForm] BookCreateDto dto, [FromForm] IFormFile? CoverFile, [FromServices] IWebHostEnvironment env)
        {
            try
            {
                string? imageUrl = null;

                // Handle file upload
                if (CoverFile != null && CoverFile.Length > 0)
                {
                    var fileName = Path.GetFileNameWithoutExtension(CoverFile.FileName);
                    var extension = Path.GetExtension(CoverFile.FileName);
                    var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{extension}";

                    var uploadPath = Path.Combine(env.WebRootPath, "images", "covers");
                    Directory.CreateDirectory(uploadPath);

                    var filePath = Path.Combine(uploadPath, uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await CoverFile.CopyToAsync(stream);
                    }

                    imageUrl = $"/images/covers/{uniqueFileName}";
                }

                var book = new Book
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    IdAuthor = dto.AuthorId,
                    IdPubblisher = dto.PubblisherId,
                    IdTipology = dto.TipologyId,
                    Price = dto.Price,
                    ImageUrl = imageUrl,
                };

                _context.Books.Add(book);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Book created successfully", book });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating book", error = ex.Message });
            }
        }
    }
}