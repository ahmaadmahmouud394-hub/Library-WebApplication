using Library_WebApplication.Busniness_Object;
using Library_WebApplication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Library_WebApplication.Controllers
{
    public class UserController : Controller
    {
        private readonly AuthenticationBO _authBO;
        private readonly AppDbContext _context;
        private readonly UserBO _userBO;
        public UserController(AuthenticationBO authBO, AppDbContext context, UserBO userBO)
        {
            _authBO = authBO;
            _context = context;
            _userBO = userBO;
        }

        public IActionResult Index()
        {
            return View();
        }
        //login Method
        [HttpPost]
        public ActionResult Index(User user)
        {
            User? ChechAuth = _authBO.GetAuthenticated(user);
            if (ChechAuth != null) { return RedirectToAction("Index", "Books", new {area = ChechAuth.Role.Name}); }
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
            return View();
        }
        [HttpPost]
        public ActionResult Create(User user)
        {
            var Created = _userBO.GetCreated(user);
            if (Created) { return View(); }
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
