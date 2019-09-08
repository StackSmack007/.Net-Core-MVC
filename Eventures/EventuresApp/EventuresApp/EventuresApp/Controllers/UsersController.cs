namespace EventuresApp.Controllers
{
    using EventuresApp.Data;
    using EventuresApp.DTOS.Users;
    using EventuresApp.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using System.Threading.Tasks;
    public class UsersController : Controller
    {
        private SignInManager<AppUser> _signManager;
        private UserManager<AppUser> _userManager;
        private ApplicationDbContext Db;

        public UsersController(ApplicationDbContext db, SignInManager<AppUser> sign, UserManager<AppUser> userManager)
        {
            Db = db;
            _signManager = sign;
            _userManager = userManager;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View("Error", new ErrorViewModel() { RequestId = $"Logged as {User.Identity.Name} Please Log-off first" });
            }
            return this.View();
        }

        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View("Error", new ErrorViewModel() { RequestId = $"Logged as {User.Identity.Name} Please Log-off first" });
            }
            return this.View();
        }

        [HttpPost]
        public IActionResult Login(LoginUserDto user)
        {
            if (ModelState.IsValid)
            {
                var result = _signManager.PasswordSignInAsync(user.UserName, user.Password, user.RememberMe, false).Result;

                if (!result.Succeeded)
                {
                    return Login();
                }
            }
            else
            {
                string[] errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToArray();
                foreach (var error in errors)
                {
                    ModelState.AddModelError("", error);
                }
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserDto newUser)
        {

            if (ModelState.IsValid)
            {
                if (Db.Users.Any(x => x.UserName == newUser.UserName))
                {
                    return View("Error", new ErrorViewModel { RequestId = "UserName taken!" });
                }
                if (newUser.Password != newUser.ConfirmPassword)
                {
                    return View("Error", new ErrorViewModel { RequestId = "Passwords mismatch!" });
                }

                AppUser user = new AppUser()
                {
                    UserName = newUser.UserName,
                    Email = newUser.Email,
                    FirstName = newUser.FirstName,
                    LastName = newUser.LastName,
                    UCN = newUser.UCN,
                };

                var isAdded = await _userManager.CreateAsync(user, newUser.Password);
                if (!isAdded.Succeeded)
                {
                    foreach (var error in isAdded.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View();
                }
                await _signManager.SignInAsync(user, true);
            }
            return View(newUser);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}