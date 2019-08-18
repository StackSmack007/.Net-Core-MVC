using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Panda_Exam.Data;
using Panda_Exam.DTOS.Home;
using Panda_Exam.Models;
using System.Diagnostics;
using System.Linq;

namespace Panda_Exam.Controllers
{
    public class HomeController : Controller
    {
        private UserManager<User> userManager;
        private PandaDbContext db;
        public HomeController(PandaDbContext db,UserManager<User> um)
        {
            this.db = db;
            userManager = um;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = userManager.GetUserAsync(HttpContext.User).Result.Id;
                var packagesOfUser = db.Packages.Where(x => x.UserId == user && x.Status != Models.Enums.Status.Acquired)
                                       .Select(x => new packageIdIndexDto(x.Id, x.Description, x.Status)).ToArray();
                return View("IndexUser", packagesOfUser);
            }
            return View("IndexGuest");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}