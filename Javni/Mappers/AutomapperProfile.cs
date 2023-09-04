
using AutoMapper;
using Javni.Models;
using Javni.ViewModels;

namespace Javni.Mappers
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile() {
            CreateMap<DAL.Models.Video, VMVideo>(); 
            CreateMap<VMVideo, DAL.Models.Video>();
            CreateMap<DAL.Models.User, LoginUser>();
            CreateMap<LoginUser, DAL.Models.User>();
            CreateMap<RegisterUser, DAL.Models.User>();
            CreateMap<DAL.Models.User, LoginUser>();
            CreateMap<DAL.Models.User, RegisterUser>();
            CreateMap<DAL.Models.User, JavniUser>();
            CreateMap<JavniUser, DAL.Models.User>();

        }
       
    }
}
