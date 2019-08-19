namespace Panda_Exam.Services
{
    using AutoMapper;
    using Panda_Exam.DTOS.Packages;
    using Panda_Exam.DTOS.Receipts;
    using Panda_Exam.Models;
    using System.Globalization;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<inputPackageDto, Package>();
            CreateMap<Package, outputPendingDeliveredPackageDto>()
                              .ForMember(d => d.RecipientName, opt => opt.MapFrom(s => s.User.UserName));

            CreateMap<Package, outputShippedPackageDto>()
                   .ForMember(d => d.RecipientName, opt => opt.MapFrom(s => s.User.UserName))
                   .ForMember(d => d.DeliveryDate, opt => opt.MapFrom(s => s.EstimatedDeliveryDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)));

            CreateMap<Package, outputDetailsPackageDto>()
                 .ForMember(d => d.RecipientName, opt => opt.MapFrom(s => s.User.UserName))
                 .ForMember(d => d.DeliveryDate, opt => opt.MapFrom(s => s.EstimatedDeliveryDate == null ? "N/A" : s.EstimatedDeliveryDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)));

            CreateMap<Package, Receipt>()
                 .ForMember(d => d.Fee, opt => opt.MapFrom(s => s.Weight * 2.67m))
                 .ForMember(d => d.RecipientId, opt => opt.MapFrom(s => s.UserId))
                 .ForMember(d => d.PackageId, opt => opt.MapFrom(s => s.Id));

            CreateMap<Receipt, outputReceiptIndexDto>()
                              .ForMember(d => d.IssuedOn, opt => opt.MapFrom(s => s.IssuedOn.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)))
                              .ForMember(d => d.Recipient, opt => opt.MapFrom(s => s.Recipient.UserName));


            CreateMap<Receipt, outputReceiptDetailsDto>()
                  .ForMember(d => d.Recepient, opt => opt.MapFrom(s => s.Recipient.UserName))
                  .ForMember(d => d.DeliveryAddress, opt => opt.MapFrom(s => s.Package.ShippingAddress))
                 
                  .ForMember(d => d.IssuedOn, opt => opt.MapFrom(s => s.IssuedOn.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)));

        }
    }
}
