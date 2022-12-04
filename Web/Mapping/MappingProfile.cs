using AutoMapper;
using Data.Entities.Shop;
using Web.Areas.Admin.Models.Shop;
using Web.Areas.Admin.Models.Shop.CategoryVM;

namespace Web.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Admin Area

            CreateMap<CreateProductViewModel, Product>();
            CreateMap<Product, CreateProductViewModel>();

            CreateMap<CreateCategoryViewModel, Category>().ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CategoryName));
            CreateMap<Category, CreateCategoryViewModel>().ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Name));

            #endregion
        }
    }
}
