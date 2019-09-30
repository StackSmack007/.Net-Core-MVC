using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EventuresApp.Models;
using Microsoft.AspNetCore.Diagnostics;
using System.Text;
using EventuresApp.DTOS.Users;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace EventuresApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
         // LoginUserDto tester = new LoginUserDto { UserName = "asparuh", Password = "alabala" };
         // var test = JsonConvert.SerializeObject(tester);
         // HttpContext.Session.SetString("UserSessionData", test);      
         // IExceptionHandlerFeature exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            return View();
        }

        public IActionResult Privacy()
        {
          // string resFromSession = HttpContext.Session.GetString("UserSessionData");
          // LoginUserDto test = JsonConvert.DeserializeObject<LoginUserDto>(resFromSession);

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
           
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult MyException()
        {

            IExceptionHandlerFeature exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            

            return View(exceptionDetails);
        }


    }
}
