namespace Panda_Exam.Controllers
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Panda_Exam.Data;
    using Panda_Exam.DTOS.Packages;
    using Panda_Exam.Models;
    using Panda_Exam.Models.Enums;
    using System;
    using System.Linq;

    public class PackagesController : Controller
    {
        private Random random;
        private PandaDbContext DB;
        private IMapper mapper;
        public PackagesController(System.Random random, PandaDbContext context, IMapper mapper)
        {
            this.random = random;
            DB = context;
            this.mapper = mapper;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var users = DB.Users.Select(x => new userInfoPackageReceiverDTO
            {
                Name = x.UserName,
                Id = x.Id

            }).ToArray();

            ViewData["Users"] = users;
            return View();
        }

        [HttpPost]
        public IActionResult Create(inputPackageDto package)
        {
            Package newPack = mapper.Map<Package>(package);

            DB.Packages.Add(newPack);
            DB.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Pending()
        {
            var packages = DB.Packages.Where(x => x.Status == Status.Pending).ProjectTo<outputPendingDeliveredPackageDto>(mapper.ConfigurationProvider)
            .ToArray();
            return View(packages);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Ship(int id)
        {
            var package = DB.Packages.FirstOrDefault(x => x.Id == id);
            package.Status = Status.Shipped;
            var shipmantDays = random.Next(20, 40);
            package.EstimatedDeliveryDate = DateTime.UtcNow.AddDays(shipmantDays);
            DB.SaveChanges();
            return RedirectToAction("Pending");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Shipped()
        {
            var packages = DB.Packages.Where(x => x.Status == Status.Shipped).ProjectTo<outputShippedPackageDto>(mapper.ConfigurationProvider)
            .ToArray();
            return View(packages);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Deliver(int id)
        {
            var package = DB.Packages.FirstOrDefault(x => x.Id == id);
            package.Status = Status.Delivered;
            DB.SaveChanges();
            return RedirectToAction("Shipped");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delivered()
        {
            var packages = DB.Packages.Where(x => x.Status == Status.Delivered || x.Status == Status.Acquired).ProjectTo<outputPendingDeliveredPackageDto>(mapper.ConfigurationProvider)
            .ToArray();
            return View(packages);
        }

        [Authorize]
        public IActionResult Details(int id)
        {
            var package = mapper.Map<outputDetailsPackageDto>(DB.Packages.Where(x => x.Id == id).Include(x => x.User).FirstOrDefault());

            if (package is null)
            {
                return View("Error", new ErrorViewModel() { RequestId = $"Package with Id {id} not found!" });
            }
            return View(package);
        }

        [Authorize]
        public IActionResult Acquire(int id)
        {
            Package package = DB.Packages.Include(x => x.User).FirstOrDefault(x => x.Id == id);
            if (package is null)
            {
                return View("Error", new ErrorViewModel() { RequestId = "Unfound Product" });
            }
            if (package.User.UserName != User.Identity.Name)
            {
                return View("Error", new ErrorViewModel() { RequestId = "Unauthorised User" });
            }
            package.Status = Status.Acquired;
            DB.SaveChanges();
            ReceiptsController.Create(package, DB, mapper);
            return RedirectToAction("Index", "Home");
        }
    }
}