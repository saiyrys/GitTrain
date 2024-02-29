using AutoMapper;
using Profunion.Dto.AnnouncementsDto;
using Profunion.Models;

namespace Profunion.Helper
{
    public class MappingAnnouncements : Profile
    {
        public MappingAnnouncements() 
        { 
            CreateMap<Announcements, AnnouncementsDto>();

            CreateMap<AnnouncementsDto, Announcements>();
        }
    }
}
