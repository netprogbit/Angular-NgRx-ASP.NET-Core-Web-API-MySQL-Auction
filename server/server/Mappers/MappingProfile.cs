using AutoMapper;
using DataLayer.Entities.Auction;
using DataLayer.Entities.Account;
using LogicLayer.Helpers;
using LogicLayer.Models;
using Server.DTOs;
using System.Linq;

namespace Server.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User

            CreateMap<User, UserModel>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(r => r.Role.Name).ToList()));

            CreateMap<UserModel, User>();

            CreateMap<UserDTO, UserModel>().ReverseMap();

            // UserRole

            CreateMap<UserRole, UserRoleModel>().ReverseMap();

            // Role

            CreateMap<Role, RoleModel>().ReverseMap();

            // Token

            CreateMap<TokenModel, TokenDTO>().ReverseMap();

            // Product

            CreateMap<Product, ProductModel>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));

            CreateMap<ProductModel, Product>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));

            CreateMap<ProductModel, ProductDTO>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => PriceHelper.IntToDecimal(src.Price)))
                .ForMember(dest => dest.SellerPrice, opt => opt.MapFrom(src => PriceHelper.IntToDecimal(src.SellerPrice)));

            CreateMap<ProductDTO, ProductModel>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => PriceHelper.DecimalToInt(src.Price)))
                .ForMember(dest => dest.SellerPrice, opt => opt.MapFrom(src => PriceHelper.DecimalToInt(src.SellerPrice)));

            // Category

            CreateMap<Category, CategoryModel>().ReverseMap();
            
            CreateMap<CategoryModel, CategoryDTO>().ReverseMap();

            //  Pagination

            CreateMap(typeof(PaginationModel<>), typeof(PaginationDTO<>));

            // Buy

            CreateMap<BuyModel, BuyDTO>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => PriceHelper.IntToDecimal(src.Price)));

            CreateMap<BuyDTO, BuyModel>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => PriceHelper.DecimalToInt(src.Price)));
        }
    }
}
