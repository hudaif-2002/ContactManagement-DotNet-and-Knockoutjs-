using Microsoft.AspNetCore.Mvc;
using ContactManagement.Data;
using ContactManagement.Models;  
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace ContactManagement.Controllers;

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

    // GET: /Auth/Login
    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpGet("Login")]
    public IActionResult Login()
    {
        return View();
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpGet("Logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login", "Auth");
    }

    // POST: /Auth/Signup
    [HttpPost("Signup")]
    public async Task<IActionResult> Signup(UserModel user)
    {
        if (ModelState.IsValid)
        {
            try
            {
                // Check if user already exists
                var existingUser = await _db.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
                if (existingUser != null)
                {
                    // If user exists, return an error message
                    return Json(new { success = false, message = "User already exists with this email." });
                }
                // Hash the password before saving
                user.PasswordHash = HashPassword(user.PasswordHash);
                user.DateOfJoining = DateTime.Now;
                // Save the new user
                _db.Users.Add(user);
                await _db.SaveChangesAsync();
                // Return success response
                return Json(new { success = true, message = "Signup successful! Redirecting to login page..." });
            }
            catch (Exception ex)
            {
                // Handle unexpected errors and log them
                Console.WriteLine($"Error during signup: {ex.Message}");
                return Json(new { success = false, message = "An unexpected error occurred. Please try again later." });
            }
        }
        else
        {
            // if ModelState is invalid
            return Json(new { success = false, message = "Please check your input fields." });
        }
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