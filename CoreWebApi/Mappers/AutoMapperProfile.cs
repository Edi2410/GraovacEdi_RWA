﻿using CoreWebApi.Models;
using AutoMapper;
using DAL.Models;

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
            CreateMap<Tag, TagDTO>();
            CreateMap<TagDTO, Tag>();
        }
    }
}
