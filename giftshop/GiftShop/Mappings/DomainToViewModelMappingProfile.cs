using AutoMapper;
using GiftShop.Entities;
using GiftShop.Models;

namespace GiftShop.Mappings
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile() : base("DomainToViewModelMappings")
        {
            ConfigureMappings();

        }

        private void ConfigureMappings()
        {
            CreateMap<Status, StatusModel>();
            CreateMap<UserType, UserTypeModel>();
            CreateMap<Category, CategoryModel>();
            CreateMap<ProductImage, ProductImageModel>();

            CreateMap<Product, ProductModel>()
                .ForMember(f => f.Category, map => map.MapFrom(m => m.Category))
                .ForMember(f => f.Images, map => map.MapFrom(m => m.ProductImages));
            CreateMap<User, UserModel>()
                .ForMember(f => f.UserType, map => map.MapFrom(m => m.UserType));
            CreateMap<OrderDetail, OrderDetailModel>()
                .ForMember(f => f.Product, map => map.MapFrom(m => m.Product));
            CreateMap<Order, OrderModel>()
                .ForMember(f => f.User, map => map.MapFrom(m => m.User))
                .ForMember(f => f.Status, map => map.MapFrom(m => m.Status))
                .ForMember(f => f.Details, map => map.MapFrom(m => m.OrderDetails));
        }
    }
}