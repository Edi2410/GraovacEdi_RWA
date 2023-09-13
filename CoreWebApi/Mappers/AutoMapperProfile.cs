using CoreWebApi.Models;
using AutoMapper;
using DAL.Models;
using CoreWebApi.DTOs;

namespace CoreWebApi.Mappers
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Video, VideoDTO>();
            CreateMap<VideoDTO, Video>();
            CreateMap<Genre, GenreDTO>();
            CreateMap<GenreDTO, Genre>();
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();
            CreateMap<Tag, TagDTO>();
            CreateMap<TagDTO, Tag>();
        }
    }
}
