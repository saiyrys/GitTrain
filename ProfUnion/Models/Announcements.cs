namespace Profunion.Models
{
    public class Announcements
    {
        public Announcements() { AnnouncementsID = Guid.NewGuid().ToString(); }
        public string AnnouncementsID { get; }
        public string Titles { get; set; }
        public string Descriptions { get; set; }
        public string PhotoPath {  get; set; }
    }
}
