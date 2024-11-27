 



using Microsoft.AspNetCore.Mvc;
using ContactManagement.Data;
using ContactManagement.Models; // Correct namespace for UserModel
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
            return View(); // Renders Signup.cshtml
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

                ViewBag.Success = "Signup successful!";      // Logging the redirection for debugging purposes
                Console.WriteLine("Signup successful, redirecting to Login...");

                // Redirect to the Login action
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
            return View(); // Renders Login.cshtml
        }





        //[HttpPost]
        //[Route("login")]
        //public async Task<ActionResult> Login(LoginModel login)
        //{
        //    var hashedPassword = HashPassword(login.Password);

        //    var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == login.Email && u.PasswordHash == hashedPassword);

        //    if (user != null)
        //    {
        //        // Store UserId in Session
        //        HttpContext.Session.SetInt32("UserId", user.UserId);

        //        ViewData["FullName"] = user.FullName;

        //        // Redirect to Contacts page
        //        //return Ok();
        //        return RedirectToAction("GetAllContacts", "Contacts");
        //    }

        //    ViewBag.Error = "Invalid login credentials.";
        //    return View();
        //}



        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login(LoginModel login)
        {
            var hashedPassword = HashPassword(login.Password);

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == login.Email && u.PasswordHash == hashedPassword);

            if (user != null)
            {
                // Store UserId in Session
                HttpContext.Session.SetInt32("UserId", user.UserId);

                // Return JSON with success status
                return Json(new { success = true, message = "Login successful" });
            }

            // Return JSON with failure status
            return Json(new { success = false, message = "Invalid login credentials." });
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            // Clear the session
            HttpContext.Session.Clear();

            // Redirect to the login page
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


