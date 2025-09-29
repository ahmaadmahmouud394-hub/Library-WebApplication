using Library_WebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library_WebApplication.Busniness_Object
{
    public class InvoicesBO
    {
        private readonly AppDbContext _context;
        public InvoicesBO(AppDbContext context)
        {
            _context = context;
        }
        public IQueryable<Invoice> GetAllInvoices()
        {
            var appDbContext = _context.Invoices.Include(i => i.Book).Include(i => i.User);
            return appDbContext;
        }
        public async Task<Invoice> GetDetails(int id)
        {
            var invoice = await _context.Invoices
                .FirstOrDefaultAsync(m => m.Id == id);
            return invoice;
        }
        public async Task<bool> GetCreated([Bind("Id,DateOfTransaction,IdBook,IdUser")] Invoice invoice)
        {
            _context.Add(invoice);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<Invoice> GetInvoiceById(int? id)
        {
            var Element = _context.Invoices.FindAsync((int)id);
            return await Element;
        }
        public bool GetEdited([Bind("Id,DateOfTransaction,IdBook,IdUser")] Invoice invoice)
        {
            try
            {
                _context.Update(invoice);
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }
        public bool GetDeleted(int id)
        {
            var invoice = _context.Invoices.Find(id);
            if (invoice != null)
            {
                _context.Invoices.Remove(invoice);
            }

            _context.SaveChanges();
            return true;
        }
    }
}
