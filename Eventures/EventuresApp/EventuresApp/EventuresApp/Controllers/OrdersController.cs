namespace EventuresApp.Controllers
{
    using EventuresApp.DTOS.Orders;
    using EventuresApp.Models;
    using EventuresApp.Services.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class OrdersController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IOrdersService ordersService;

        public OrdersController(UserManager<AppUser> userManager, IOrdersService ordersService)
        {
            this.userManager = userManager;
            this.ordersService = ordersService;

        }

        [Authorize(Roles = "Admin")]
        public IActionResult AllOrders()
        {
            ICollection<OrderDto> orders = ordersService.GetAll();
            return View(orders);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Order(TicketOrderDto dto)
        {
            var userId = userManager.GetUserId(this.User);
            var successfullOrder = await ordersService.OrderTicketsForEvent(userId, dto);
            if (successfullOrder && dto.TicketsCount>0)
            {
                TempData["Purchase"] = $"Successfully purchased {dto.TicketsCount} tickets for event {dto.EventName}";
            }
            else
            {
                int ticketsAvailable = ordersService.CheckTicketsInStock(dto.EventId);
                TempData["FailedPurchase"] = $"Not Enough Tickets available! Maximum allowed : {ticketsAvailable}";
            }
            return RedirectToAction("All", "Events", new { area = "Event" });
        }
    }
}