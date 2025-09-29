using Library_WebApplication.Models;
using Library_WebApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Library_WebApplication.Busniness_Object
{
    public class TipologyBO
    {
        private readonly AppDbContext _context;
        public TipologyBO(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Tipology> GetDetails(int id) 
        {
            var tipology = await _context.Tipologys
                .FirstOrDefaultAsync(m => m.Id == id);
            return tipology;
        }
        public async Task <bool> GetCreated([Bind("Id,Name")] Tipology tipology)
        {
            _context.Add(tipology);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<Tipology> GetTipologyById(int id)
        {
            var Element = _context.Tipologys.FindAsync(id);
            return await Element;
        }
        public bool GetEdited([Bind("Id,Name")] Tipology tipology)
        {
            try
            {
                _context.Update(tipology);
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
            var tipology =  _context.Tipologys.Find(id);
            if (tipology != null)
            {
                _context.Tipologys.Remove(tipology);
            }

            _context.SaveChanges();
            return true;
        }


    }
}
