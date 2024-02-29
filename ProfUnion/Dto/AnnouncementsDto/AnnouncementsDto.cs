using Profunion.Models;

namespace Profunion.Dto.AnnouncementsDto
{
    public class AnnouncementsDto
    {
        public AnnouncementsDto() { AnnouncementsID = Guid.NewGuid().ToString(); }
        public string AnnouncementsID { get; }
        public string? Titles { get; set; }
        public string? Descriptions { get; set; }
        public string? PhotoPath { get; set; }
 
    }
}
