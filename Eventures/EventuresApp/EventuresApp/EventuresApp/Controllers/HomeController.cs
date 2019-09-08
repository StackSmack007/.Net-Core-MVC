using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EventuresApp.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace EventuresApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            IExceptionHandlerFeature exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            return View();
        }

        public IActionResult Privacy()
        {
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
