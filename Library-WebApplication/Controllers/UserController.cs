using Library_WebApplication.Busniness_Object;
using Microsoft.AspNetCore.Mvc;
using Library_WebApplication.Models;
using Microsoft.AspNetCore.Identity;
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
            bool ChechAuth = _authBO.GetAuthenticated(user);
            if (ChechAuth) { return RedirectToAction("Index", "Books"); }
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
    }
}
