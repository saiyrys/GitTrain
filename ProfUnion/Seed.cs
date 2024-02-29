using Profunion.Data;
using Profunion.Models;
using System.Text;

namespace Profunion
{
    public class Seed
    {
        private readonly DataContext dataContext;

        public Seed(DataContext Context)
        {
            dataContext = Context;
        }

        public void SeedDataContext()
        {
            if (!dataContext.Users.Any())
            {
                var users = new List<User>();
                {
                    new User()
                    {
                        Username = "Test1",
                        FirstName = "test1",
                        LastName = "test11",
                        PasswordHash = "1234",
                    };
                }
                dataContext.Users.AddRange(users);
                dataContext.SaveChanges();
            }
        }
    }
}
