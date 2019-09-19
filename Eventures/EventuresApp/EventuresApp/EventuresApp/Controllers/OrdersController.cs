namespace EventuresApp.Controllers
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using EventuresApp.Data;
    using EventuresApp.DTOS.Orders;
    using EventuresApp.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Linq;

    public class OrdersController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly ApplicationDbContext db;
        private readonly IMapper mapper;

        public OrdersController(UserManager<AppUser> userManager, ApplicationDbContext db, IMapper mapper)
        {
            this.userManager = userManager;
            this.db = db;
            this.mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AllOrders()
        {
            orderDto[] orders = db.Orders.ProjectTo<orderDto>(mapper.ConfigurationProvider).ToArray();
            return View(orders);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Order(ticketOrderDto dto)
        {
            var userId = userManager.GetUserId(this.User);
            var currentEvent = db.Events.FirstOrDefault(x => x.Id == dto.EventId);
            if (currentEvent.TotalTickets >= dto.TicketsCount)
            {
                currentEvent.TotalTickets -= dto.TicketsCount;
                var order = new Order
                {
                    AppUserId = userId,
                    EventId = dto.EventId,
                    OrderedOn = DateTime.UtcNow,
                    TicketCount = dto.TicketsCount,
                };
                db.Orders.Add(order);
                db.SaveChanges();
                TempData["Purchase"] = $"Successfully purchased {dto.TicketsCount} tickets for event {dto.EventName}";
                return RedirectToAction("All", "Events", new { area = "Event" });
            }
            else
            {
                TempData["FailedPurchase"] = $"Not Enough Tickets available! Maximum allowed : {currentEvent.TotalTickets}";
                return RedirectToAction("All", "Events", new { area = "Event" });
            }
        }
    }
}