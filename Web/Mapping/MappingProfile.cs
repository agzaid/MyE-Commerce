using AutoMapper;
using Data.Entities.Shop;
using Data.Entities.User;
using Web.Areas.Admin.Models.Shop;
using Web.Areas.Admin.Models.Shop.CategoryVM;
using Web.Areas.Admin.Models.Users;

namespace Web.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Admin Area

            //product
            CreateMap<CreateProductViewModel, Product>();
            CreateMap<Product, CreateProductViewModel>();

            //category
            CreateMap<CreateCategoryViewModel, Category>().ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CategoryName));
            CreateMap<Category, CreateCategoryViewModel>().ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Name));

            //customer
            CreateMap<Customer, IndexCustomersViewModel>();
            CreateMap<IndexCustomersViewModel, Customer>();

            #endregion
        }
    }
}
