namespace Panda_Exam.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Panda_Exam.Data;
    using Panda_Exam.DTOS.Receipts;
    using Panda_Exam.Models;
    using System;
    using System.Globalization;
    using System.Linq;

    public class ReceiptsController : Controller
    {
        private UserManager<User> userManager;
        private PandaDbContext DB;
        public ReceiptsController(PandaDbContext db, UserManager<User> um)
        {
            DB = db;
            userManager = um;
        }

        public static void Create(Package package, PandaDbContext DB)
        {
            var receipt = new Receipt
            {
                Fee = package.Weight * 2.67m,
                IssuedOn = DateTime.UtcNow,
                RecipientId = package.UserId,
                PackageId = package.Id,
            };
            DB.Receipts.Add(receipt);
            DB.SaveChanges();
        }

        [Authorize]
        public IActionResult Index()
        {
            var currentUserId = userManager.GetUserAsync(HttpContext.User).Result.Id;
            var receipts = DB.Receipts.Where(x => x.RecipientId == currentUserId).Select(x => new outputReceiptIndexDto
            {
                Id = x.Id,
                Fee = x.Fee,
                IssuedOn = x.IssuedOn.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                Recipient = x.Recipient.UserName
            }).ToArray();

            return View(receipts);
        }

        [Authorize]
        public IActionResult Details(int id)
        {
            var currentUserId = userManager.GetUserAsync(HttpContext.User).Result.Id;
            var receipt = DB.Receipts.Where(x => x.RecipientId == currentUserId && x.Id == id).Select(x => new outputReceiptDetailsDto
            {
                Id=id,
                Recepient = x.Recipient.UserName,
                PackageWeight = x.Package.Weight,
                PackageDescription = x.Package.Description,
                DeliveryAddress = x.Package.ShippingAddress,
                IssuedOn = x.IssuedOn.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                Fee = x.Fee
            }).FirstOrDefault();

            if (receipt is null)
            {
                return View("Error", new ErrorViewModel() { RequestId = $"Receipt with Id {id} not found!" });
            }
            return View(receipt);
        }
    }
}