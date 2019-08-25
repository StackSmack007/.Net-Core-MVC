using ChushkaApp.Data;
using ChushkaApp.DTOS.Orders;
using ChushkaApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Linq;

namespace ChushkaApp.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ChushkaDBContext context;

        public OrdersController(ChushkaDBContext context)
        {
            this.context = context;
        }

        [Authorize]
        public IActionResult Order(int id)
        {
            ChushkaUser user = context.Users.Include(x => x.Orders).FirstOrDefault(x => x.UserName == User.Identity.Name);

            if (!context.Products.Where(x => !x.IsDeleted).Any(x => x.Id == id))
            {
                return View("Error", new ErrorViewModel("Unfound Product"));
            }
            user.Orders.Add(new Order { ProductId = id, OrderedOn = DateTime.UtcNow });
            context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult All()
        {
            orderInfoDTO[] orders = context.Orders.Select(x => new orderInfoDTO
            {
                OrderId = x.Id,
                Customer = x.Client.FullName,
                Product = x.Product.Name,
                OrderedOn = x.OrderedOn.ToString("hh:mm dd/MM/yyyy", CultureInfo.InvariantCulture)
            }).ToArray();

            return View(orders);
        }


    }
}
