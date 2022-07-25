using AutoMapper;
using ProductShop.DTO.InputDTOs;
using ProductShop.DTO.OutputDTOs;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            CreateMap<UserInputModel, User>();
            CreateMap<ProductInputModel, Product>();
            CreateMap<CategoryInputModel, Category>();
            CreateMap<CategoryProductInputModel, CategoryProduct>();
          

        }
    }
}
