using Profunion.Models;
namespace Profunion.Interfaces
{
    public interface IAnnouncementsRepository
    {
        Task<ICollection<Announcements>> GetAnnouncements();
        Task<Announcements> GetAnnouncementsByID(string ID);
        Task<Announcements> GetAnnouncementsByTitles(string titles);
        Task<bool> AnnouncementsExists(string titles);
        Task<bool> CreateAnnouncements(Announcements announcements);
        Task<bool> UpdateAnnouncements(Announcements announcements);
        Task<bool> DeleteAnnouncements(Announcements announcements);
        Task<bool> SaveAnnouncements();
    }
}
