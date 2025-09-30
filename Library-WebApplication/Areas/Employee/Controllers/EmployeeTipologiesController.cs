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
    [Area("Employee")]
    public class EmployeeTipologiesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly TipologyBO _tipologyBO;

        public EmployeeTipologiesController(AppDbContext context, TipologyBO tipologyBO)
        {
            _context = context;
            _tipologyBO = tipologyBO;
        }

        // GET: Tipologies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Tipologys.ToListAsync());
        }

        // GET: Tipologies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) { return NotFound(); }
            var Tipology = _tipologyBO.GetDetails((int)id);
            if (Tipology == null) { return NotFound(); }
                return View(await Tipology);
        }

        // GET: Tipologies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tipologies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Tipology tipology)
        {
            try
            {
                _tipologyBO.GetCreated(tipology);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        // GET: Tipologies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipology = _tipologyBO.GetTipologyById((int)id);
            if (tipology == null)
            {
                return NotFound();
            }
            return View(await tipology);
        }

        // POST: Tipologies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Tipology tipology)
        {
            if (id != tipology.Id)
            {
                return NotFound();
            }
                var Edited = _tipologyBO.GetEdited(tipology);
            if (Edited)
                return RedirectToAction(nameof(Index));
            else
                return NotFound();
        }

        // GET: Tipologies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipology = _tipologyBO.GetTipologyById((int)id);
            if (tipology == null)
            {
                return NotFound();
            }

            return View(await tipology);
        }

        // POST: Tipologies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _tipologyBO.GetDeleted(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
