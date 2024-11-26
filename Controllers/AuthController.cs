//using Microsoft.AspNetCore.Mvc;
//using ContactManagement.Data;
//using ContactManagement.Models; // Correct namespace for UserModel
//using System.Linq;
//using System.Security.Cryptography;
//using System.Text;
//using Microsoft.EntityFrameworkCore;

//namespace ContactManagement.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class AuthController : Controller
//    {
//        private readonly ApplicationDbContext _db;

//        public AuthController(ApplicationDbContext db)
//        {
//            _db = db;
//        }

//        [HttpGet("signup")]
//        [ApiExplorerSettings(IgnoreApi = true)]
//        public IActionResult Signup()
//        {
//            return Ok("successull loginn");
//        }

//        [HttpGet("login")]
//        [ApiExplorerSettings(IgnoreApi = true)]
//        public IActionResult Login()
//        {
//            return View();
//        }

//        [HttpPost("signup")]
//        public async Task<ActionResult> Signup([FromBody] UserModel user) // This refers to ContactManagement.Models.UserModel
//        {
//            if (ModelState.IsValid)
//            {
//                user.PasswordHash = HashPassword(user.PasswordHash);
//                user.DateOfJoining = DateTime.Now;

//                _db.Users.Add(user);
//                await _db.SaveChangesAsync();

//                return Ok(new { message = "User registered successfully" });
//            }

//            return BadRequest(ModelState);
//        }

//        [HttpPost("login")]
//        public async Task<ActionResult> Login([FromBody] LoginModel login)
//        {
//            var hashedPassword = HashPassword(login.Password);

//            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == login.Email && u.PasswordHash == hashedPassword);

//            if (user != null)
//            {
//                HttpContext.Session.SetInt32("UserId", user.UserId);
//                return Ok(new { message = "Login successful", userId = user.UserId });
//            }

//            return Unauthorized(new { message = "Invalid login credentials" });
//        }

//        private string HashPassword(string password)
//        {
//            using (SHA512 sha512 = SHA512.Create())
//            {
//                var bytes = Encoding.UTF8.GetBytes(password);
//                var hash = sha512.ComputeHash(bytes);
//                return Convert.ToBase64String(hash);
//            }
//        }
//    }

//    public class LoginModel
//    {
//        public string Email { get; set; }
//        public string Password { get; set; }
//    }
//}




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

        // POST: /Auth/Login

        //[HttpPost]
        //[Route("login")]
        //public async Task<ActionResult> Login([FromBody] LoginModel login)
        //{
        //    var hashedPassword = HashPassword(login.Password);

        //    var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == login.Email && u.PasswordHash == hashedPassword);

        //    if (user != null)
        //    {
        //        HttpContext.Session.SetInt32("UserId", user.UserId);

        //        // Redirect to the Contacts page
        //        return RedirectToAction("GetAllContacts", "Contacts");
        //    }

        //    return Unauthorized(new { message = "Invalid login credentials" });
        //}


        /*   [HttpPost]
           [Route("login")]
           public async Task<ActionResult> Login(LoginModel login)
           {
               var hashedPassword = HashPassword(login.Password);

               var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == login.Email && u.PasswordHash == hashedPassword);

               if (user != null)
               {
                   HttpContext.Session.SetInt32("UserId", user.UserId);

                   // Redirect to the Contacts page
                   return RedirectToAction("GetAllContacts", "Contacts");
               }

               ViewBag.Error = "Invalid login credentials.";
               return View();

           }*/



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

                ViewData["FullName"] = user.FullName;

                // Redirect to Contacts page
                return RedirectToAction("GetAllContacts", "Contacts");
            }

            ViewBag.Error = "Invalid login credentials.";
            return View();
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


