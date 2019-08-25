using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Panda_Exam.Data;
using Panda_Exam.DTOS.Home;
using Panda_Exam.Models;
using System;
using System.Diagnostics;
using System.Linq;

namespace Panda_Exam.Controllers
{
    public class HomeController : Controller
    {
        private UserManager<User> userManager;
        private PandaDbContext db;
      private  IConfiguration cfg;
      private  IServiceProvider serviceProvider;
        public HomeController(PandaDbContext db,UserManager<User> um,IConfiguration cfg,IServiceProvider serviceProvider)
        {
            this.db = db;
            userManager = um;
            this.cfg = cfg;
            this.serviceProvider = serviceProvider;
        }

        public IActionResult Index()
        {
            var cfg2 = serviceProvider.GetService(typeof(IConfiguration));

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