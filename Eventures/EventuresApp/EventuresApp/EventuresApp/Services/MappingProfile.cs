﻿namespace EventuresApp.Services
{
    using AutoMapper;
    using EventuresApp.Areas.Event.Models;
    using EventuresApp.DTOS.Orders;
    using EventuresApp.DTOS.Users;
    using EventuresApp.Models;
    using EventuresApp.Services.MapperAutoConfiguration;
    using System;
    using System.Linq;
    using System.Reflection;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMapToMappings();
            CreateMapFromMappings();
            CreateMap<Event, eventInfoDtoOutput>();
            CreateMap<Order, orderDto>();
            CreateMap<RegisterUserDto, AppUser>();
            CreateMap<AppUser, RoleUserDTO>();
        }

        private void CreateMapToMappings()
        {
            Type[] sourseTypes = Assembly.GetCallingAssembly()
                                         .GetTypes()
                                         .Where(x => x.GetInterfaces()
                                         .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapTo<>)))
                                         .ToArray();
            foreach (Type sType in sourseTypes)
            {
                Type[] targetTypes = sType.GetInterfaces()
                                          .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapTo<>))
                                          .Select(x => x.GetGenericArguments().First())
                                          .ToArray();

                foreach (Type targetType in targetTypes)
                {
                    this.CreateMap(sType, targetType);
                }
            }
        }

        private void CreateMapFromMappings()
        {
            Type[] destTypes = Assembly.GetCallingAssembly()
                                       .GetTypes()
                                       .Where(x => x.GetInterfaces()
                                       .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
                                       .ToArray();

            foreach (Type dType in destTypes)
            {
                Type[] sourceTypes = dType.GetInterfaces()
                                          .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>))
                                          .Select(x => x.GetGenericArguments().First())
                                          .ToArray();

                foreach (Type sType in sourceTypes)
                {
                    this.CreateMap(sType, dType);
                }
            }
        }
    }
}