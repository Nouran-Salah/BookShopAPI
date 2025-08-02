using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DAl.models;
using DAL.DTO.CategoryDto;
using DAL.DTO.ProductDto;
using DAL.models;

namespace DAL
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<CategoryCreateDto,Category>();
            CreateMap<Category, CategoryReadDto>();
            CreateMap<CategoryReadDto, Category>();
            CreateMap<ProductCreateDto, Product>();
            CreateMap<Product, ProductReadDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.catName));
            CreateMap<ProductUpdateDto, Product>(); 
            CreateMap<Product, ProductUpdateDto>();

        }
    }
}
