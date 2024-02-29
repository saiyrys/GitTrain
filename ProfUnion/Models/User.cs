namespace Profunion.Models
{
    public class User
    {
        public User() {UserID = Guid.NewGuid().ToString();}
        public string UserID { get; set; }
        public string? Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PasswordHash { get; set; }
        public UserRole userRole { get; set; } 
        public bool IsAdmin => userRole == UserRole.Admin;
    }
    public enum UserRole
    {
        User,
        Admin
    }

}
