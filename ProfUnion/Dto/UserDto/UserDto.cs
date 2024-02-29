using Newtonsoft.Json.Converters;
using Profunion.Models;
using System.Text.Json.Serialization;

namespace Profunion.Dto.UserDto
{
    public class UserDto
    {
        public UserDto() { UserID = Guid.NewGuid().ToString(); }
        public string UserID { get; }
        public string? Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PasswordHash { get; set; }
        public UserRole userRole { get; set; }
    }
}
