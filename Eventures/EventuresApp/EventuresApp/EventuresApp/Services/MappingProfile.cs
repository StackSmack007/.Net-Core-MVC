namespace EventuresApp.Services
{
    using AutoMapper;
    using EventuresApp.Areas.Event.Models;
    using EventuresApp.DTOS.Orders;
    using EventuresApp.Models;
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<eventCreateDto, Event>();
            CreateMap<Event, eventInfoDtoOutput>();
            CreateMap<Order, orderDto>();
        }
    }
}