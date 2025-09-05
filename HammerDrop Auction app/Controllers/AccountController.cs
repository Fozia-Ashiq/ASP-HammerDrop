using HammerDrop_Auction_app.Entities;
using HammerDrop_Auction_app.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace HammerDrop_Auction_app.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IEmailSender _emailSender;

        public AccountController(AppDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }


        [HttpPost]
        public async Task<IActionResult> Index(string email, string subject, string message)
        {
            await _emailSender.SendEmailAsync(email, subject, message);
            return View();
        }
        public IActionResult Index()
        {
            return View(_context.UserAccounts.ToList());
        }

        public IActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Registration(RegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var storedCode = TempData["GeneratedCode"]?.ToString();
            var storedEmail = TempData["UserEmail"]?.ToString();

            if (string.IsNullOrEmpty(model.VerificationCode))
            {
                ModelState.AddModelError("VerificationCode", "Please enter the verification code.");
                return View(model);
            }
            if (model.Email != storedEmail)
            {
                ModelState.AddModelError("Email", "This email does not match the one used to get the code.");
                return View(model);
            }

            if (model.VerificationCode != storedCode)
            {
                ModelState.AddModelError("VerificationCode", "Incorrect verification code.");
                return View(model);
            }

            bool emailExists = _context.UserAccounts.Any(u => u.Email == model.Email);
            bool usernameExists = _context.UserAccounts.Any(u => u.UserName == model.UserName);

            if (emailExists)
            {
                ModelState.AddModelError("Email", "Email is already registered.");
                return View(model);
            }

            if (usernameExists)
            {
                ModelState.AddModelError("Username", "Username is already taken.");
                return View(model);
            }


            // create new account
            var account = new UserAccount
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.UserName,
                Password = model.Password,
                VerificationCode = storedCode,
                IsEmailVerified = true
            };

            try
            {
                _context.UserAccounts.Add(account);
                _context.SaveChanges();

                ModelState.Clear();
                TempData["SuccessMessage"] = "Registered successfully. You can now login.";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while saving the data. Please try again.");
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SendCode(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return Json(new { success = false, message = "Please enter a valid email." });
            }

            bool emailExists = _context.UserAccounts.Any(u => u.Email == email);
            if (emailExists)
            {
                return Json(new { success = false, message = "This email is already registered. Please use a different email." });
            }

            var code = new Random().Next(100000, 999999).ToString();
            TempData["GeneratedCode"] = code;
            TempData["UserEmail"] = email;

            var subject = "HammerDrop Verification Code";
            var message = $"Your verification code is: {code}";

            try
            {
                await _emailSender.SendEmailAsync(email, subject, message);
                return Json(new { success = true, message = "Verification code sent successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error sending verification code: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(LogInViewModel model)
        {
            var user = _context.UserAccounts.FirstOrDefault(x =>
        (x.UserName == model.UserNameorEmail || x.Email == model.UserNameorEmail)
        && x.Password == model.Password);


            if (user == null)
            {
                ModelState.AddModelError("", "Email/Username or password is not correct.");
                return View();
            }

            if (!user.IsEmailVerified)
            {
                ModelState.AddModelError("", "Please verify your email before logging in.");
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim("Permission", Permission.User.ToString())
            };

            if (user.Email == "hammerdrop84@gmail.com")
            {
                claims.Add(new Claim("Permission", Permission.Admin.ToString()));
            }

            var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(claimIdentity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            HttpContext.Session.SetString("UserSession", user.UserName);
            return RedirectToAction("Index", "Ads");
        }

        public IActionResult SecurePage()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewBag.MyMessage = HttpContext.Session.GetString("UserSession");
            }
            else
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        [HasPermission(Permission.Admin)]
        public IActionResult Admin()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("UserSession");
            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }
    }
}