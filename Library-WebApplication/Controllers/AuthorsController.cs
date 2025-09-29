using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Library_WebApplication.Models;
using Library_WebApplication.Busniness_Object;

namespace Library_WebApplication.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly AuthorBO _authorBO;

        public AuthorsController(AppDbContext context, AuthorBO authorBO)
        {
            _context = context;
            _authorBO = authorBO;
        }

        // GET: Authors
        public async Task<IActionResult> Index()
        {
            var Authors =  _authorBO.GetAllAuthors();
            return View(Authors.ToListAsync());
        }

        // GET: Authors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) { return NotFound(); }
            var author = _authorBO.GetDetails((int)id);
            if (author== null) { return NotFound(); }
            return View(await author);
        }

        // GET: Authors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Authors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,DateOfBirth,DateOfDeath")] Author author)
        {
            try
            {
                _authorBO.GetCreated(author);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        // GET: Authors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = _authorBO.GetAuthorById((int)id);
            if (author == null)
            {
                return NotFound();
            }
            return View(await author);
        }

        // POST: Authors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,DateOfBirth,DateOfDeath")] Author author)
        {
            if (id != author.Id)
            {
                return NotFound();
            }
            var Edited = _authorBO.GetEdited(author);
            if (Edited)
                return RedirectToAction(nameof(Index));
            else
                return NotFound();
        }

        // GET: Authors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = _authorBO.GetAuthorById((int)id);
            if (author == null)
            {
                return NotFound();
            }

            return View(await author);
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _authorBO.GetDeleted(id);
            return RedirectToAction(nameof(Index));
        }

        private bool AuthorExists(int id)
        {
            return _context.Authors.Any(e => e.Id == id);
        }
    }
}
