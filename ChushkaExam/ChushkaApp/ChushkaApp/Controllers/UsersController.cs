namespace ChushkaApp.Controllers
{
    using AutoMapper;
    using ChushkaApp.Data;
    using ChushkaApp.DTOS.Users;
    using ChushkaApp.Models;
    using ChushkaApp.Services;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using System.Threading.Tasks;

    public class UsersController : Controller
    {
        private readonly SignInManager<ChushkaUser> _signInManager;
        private readonly UserManager<ChushkaUser> _userManager;
        private IMapper mapper;
        private readonly ChushkaDBContext context;
        private readonly RoleManager<IdentityRole> roleManager;

        public UsersController(SignInManager<ChushkaUser> sm, UserManager<ChushkaUser> um, IMapper mapper, ChushkaDBContext context, RoleManager<IdentityRole> roleManager)
        {
            this._signInManager = sm;
            this._userManager = um;
            this.mapper = mapper;
            this.context = context;
            this.roleManager = roleManager;
        }

        public IActionResult Register()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View();
            }
            return View("Error", new ErrorViewModel("Please Log Off First!"));
        }

        [HttpPost]
        public async Task<IActionResult> Register(inputUserRegisterDTO dto)
        {
            if (ModelState.IsValid)
            {
                Seeder.SeedRoles(roleManager, context);
                if (dto.Password != dto.VerifyPassword)
                {
                    return View("Error", new ErrorViewModel("Passwords Missmatch!"));
                }
                if (context.Users.Any(x => x.UserName == dto.UserName))
                {
                    return View("Error", new ErrorViewModel("UserNameTaken"));
                }

                ChushkaUser newUser = mapper.Map<ChushkaUser>(dto);

                var isAdded = await _userManager.CreateAsync(newUser, dto.Password);
                if (isAdded.Succeeded)
                {
                    if (context.Users.Count() <= 1)
                    {
                        await _userManager.AddToRoleAsync(newUser, "Admin");
                    }
                    await _userManager.AddToRoleAsync(newUser, "User");

                    await _signInManager.SignInAsync(newUser, true);
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

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View();
            }
            return View("Error", new ErrorViewModel("Please Log Off First!"));
        }

        [HttpPost]
        public async Task<IActionResult> Login(inputUserLoginDTO user)
        {
            if (ModelState.IsValid)
            {
                var isSigned = await _signInManager.PasswordSignInAsync(user.UserName, user.Password, true, false);

                if (!isSigned.Succeeded)
                {
                    ModelState.AddModelError("", "UserName or PasswordMismatch");
                    return View();
                }
            }
            return RedirectToAction("Index", "Home");
        }

    }
}