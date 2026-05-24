using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UnityVentures.Data;
using UnityVentures.Models;
using UnityVentures.ViewModels;

namespace UnityVentures.Controllers
{
    public class AccountController : Controller
    {
        private readonly UnityVenturesDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UnityVenturesDbContext context, 
                                UserManager<IdentityUser> userManager,
                                SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Register()
        {
            return View(new BusinessRegistrationViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(BusinessRegistrationViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            // Check if email already exists
            var existingUser = await _userManager.FindByEmailAsync(viewModel.EmailAddress);

            if (existingUser != null)
            {
                ModelState.AddModelError("", "An account with this email already exists.");
                return View(viewModel);
            }

            // Save uploaded image
            string uploadsFolder = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "uploads");

            Directory.CreateDirectory(uploadsFolder);

            string fileName = Guid.NewGuid() +
                Path.GetExtension(viewModel.IdCardWithSelfie.FileName);

            string filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await viewModel.IdCardWithSelfie.CopyToAsync(stream);
            }

            // Create Identity User
            var user = new IdentityUser
            {
                UserName = viewModel.EmailAddress,
                Email = viewModel.EmailAddress,
                PhoneNumber = viewModel.PhoneNumber
            };

            // CREATE USER WITH PASSWORD (THIS WAS MISSING)
            var result = await _userManager.CreateAsync(user, viewModel.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(viewModel);
            }

            // Create Business
            var business = new Business
            {
                BusinessName = viewModel.BusinessName,
                IdNumber = viewModel.IdNumber,
                IdCardWithSelfieImageUrl = "/uploads/" + fileName,
                Balance = 0,
                CreatedAt = DateTime.UtcNow,
                User = user
            };

            _context.Businesses.Add(business);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login");
        }
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var user = await _userManager.FindByEmailAsync(viewModel.EmailAddress);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(viewModel);
            }

            var result = await _signInManager.PasswordSignInAsync(
                user,
                viewModel.Password,
                isPersistent: false,
                lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(viewModel);
            }

            return RedirectToAction("Dashboard", "Business");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
