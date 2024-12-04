using Microsoft.AspNetCore.Mvc;
using ContactManagement.Data;
using ContactManagement.Models;  
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace ContactManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AuthController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: /Auth/Signup
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("Signup")]
        public IActionResult Signup()
        {
            return View();  
        }

        // POST: /Auth/Signup
        [HttpPost("Signup")]
        public async Task<IActionResult> Signup(UserModel user)
        {
            if (ModelState.IsValid)
            {
                user.PasswordHash = HashPassword(user.PasswordHash);
                user.DateOfJoining = DateTime.Now;

                _db.Users.Add(user);
                await _db.SaveChangesAsync();

                ViewBag.Success = "Signup successful!";       
                Console.WriteLine("Signup successful, redirecting to Login...");

                return RedirectToAction("Login", "Auth");
            }

            ViewBag.Error = "Signup failed. Please try again.";
            return View(user);
        }

        // GET: /Auth/Login
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }




         


        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login(LoginModel login)
        {
            var hashedPassword = HashPassword(login.Password);

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == login.Email && u.PasswordHash == hashedPassword);

            if (user != null)
            {
                
                HttpContext.Session.SetInt32("UserId", user.UserId);

             
                return Json(new { success = true, message = "Login successful" });
            }

            return Json(new { success = false, message = "Invalid login credentials." });
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Auth");
        }



        private string HashPassword(string password)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha512.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}


