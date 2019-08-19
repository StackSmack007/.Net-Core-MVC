namespace Panda_Exam.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Panda_Exam.Data;
    using Panda_Exam.DTOS.Users;
    using Panda_Exam.Models;
    using System.Linq;
    using System.Threading.Tasks;

    public class UsersController : Controller
    {
        private SignInManager<User> _signManager;
        private UserManager<User> _userManager;
        private PandaDbContext Db;
        private RoleManager<IdentityRole> _roleManager;
        public UsersController(PandaDbContext db, SignInManager<User> sign, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            Db = db;
            _signManager = sign;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View("Error",new ErrorViewModel() {RequestId= $"Logged as {User.Identity.Name} Please Log-off first" });
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
        public async Task<IActionResult> Login(LoginUserDTO user)
        {
            if (ModelState.IsValid)
            {
                var result = await _signManager.PasswordSignInAsync(user.UserName, user.Password, false, false);

                if (!result.Succeeded)
                {
                    return RedirectToAction("Login");
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserDto newUser)
        {
            if (!Db.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole("User"));
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (ModelState.IsValid)
            {
                if (Db.Users.Any(x => x.UserName == newUser.UserName))
                {
                    return View("Error", new ErrorViewModel { RequestId = "UserName taken!" });
                }
                if (newUser.Password != newUser.VerifyPassword)
                {
                    return View("Error", new ErrorViewModel { RequestId = "Passwords mismatch!" });
                }

                User user = new User() { UserName = newUser.UserName, Email = newUser.Email };

                var isAdded = await _userManager.CreateAsync(user, newUser.Password);
                if (isAdded.Succeeded)
                {
                    await _signManager.SignInAsync(user, true);

                    await _userManager.AddToRoleAsync(user, "User");
                    if (!Db.Users.Any(x=>x.Id!=user.Id))
                    {
                        await _userManager.AddToRoleAsync(user, "Admin");
                    }
                }
                else
                {
                    foreach (var error in isAdded.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View();
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}     