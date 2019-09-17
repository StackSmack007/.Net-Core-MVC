namespace EventuresApp.Controllers
{
    using AutoMapper;
    using EventuresApp.Data;
    using EventuresApp.DTOS.Users;
    using EventuresApp.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    public class UsersController : Controller
    {
        private SignInManager<AppUser> _signInManager;
        private UserManager<AppUser> _userManager;
        private readonly IMapper mapper;
        private ApplicationDbContext Db;

        public UsersController(ApplicationDbContext db, SignInManager<AppUser> sign, UserManager<AppUser> userManager, IMapper mapper)
        {
            Db = db;
            _signInManager = sign;
            _userManager = userManager;
            this.mapper = mapper;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View("Error", new ErrorViewModel() { RequestId = $"Logged as {User.Identity.Name} Please Log-off first" });
            }
            ViewData["ExternalLoginProviders"] = _signInManager.GetExternalAuthenticationSchemesAsync().Result.ToArray();

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
                var result = _signInManager.PasswordSignInAsync(user.UserName, user.Password, user.RememberMe, false).Result;

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
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", values: new { returnUrl });
            //   var redirectUrl = "/Users/ExternalLoginCallback";
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            var result = new ChallengeResult(provider, properties);
            return result;
        }

        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null)
        {
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            string ErrorMessage = string.Empty;
            if (info == null)
            {
                ErrorMessage = "Error loading external login information.";
                return Content("Login");
            }
            var loginProvider = info.LoginProvider;
            var result = await _signInManager.ExternalLoginSignInAsync(loginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            if (result.IsLockedOut)
            {
                return Content("Locked Out!");
            }
            // If the user does not have an account, then ask the user to create an account.
            RegisterUserDto newUser = new RegisterUserDto();
            if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email)) newUser.Email = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (info.Principal.HasClaim(c => c.Type == ClaimTypes.GivenName)) newUser.UserName = info.Principal.FindFirstValue(ClaimTypes.GivenName);
            if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Name)) newUser.FirstName = info.Principal.FindFirstValue(ClaimTypes.Name);
            if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Surname)) newUser.LastName = info.Principal.FindFirstValue(ClaimTypes.Surname);
            TempData["Provider"] = loginProvider;
            return RedirectToAction("RegisterFromExternalProvider", newUser);
        }

        public IActionResult RegisterFromExternalProvider(RegisterUserDto newUser, string provider = null)
        {
            return View(newUser);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterFromExternalProvider(RegisterUserDto newUser)
        {
            string returnUrl = "/";
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return Content("Error loading external login information during confirmation.");
            }
            if (ModelState.IsValid)
            {
                var user = mapper.Map<AppUser>(newUser);
                var result = await _userManager.CreateAsync(user, newUser.Password);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(newUser);
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
                await _signInManager.SignInAsync(user, true);
            }
            return View(newUser);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}