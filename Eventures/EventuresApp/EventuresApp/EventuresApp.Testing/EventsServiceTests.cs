using AutoMapper;
using EventuresApp.Areas.Event.Models;
using EventuresApp.Models;
using EventuresApp.Services;
using EventuresApp.Services.Contracts;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace EventuresApp.Testing
{
    public class UnitTest1
    {
        private IEventsService eventsService;
        public UnitTest1()
        {
            Mock<IRepository<Event>> rep = new Mock<IRepository<Event>>();
            var AllResult = (new List<Event>()
            { new Event { Name = "ev1",TotalTickets=3 },
                new Event { Name = "ev2",TotalTickets=3 },
                new Event { Name = "ev3",TotalTickets=3 }
            }).AsQueryable();
            rep.Setup(x => x.All()).Returns(AllResult);

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Event, eventInfoDtoOutput>();
            });
            var mapper = new Mapper(config);

            this.eventsService = new EventsService(rep.Object, mapper, null);
        }


      
        [Theory]
        [InlineData(1, 2, 2,"ev1")]
        [InlineData(2, 2, 1,"ev3")]
        [InlineData(1, 3, 3,"ev1")]
        [InlineData(3, 1, 1,"ev3")]
        [InlineData(2, 1, 1, "ev2")]
        public void GetMyEvents_ReturnsAdequateCountAndkeepsTheOrder(int? page,int countOfResultsPerPage,int expectedCount,string topName)
        {
            var result = eventsService.GetPortionOfEvents(page, countOfResultsPerPage);
            Assert.Equal(result.First().Name, topName);
            var actualCount = result.Count();

            Assert.Equal(expectedCount, actualCount);
        }
    }
}
