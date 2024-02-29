using Microsoft.EntityFrameworkCore;
using Profunion.Models;

namespace Profunion.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Announcements> announcements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(u => new { u.UserID, u.Username, u.PasswordHash, u.FirstName, u.LastName });

            modelBuilder.Entity<Announcements>(entity =>
            {
                entity.HasKey(a => new { a.AnnouncementsID });
                entity.HasIndex(a => a.Titles).IsUnique();
                entity.Property(a => a.Titles).IsRequired().HasMaxLength(50);
                entity.Property(a => a.Descriptions).HasMaxLength(500);

            });
        }



    }/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Profunion.Interfaces;
using Profunion.Models;

namespace Profunion.Services
    {
        public class MessagesService : IMessagesService
        {
            private readonly YourDbContext _dbContext;

            public MessagesService(YourDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<IEnumerable<Message>> GetMessagesForUserAsync(string userId)
            {
                return await _dbContext.Messages
                    .Where(m => m.RecipientID == userId)
                    .ToListAsync();
            }

            public async Task<Message> GetMessageAsync(int messageId)
            {
                return await _dbContext.Messages.FindAsync(messageId);
            }

            public async Task<Message> SendMessageAsync(string senderId, string recipientId, string messageText)
            {
                var message = new Message
                {
                    Texts = messageText,
                    InitiatorID = senderId,
                    RecipientID = recipientId,
                    // Добавьте логику для установки других свойств сообщения, если необходимо
                };

                _dbContext.Messages.Add(message);
                await _dbContext.SaveChangesAsync();

                return message;
            }

            public async Task DeleteMessageAsync(int messageId)
            {
                var message = await _dbContext.Messages.FindAsync(messageId);
                if (message != null)
                {
                    _dbContext.Messages.Remove(message);
                    await _dbContext.SaveChangesAsync();
                }
            }
        }*/
}
