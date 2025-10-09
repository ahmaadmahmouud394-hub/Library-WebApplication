using Library_WebApplication.Busniness_Object;
using Library_WebApplication.Models;
using Library_WebApplication.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Diagnostics;
using System.Security.Claims;

namespace Library_WebApplication.Controllers
{
    public class UserController : Controller
    {
        private readonly AuthenticationBO _authBO;
        private readonly AppDbContext _context;
        private readonly UserBO _userBO;
        private readonly JwtService _jwtService;
        public UserController(AuthenticationBO authBO, AppDbContext context, UserBO userBO, JwtService jwtService)
        {
            _authBO = authBO;
            _context = context;
            _userBO = userBO;
            _jwtService = jwtService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Index(User user)
        {
            User? checkAuth = _authBO.GetAuthenticated(user);

            if (checkAuth != null)
            {
                HttpContext.Session.SetString("UserId", checkAuth.Id.ToString());
                HttpContext.Session.SetString("Role", checkAuth.Role.Name.ToString());

                var claims = new List<Claim>

                {
                    new Claim(ClaimTypes.NameIdentifier, checkAuth.Id.ToString()),
                    new Claim(ClaimTypes.Name, $"{checkAuth.FirstName} {checkAuth.LastName}"),
                    new Claim(ClaimTypes.Email, checkAuth.Email),
                    new Claim(ClaimTypes.Role, checkAuth.Role.Name)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                var authProps = new AuthenticationProperties
                {
                    IsPersistent = false,
                    ExpiresUtc = false
            ? DateTimeOffset.UtcNow.AddDays(14)
            : DateTimeOffset.UtcNow.AddHours(1)
                };

                await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                authProps);

                // ✅ Generate JWT token
                var token = await _jwtService.GenerateToken(checkAuth, checkAuth.Role.Name);
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine(token);
                Console.WriteLine("-------------------------------------------");
                var checktoken = _jwtService.ValidateToken(token);
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine(checktoken);
                Console.WriteLine("-------------------------------------------");

                // ✅ Store token in cookie (optional, so MVC app can u se it)
                Response.Cookies.Append("AuthToken", token, new CookieOptions
                {
                    HttpOnly = true,   // cannot be accessed by JS
                    Secure = true,     // use only with HTTPS
                    SameSite = SameSiteMode.Strict
                });

                Response.Cookies.Append("Authorization", checkAuth.Role.Name);

                // ✅ Redirect user based on role (old behavior)
                return RedirectToAction("Index", "Books", new { area = checkAuth.Role.Name });
            }
            else
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }
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
            else { return RedirectToAction("AccessDenied"); }            
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
        public IActionResult AccessDenied()
        {
            return View();
        }
        public async Task<ActionResult> Edit(int Id) 
        {
            var user =  _userBO.FindUser(Id);
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
