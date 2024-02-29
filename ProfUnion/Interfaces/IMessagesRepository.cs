﻿using Profunion.Models;

namespace Profunion.Interfaces
{
    public interface IMessagesRepository
    {
        Task<IEnumerable<Messages>> GetMessagesForUserAsync(string userId);
        Task<Messages> GetMessageAsync(int messageId);
        Task<Messages> SendMessageAsync(string senderId, string recipientId, string messageText);
        Task DeleteMessageAsync(int messageId);
    }
}
