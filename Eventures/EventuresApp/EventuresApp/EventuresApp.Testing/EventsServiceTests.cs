using AutoMapper;
using EventuresApp.Areas.Event.Models;
using EventuresApp.Models;
using EventuresApp.Services;
using EventuresApp.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
namespace EventuresApp.Testing
{

    public class EventsServiceTests
    {
        private List<Event> dbWithEvents;
        private readonly ServiceProvider DIContainer;
        public EventsServiceTests()
        {
            dbWithEvents = new List<Event>()
            {
                new Event { Name = "ev1",TotalTickets=3 },
                new Event { Name = "invalid",TotalTickets=0 },
                new Event { Name = "ev2",TotalTickets=3 },
                new Event { Name = "ev3",TotalTickets=3 }
            };
            DIContainer = SetServices();
        }
      
        [Theory]
        [InlineData(1, 2, 2,"ev1")]
        [InlineData(2, 2, 1,"ev3")]
        [InlineData(1, 3, 3,"ev1")]
        [InlineData(3, 1, 1,"ev3")]
        [InlineData(2, 1, 1, "ev2")]
        public void GetMyEvents_ReturnsAdequateCountAndkeepsTheOrder(int? page,int countOfResultsPerPage,int expectedCount,string topName)
        {
            var eventsService = DIContainer.GetService<IEventsService>();
            var result = eventsService.GetPortionOfEvents(page, countOfResultsPerPage);
            Assert.Equal(result.First().Name, topName);
            var actualCount = result.Count();
            Assert.Equal(expectedCount, actualCount);
        }
        [Fact]
        public void AddNew_AddsEvent_WhenGivenDTO()
        {
            var eventsService = DIContainer.GetService<IEventsService>();
            var newEvent = new eventCreateDto
            {
                Name = "TestEvent1",
                Place = "TestPlace1",
                TotalTickets = 100,
                PricePerTicket = 12.43m
            };
            eventsService.AddNew(newEvent,"SomeUser").GetAwaiter().GetResult();
            Assert.True(dbWithEvents.Last().Name==newEvent.Name);
        }

        [Fact]
        public void Returns_EventOfUser( )
        {
            string userId = "test3rId";
            dbWithEvents[2].Orders.Add(new Order
            {
                AppUserId = userId
            });

            dbWithEvents[1].Orders.Add(new Order
            {
                AppUserId = userId+"fake"
            });

            var eventsService = DIContainer.GetService<IEventsService>();

            var myEvents = eventsService.GetMyEvents(userId);
            Assert.Single(myEvents);

            Assert.Equal(dbWithEvents[2].Name, myEvents.Single().Name);
        }












        private ServiceProvider SetServices()
        {
            var services = new ServiceCollection();

            Mock<IRepository<Event>> rep = new Mock<IRepository<Event>>();

            rep.Setup(x => x.All()).Returns(ReturnMyEvents());
            rep.Setup(x => x.AddAssync(It.IsAny<Event>())).Returns((Event e)=>  AddMyEvent(e));

            services.AddScoped<IRepository<Event>>(x => rep.Object);

            var config = new MapperConfiguration(cfg => {
                //cfg.AddProfile(new MappingProfile());
                cfg.CreateMap<Event, eventInfoDtoOutput>();
                cfg.CreateMap<eventCreateDto, Event>();
                cfg.CreateMap<Event, myEventDto>();
            });

            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);
            services.AddLogging();
            services.AddScoped<IEventsService, EventsService>();

            return services.BuildServiceProvider();
        }



        private async Task AddMyEvent(Event newEvent)
        {
            dbWithEvents.Add(newEvent);
        }

        private IQueryable<Event> ReturnMyEvents()
        {
            return dbWithEvents.AsQueryable();
        }

    }
}
