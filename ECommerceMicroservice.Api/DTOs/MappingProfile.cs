using AutoMapper;
using ECommerceMicroservice.Core.Entities;

namespace ECommerceMicroservice.Api.DTOs
{
    public class MappingProfile:Profile
    {

        public MappingProfile()
        {
                CreateMap<Category, CategoryDto>();
                CreateMap<CreateCategoryDto, Category>();
                
                CreateMap<CreateProductDto, Product>();

            // Product entity'sinden ProductDto'ya eşleştirme
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryName,
                           opt => opt.MapFrom(src => src.Category.Name));


        }
    }
}
