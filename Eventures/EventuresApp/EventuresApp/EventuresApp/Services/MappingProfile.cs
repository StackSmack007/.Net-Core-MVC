using AutoMapper;
using EventuresApp.Areas.Event.Models;
using EventuresApp.Models;


namespace EventuresApp.Services
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
   
            CreateMap<eventCreateDto, Event>();
            CreateMap<Event, eventInfoDtoOutput>();
        }
    }
}