namespace EventuresApp.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using EventuresApp.Areas.Event.Models;
    using EventuresApp.Models;
    using EventuresApp.Services.Contracts;
    using Microsoft.Extensions.Logging;
    using X.PagedList;

    public class EventsService : IEventsService
    {
        private readonly IRepository<Event> eventsRepository;
        private readonly IMapper mapper;
        private readonly ILogger<EventsService> logger;

        public EventsService(IRepository<Event> eventsRepository, IMapper mapper, ILogger<EventsService> logger)
        {
            this.eventsRepository = eventsRepository;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task AddNew(eventCreateDto dto, string userName)
        {
            logger.LogCritical($"{userName} is creating an event with name {dto.Name}");
            Event newEvent = mapper.Map<Event>(dto);
            await eventsRepository.AddAssync(newEvent);
            await eventsRepository.SaveChangesAsync();
        }

        public ICollection<myEventDto> GetMyEvents(string currentUserId)
        {
            myEventDto[] myEvents = eventsRepository.All().Where(x => x.Orders.Any(o => o.AppUserId == currentUserId))
               .Select(x => new myEventDto
               {
                   Name = x.Name,
                   Start = x.Start,
                   End = x.End,
                   TotalTicketCount = x.Orders.Where(o => o.AppUserId == currentUserId).Sum(o => o.TicketCount)
               }).OrderByDescending(x => x.TotalTicketCount).ToArray();
            return myEvents;
        }

        public IPagedList<eventInfoDtoOutput> GetPortionOfEvents(int? pageNumber, int eventsPerPage)
        {
            var eventDtos = eventsRepository.All().Where(x => x.TotalTickets > 0)
                .OrderByDescending(x => x.TotalTickets).ProjectTo<eventInfoDtoOutput>(mapper.ConfigurationProvider);

            var portionOfEvents = eventDtos.ToPagedList(pageNumber ?? 1, eventsPerPage);
            return portionOfEvents;


        }
    }
}