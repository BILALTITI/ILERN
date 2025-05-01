using Assigment2.Models;
using Assigment2.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Assigment2.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this._configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var viewModel = new AuthViewModel
            {
                LoginModel = new LoginUserViewModel(),
                RegisterModel = new RegisterUserViewModel()
            };
            var userEmail = User.FindFirstValue(ClaimTypes.Email); // Get logged-in user ID
            var user = await _userManager.FindByEmailAsync(userEmail); // Fetch user from DB

            if (user != null)
            {
                ViewBag.UserName = user.UserName;
                ViewBag.ProfilePicture = user.ProfilePicture; // Set a default image if null
            }
            //else
            //{
            //    //    ViewBag.UserName = "Guest";
            //    //    ViewBag.ProfilePicture = "/images/default.png";
            //    //}
            //}
                return View("Home" ,viewModel);  // Make sure to pass AuthViewModel to the view
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        // POST: /Account/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            // Change password
            var result = await _userManager.ChangePasswordAsync(
                user,
                model.OldPassword,
                model.NewPassword);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            // Re-sign in the user to refresh authentication cookie
            await _signInManager.RefreshSignInAsync(user);

            TempData["SuccessMessage"] = "Your password has been changed successfully.";
            return RedirectToAction("Profile");
        }
        [HttpGet]

        // Edit Profile Action
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound(); // If user doesn't exist
            }
            var roles = await _userManager.GetRolesAsync(user);
            var model = new ProfileViewModel
            {
                UserName = user.UserName,
                Email = user.Email,
                Address = user.Address,
                FullName = user.FullName,
                DateOfBirth = user.DateOfBirth,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive,
                ProfilePicture = user.ProfilePicture,
            Roles = roles.ToList(),
                

            };

            return View(model);
        }  // POST Edit Profile Action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound(); // If user doesn't exist
                }

                user.UserName = model.UserName;
                user.Email = model.Email;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    await _signInManager.RefreshSignInAsync(user); // Refresh the sign-in to apply changes
                    return RedirectToAction("Profile"); // Redirect to Profile Page
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }
        public async Task<IActionResult> Profile()
        {
            // Get the current logged-in user
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound(); // Handle case if user is not found
            }

            // Get the user's roles
            var roles = await _userManager.GetRolesAsync(user);

            // Create a ViewModel with user info
            var profileViewModel = new ProfileViewModel
            {
                UserName = user.UserName,
                Email = user.Email,
                Roles = (List<string>)roles,
                FullName = user.FullName,
                Address = user.Address,
                DateOfBirth = user.DateOfBirth,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive,
                ProfilePicture = user.ProfilePicture,

                Gender = user.Gender,

            };

            return View(profileViewModel); // Pass ViewModel to the view
        }
        [HttpGet]
        public IActionResult Settings()
        {
            // Retrieve user information and pass it to the view model
            var user = _userManager.GetUserAsync(User).Result;
            var model = new SettingsViewModel
            {
                UserName = user.UserName,
                Email = user.Email,

            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSettings(SettingsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                // Update email
                if (user.Email != model.Email)
                {
                    var result = await _userManager.SetEmailAsync(user, model.Email);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError(string.Empty, "Error updating email.");
                        return View("Settings", model);
                    }
                }

                // Change password if provided
                if (!string.IsNullOrEmpty(model.NewPassword) && model.NewPassword == model.ConfirmPassword)
                {
                    var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (!changePasswordResult.Succeeded)
                    {
                        ModelState.AddModelError(string.Empty, "Error changing password.");
                        return View("Settings", model);
                    }
                }

                // Update newsletter preference
                await _userManager.UpdateAsync(user);

                TempData["SuccessMessage"] = "Settings updated successfully!";
                return RedirectToAction("Settings");
            }

            return View("Settings", model);
        }

        public IActionResult Login()
        {

            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterUserViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

     
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Handle file upload
                if (model.ProfilePictureFile != null && model.ProfilePictureFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_env.WebRootPath, "images");

                    // Ensure the folder exists
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProfilePictureFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Validate file type and size
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var fileExtension = Path.GetExtension(model.ProfilePictureFile.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("ProfilePictureFile", "Only image files (jpg, jpeg, png, gif) are allowed.");
                        return View(model);
                    }

                    if (model.ProfilePictureFile.Length > 5 * 1024 * 1024) // Limit size to 5MB
                    {
                        ModelState.AddModelError("ProfilePictureFile", "File size cannot exceed 5MB.");
                        return View(model);
                    }

                    // Save the file
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ProfilePictureFile.CopyToAsync(fileStream);
                    }

                    // Set the file path to the ProfilePicture field
                    model.ProfilePicture = "/images/" + uniqueFileName;
                }
                if (model.Email != null && model.Email.Length > 0)
                {
                    var existingUser = await _userManager.FindByEmailAsync(model.Email);
                    if (existingUser != null)
                    {
                        ModelState.AddModelError("Email", "Email already in use. Enter another one ");
                        return View(model);
                    }
                }
                // Check if username already exists
                var existingUserName = await _userManager.FindByNameAsync(model.UserName);
                if (existingUserName != null)
                {
                    ModelState.AddModelError("UserName", "Username already in use. Enter another one ");
                    return View(model);
                }
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FullName = model.FullName,
                    Address = model.Address,
                    DateOfBirth = model.DateOfBirth,
                    Gender = model.Gender,
                    Role = model.Role,
                    IsActive = true,
                    PhoneNumber = model.PhoneNumber,
                    ProfilePicture = model.ProfilePicture // Save the file path here
                };

                // Create the user
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // Redirect after successful registration
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    // Add errors to model state
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            // Return view with errors
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel); // Return view with validation errors
            }

            // Find user by email
            var normalizedEmail = _userManager.NormalizeEmail(viewModel.Email.Trim());
            var user = await _userManager.FindByEmailAsync(normalizedEmail);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View(viewModel);
            }


            // Validate password
            bool isPasswordValid = await _userManager.CheckPasswordAsync(user, viewModel.Password);
            if (!isPasswordValid)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View(viewModel);
            }

            // Create claims
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim("ProfilePicture", user.ProfilePicture ?? "/images/default-profile.png"),
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            // Add roles as claims
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var claimsIdentity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            // Authenticate user
            await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, claimsPrincipal);

            // Generate JWT Token
            var secretKey = _configuration["JWT:SecretKey"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: signingCredentials
            );

            // Save token in cookie
            Response.Cookies.Append("jwtToken", new JwtSecurityTokenHandler().WriteToken(token), new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(1)
            });

            return RedirectToAction("Index", "Home");
        }
        private int DetermineStepFromErrors(IEnumerable<string> errorFields)
        {
            var step1Fields = new[] { "FullName", "UserName", "Email", "PhoneNumber", "DateOfBirth", "Gender" };
            var step2Fields = new[] { "Password", "ConfirmPassword", "Address", "Role", "ProfilePictureFile" };

            foreach (var field in errorFields)
            {
                if (step1Fields.Contains(field)) return 1;
                if (step2Fields.Contains(field)) return 2;
            }
            return 1;
        }

        
        // Helper methods
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidUsername(string username)
        {
            const string pattern = @"^[a-zA-Z0-9_]{4,20}$";
            return Regex.IsMatch(username, pattern);
        }

        private (bool IsValid, string ErrorMessage) ValidateProfilePicture(IFormFile file)
        {
            if (file == null) return (true, null);

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var maxSize = 5 * 1024 * 1024; // 5MB

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
            {
                return (false, "Invalid file type. Allowed types: JPG, PNG, GIF");
            }

            if (file.Length > maxSize)
            {
                return (false, "File size exceeds 5MB limit");
            }

            // Additional MIME type validation
            var mimeType = file.ContentType.ToLowerInvariant();
            var validMimeTypes = new[] { "image/jpeg", "image/png", "image/gif" };

            if (!validMimeTypes.Contains(mimeType))
            {
                return (false, "Invalid file content type");
            }

            return (true, null);
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim("ProfilePicture", user.ProfilePicture ?? "/default.png")
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration["JWT:SecretKey"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost]
         [ValidateAntiForgeryToken] // Prevent CSRF attacks
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync(); // Sign out from Identity
                                                 // If using refresh token or any other session-based mechanism, invalidate here

            // Clear JWT from cookies if using cookies (optional)
            Response.Cookies.Delete("jwtToken"); // JWT cookie name here

            // Optional: If using refresh tokens, clear them in your data store
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    // Optional: Invalidate any refresh tokens here
                    user.UserName = null;
                    await _userManager.UpdateAsync(user);
                }
            }

            // Redirect to home or login page after logout
            return RedirectToAction("Login", "Account");
        }
        
    }
}
