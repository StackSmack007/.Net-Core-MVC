using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Panda_Exam.Data;
using Panda_Exam.DTOS.Packages;
using Panda_Exam.Models;
using Panda_Exam.Models.Enums;
using System;
using System.Globalization;
using System.Linq;

namespace Panda_Exam.Controllers
{

    public class PackagesController : Controller
    {
        private System.Random random;
        private PandaDbContext DB;
        public PackagesController(System.Random random, PandaDbContext context)
        {
            this.random = random;
            DB = context;
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
            Package newPack = new Package
            {
                Description = package.Description,
                Weight = package.Weight,
                ShippingAddress = package.ShippingAddress,
                UserId = package.UserId
            };

            DB.Packages.Add(newPack);
            DB.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Pending()
        {
            var packages = DB.Packages.Where(x => x.Status == Status.Pending).Select(x => new outputPendingDeliveredPackageDto
            {
                Id = x.Id,
                Description = x.Description,
                Weight = x.Weight,
                ShippingAddress = x.ShippingAddress,
                RecipientName = x.User.UserName
            }).ToArray();
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
            var packages = DB.Packages.Where(x => x.Status == Status.Shipped).Select(x => new outputShippedPackageDto
            {
                Id = x.Id,
                Description = x.Description,
                Weight = x.Weight,
                RecipientName = x.User.UserName,
                DeliveryDate = x.EstimatedDeliveryDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
            }).ToArray();
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
            var packages = DB.Packages.Where(x => x.Status == Status.Delivered || x.Status == Status.Acquired).Select(x => new outputPendingDeliveredPackageDto
            {
                Id = x.Id,
                Description = x.Description,
                Weight = x.Weight,
                ShippingAddress = x.ShippingAddress,
                RecipientName = x.User.UserName
            }).ToArray();
            return View(packages);
        }

        [Authorize]
        public IActionResult Details(int id)
        {
            var package = DB.Packages.Where(x => x.Id == id)
                .Select(x => new outputDetailsPackageDto
                {
                    Description = x.Description,
                    Weight = x.Weight,
                    Status = x.Status.ToString(),
                    ShippingAddress = x.ShippingAddress,
                    RecipientName = x.User.UserName,
                    DeliveryDate = x.EstimatedDeliveryDate == null ? "N/A" : x.EstimatedDeliveryDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
                }).FirstOrDefault();
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
            ReceiptsController.Create(package, DB);
            return RedirectToAction("Index", "Home");
        }
    }
}