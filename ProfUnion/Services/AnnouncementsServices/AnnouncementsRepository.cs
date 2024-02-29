using Konscious.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Profunion.Data;
using Profunion.Models;
using Profunion.Interfaces;
using System.Text;

namespace Profunion.Services.AnnouncementsServices
{
    public class AnnouncementsRepository : IAnnouncementsRepository
    {
        private readonly DataContext _context;

        public AnnouncementsRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Announcements> GetAnnouncementsByID(string ID)
        {
            return await _context.announcements.Where(a => a.AnnouncementsID == ID).FirstOrDefaultAsync();
        }

        public async Task<Announcements> GetAnnouncementsByTitles(string titles)
        {
            return await _context.announcements.Where(a => a.Titles == titles).FirstOrDefaultAsync();
        }

        public async Task<ICollection<Announcements>> GetAnnouncements()
        {
            return await _context.announcements.OrderBy(a => a.Titles).ToListAsync();
        }

        public async Task<bool> AnnouncementsExists(string titles)
        {
            return await _context.announcements.AnyAsync(a => a.Titles == titles);
        }
        public async Task<bool> CreateAnnouncements(Announcements announcements)
        {
           
            _context.Add(announcements);
            await _context.SaveChangesAsync();

            return await SaveAnnouncements();
        }

       
        public async Task<bool> SaveAnnouncements()
        {
            var saved = await _context.SaveChangesAsync();

            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateAnnouncements(Announcements announcements)
        {
            _context.Update(announcements);

            return await _context.SaveChangesAsync() > 0;


        }
        public async Task<bool> DeleteAnnouncements(Announcements announcements)
        {
            _context.Remove(announcements);

            return await SaveAnnouncements();
        }
    }
}
