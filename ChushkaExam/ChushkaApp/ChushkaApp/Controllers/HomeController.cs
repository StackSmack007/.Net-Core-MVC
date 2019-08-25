using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ChushkaApp.Models;
using ChushkaApp.Data;
using ChushkaApp.Services;
using ChushkaApp.DTOS.Home;
using AutoMapper.QueryableExtensions;
using AutoMapper;

namespace ChushkaApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ChushkaDBContext context;
        private readonly IMapper mapper;

        public HomeController(ChushkaDBContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        public IActionResult Index()
        {
            Seeder.SeedProducts(context);

            List<outputProductDTO> products = new List<outputProductDTO>();
            if (User.Identity.IsAuthenticated)
            {
                products = context.Products.Where(x=>!x.IsDeleted).ProjectTo<outputProductDTO>(mapper.ConfigurationProvider).ToList();
            }
            return View(products);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

      
    }
}
