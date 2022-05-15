using api.Entities.DTOs;

using AutoMapper;

namespace api.Entities
{
    public class BaseProfile : Profile
    {
        public BaseProfile()
        {
            CreateMap<Category, CategoryDTO>();
            CreateMap<CategoryDTO, Category>();
            
            CreateMap<InsertCategoryDTO, Category>().AfterMap((src, dest) =>
            {
                dest.Id = Guid.NewGuid();
                dest.CreateAt = DateTime.UtcNow;
                dest.UpdateAt = DateTime.UtcNow;
            });
            
            CreateMap<Post, PostDTO>();
            CreateMap<PostDTO, Post>();
            CreateMap<InsertPostDTO, Post>().AfterMap((src, dest) =>
            {
                dest.Id = Guid.NewGuid();
                dest.CreateAt = DateTime.UtcNow;
                dest.UpdateAt = DateTime.UtcNow;
            });
        }
    }
}
    