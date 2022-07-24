using AutoMapper;
using ProductShop.DataTransferObjects_DTOs_;
using ProductShop.DataTransferObjects_DTOs_.OutputModels;
using ProductShop.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            //Input
            CreateMap<UserInputModel, User>();
            CreateMap<ProductInputModel, Product>();
            CreateMap<CategoryInputModel, Category>();
            CreateMap<CategoryProductInputModel, CategoryProduct>();


            //Output

            CreateMap<Product, ProductInRangeOutputModel>()                
                .ForMember(d => d.SellerFullName, mo => mo.MapFrom(s => s.Seller.FirstName + " " + s.Seller.LastName));


            CreateMap<User, UserOutputModel>()
                .ForMember(d => d.SoldProducts, mo => mo.MapFrom(s => s.ProductsSold.Where(p => p.BuyerId.HasValue)));
            CreateMap<Product, ProductInfoOutputModel>();


            CreateMap<Category, CategoryByProductsCountOutputModel>()
                .ForMember(d => d.CategoryName, mo => mo.MapFrom(s => s.Name))
                .ForMember(d => d.ProductsCount, mo => mo.MapFrom(s => s.CategoryProducts.Count))
                .ForMember(d => d.ProductsAveragePrice, 
                            mo => mo.MapFrom(s => $"{(s.CategoryProducts.Count == 0 ? (decimal)0.00 : s.CategoryProducts.Average(p=>p.Product.Price)):f2}"))               
                .ForMember(d => d.TotalRevenue, mo => mo.MapFrom(s => $"{s.CategoryProducts.Sum(p => p.Product.Price):f2}"));


            CreateMap<Product, ProductOfUserOutputModel>();
            CreateMap<User, SoldProductOutputModel>()
                .ForMember(d => d.ProductsOfUser, mo => mo.MapFrom(s => s.ProductsSold.Where(p => p.BuyerId.HasValue)));
            CreateMap<User, UserWithSoldProductsOutputModel>()
                .ForMember(d => d.ProductSold, mo => mo.MapFrom(s => s));
            CreateMap<List<UserWithSoldProductsOutputModel>, UserCountOutputModel>()
                .ForMember(d => d.Users, mo => mo.MapFrom(s => s));
            
        }

    }
}
