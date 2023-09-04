using Administrator.ViewModels;
using AutoMapper;


namespace Administrator.Mappers
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile() {
            CreateMap<DAL.Models.Video, VMVideo>(); 
            CreateMap<VMVideo, DAL.Models.Video>();
            CreateMap<VMTag, DAL.Models.Tag>();
            CreateMap<VMGenre, DAL.Models.Genre>();
            CreateMap<DAL.Models.Genre, VMGenre>();
            CreateMap<DAL.Models.Tag, VMGenre>();
        }
       
    }
}
