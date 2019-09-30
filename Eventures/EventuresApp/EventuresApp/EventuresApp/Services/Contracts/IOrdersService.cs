using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventuresApp.DTOS.Orders;

namespace EventuresApp.Services.Contracts
{
    public interface IOrdersService
    {
        ICollection<OrderDto> GetAll();
        Task<bool> OrderTicketsForEvent(string userId, TicketOrderDto dto);
        int CheckTicketsInStock(string eventId);
    }
}
