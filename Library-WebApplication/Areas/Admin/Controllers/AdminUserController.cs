using Library_WebApplication.Busniness_Object;
using Library_WebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Library_WebApplication.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminUserController : Controller
    {
        private readonly AuthenticationBO _authBO;
        private readonly AppDbContext _context;
        private readonly UserBO _userBO;
        public AdminUserController(AuthenticationBO authBO, AppDbContext context, UserBO userBO)
        {
            _authBO = authBO;
            _context = context;
            _userBO = userBO;
        }
        [Route("Admin/User")]
        public IActionResult Index()
        {
            return View();
        }
        //login Method
        [HttpPost]
        public ActionResult Index(User user)
        {
            User? ChechAuth = _authBO.GetAuthenticated(user);
            if (ChechAuth != null) { return RedirectToAction("Index", "Books"); }
            else { return NotFound(); }
        }
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(User user)
        {
                var SignUP = _userBO.GetSignedUp(user);
            if (SignUP) { return RedirectToAction("Index"); }
            else { return NotFound(); }            
        }
        public ActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult Create(User user)
        {
            var Created = _userBO.GetCreated(user);
            if (Created) { return RedirectToAction("Index"); }
            else { return NotFound(); }
        }
        public async Task<ActionResult> Edit(int Id) 
        {
            var user = await _userBO.FindUser(Id);
            if (user == null)
            {
               return NotFound();
            }
            return View(user);
            
        }
        [HttpPost]
        public async Task<ActionResult> Edit( User user)
        {

            if (user == null)
            {
                return NotFound();
            }
            var edited = _userBO.GetEdited(user);
            if (edited) { return RedirectToAction("ShowAll"); }

            return NotFound();

        }
        public async Task<IActionResult> ShowAll()
        {
            var appDbContext = _context.User.Include(b => b.Role);

            return View(await appDbContext.ToListAsync());
        }


    }
}
