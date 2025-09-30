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
    [Area("Admin")]
    public class AdminPubblishersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly PubblisherBO _pubblisherBO;
        public AdminPubblishersController(AppDbContext context, PubblisherBO pubblisherBO)
        {
            _context = context;
            _pubblisherBO = pubblisherBO;
        }

        // GET: Pubblishers
        public async Task<IActionResult> Index()
        {
            var pubblishers = _pubblisherBO.GetAllPublishers();
            return View(await pubblishers.ToListAsync());
        }

        // GET: Pubblishers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) { return NotFound(); }
            var pubblisher = _pubblisherBO.GetDetails((int)id);
            if (pubblisher == null) { return NotFound(); }
            return View(await pubblisher);
        }

        // GET: Pubblishers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pubblishers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Pubblisher pubblisher)
        {
            try
            {
                _pubblisherBO.GetCreated(pubblisher);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        // GET: Pubblishers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pubblisher= _pubblisherBO.GetPubblisherById((int)id);
            if (pubblisher == null)
            {
                return NotFound();
            }
            return View(await pubblisher);
        }

        // POST: Pubblishers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Pubblisher pubblisher)
        {
            if (id != pubblisher.Id)
            {
                return NotFound();
            }
            var Edited = _pubblisherBO.GetEdited(pubblisher);
            if (Edited)
                return RedirectToAction(nameof(Index));
            else
                return NotFound();

        }

        // GET: Pubblishers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pubblisher = _pubblisherBO.GetPubblisherById((int)id);
            if (pubblisher == null)
            {
                return NotFound();
            }

            return View(await pubblisher);
        }

        // POST: Pubblishers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _pubblisherBO.GetDeleted(id);
            return RedirectToAction(nameof(Index));
        }

        private bool PubblisherExists(int id)
        {
            return _context.Publishers.Any(e => e.Id == id);
        }
    }
}
