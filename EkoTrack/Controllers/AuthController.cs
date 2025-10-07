using System.Security.Claims;
using BCrypt.Net;
using EkoTrack.Data;
using EkoTrack.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EkoTrack.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _db;
        public AuthController(ApplicationDbContext db) => _db = db;

        // GET: /Auth/Login
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Auth/Login
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, string? returnUrl = null)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "Ingresa email y contraseña.");
                return View();
            }

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                ModelState.AddModelError("", "Credenciales inválidas.");
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()) // <-- clave para [Authorize(Roles="...")]
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        // GET: /Auth/Register
        [HttpGet]
        public IActionResult Register() => View();

        // POST: /Auth/Register
        [HttpPost]
        public async Task<IActionResult> Register(string fullName, string email, string password, Role role)
        {
            if (await _db.Users.AnyAsync(u => u.Email == email))
            {
                ModelState.AddModelError("", "El email ya está registrado.");
                return View();
            }

            var hash = BCrypt.Net.BCrypt.HashPassword(password);
            var user = new AppUser
            {
                FullName = fullName,
                Email = email,
                PasswordHash = hash,
                Role = role
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            TempData["msg"] = "Cuenta creada. Ahora inicia sesión.";
            return RedirectToAction("Login");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}

