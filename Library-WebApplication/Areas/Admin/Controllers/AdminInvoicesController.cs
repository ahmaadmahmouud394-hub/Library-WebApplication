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
    public class AdminInvoicesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly InvoicesBO _invoicesBO;

        public AdminInvoicesController(AppDbContext context, InvoicesBO invoicesBO)
        {
            _context = context;
            _invoicesBO = invoicesBO;
        }

        // GET: Invoices
        public async Task<IActionResult> Index()
        {
            var appDbContext = _invoicesBO.GetAllInvoices();
            return View(await appDbContext.ToListAsync());
        }

        // GET: Invoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) { return NotFound(); }
            var invoice = _invoicesBO.GetDetails((int)id);
            if (invoice == null) { return NotFound(); }
            return View(await invoice);
        }

        // GET: Invoices/Create
        public IActionResult Create()
        {
            ViewData["IdBook"] = new SelectList(_context.Books, "Id", "Description");
            ViewData["IdUser"] = new SelectList(_context.User, "Id", "Email");
            return View();
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DateOfTransaction,IdBook,IdUser")] Invoice invoice)
        {
            try
            {
                _invoicesBO.GetCreated(invoice);
                ViewData["IdBook"] = new SelectList(_context.Books, "Id", "Description", invoice.IdBook);
                ViewData["IdUser"] = new SelectList(_context.User, "Id", "Email", invoice.IdUser);
                return View(invoice);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        // GET: Invoices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = _invoicesBO.GetInvoiceById((int)id);
            if (invoice == null)
            {
                return NotFound();
            }
            return View(await invoice);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DateOfTransaction,IdBook,IdUser")] Invoice invoice)
        {
            if (id != invoice.Id)
            {
                return NotFound();
            }
            var Edited = _invoicesBO.GetEdited(invoice);
            if (Edited)
                return RedirectToAction(nameof(Index));
            else
            {
                return NotFound();
            }

            ViewData["IdBook"] = new SelectList(_context.Books, "Id", "Description", invoice.IdBook);
            ViewData["IdUser"] = new SelectList(_context.User, "Id", "Email", invoice.IdUser);
            return View(invoice);
        }

        // GET: Invoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = _invoicesBO.GetInvoiceById((int)id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(await invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _invoicesBO.GetDeleted(id);
            return RedirectToAction(nameof(Index));
        }

        private bool InvoiceExists(int id)
        {
            return _context.Invoices.Any(e => e.Id == id);
        }
    }
}
