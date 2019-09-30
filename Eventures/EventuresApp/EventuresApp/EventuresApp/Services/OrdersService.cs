using AutoMapper;
using AutoMapper.QueryableExtensions;
using EventuresApp.DTOS.Orders;
using EventuresApp.Models;
using EventuresApp.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventuresApp.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly IRepository<Order> ordersRepository;
        private readonly IRepository<Event> eventsRepository;
        private readonly IMapper mapper;

        public OrdersService(IRepository<Order> ordersRepository, IRepository<Event> eventsRepository, IMapper mapper)
        {
            this.ordersRepository = ordersRepository;
            this.eventsRepository = eventsRepository;
            this.mapper = mapper;
        }

        public int CheckTicketsInStock(string eventId)
        {
            return eventsRepository.All().Where(x => x.Id == eventId).Select(x => x.TotalTickets).SingleOrDefault();
        }

        public ICollection<OrderDto> GetAll()
        {
            return ordersRepository.All().ProjectTo<OrderDto>(mapper.ConfigurationProvider).ToArray();
        }

        public async Task<bool> OrderTicketsForEvent(string userId, TicketOrderDto dto)
        {
            var currentEvent = await eventsRepository.All().FirstOrDefaultAsync(x => x.Id == dto.EventId);

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
                await ordersRepository.AddAssync(order);
                await ordersRepository.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}