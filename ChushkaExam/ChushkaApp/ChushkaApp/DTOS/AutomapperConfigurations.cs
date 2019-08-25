namespace ChushkaApp.DTOS
{
    using AutoMapper;
    using ChushkaApp.DTOS.Home;
    using ChushkaApp.DTOS.Products;
    using ChushkaApp.DTOS.Users;
    using ChushkaApp.Models;
    using System;

    public class AutomapperConfigurations : Profile
    {

        public AutomapperConfigurations()
        {
            CreateMap<inputUserRegisterDTO, ChushkaUser>();
            CreateMap<Product, outputProductDTO>();
            CreateMap<Product, ProductDetailsDTO>()
                .ForMember(d => d.Type, opt => opt.MapFrom(s => s.Type.ToString()));

            CreateMap<ProductDetailsDTO, Product>()
                .ForMember(d => d.Type, opt => opt.MapFrom(s => Enum.Parse<ProductType>(s.Type)));




        }

    }
}
