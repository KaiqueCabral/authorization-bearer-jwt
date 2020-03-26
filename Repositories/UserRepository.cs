using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationProject.Models
{
    public class UserRepository
    {
        public static async Task<User> Get(string username, string password)
        {
            List<User> users = new List<User>
            {
                new User { Id = 1, Username = "batman", Password = "123", Role = "Manager" },
                new User { Id = 2, Username = "robin", Password = "123", Role = "Employee" }
            };

            User user = users.Where(x => x.Username.ToLower().Equals(username.ToLower()) && x.Password.Equals(password)).FirstOrDefault();

            return user;
        }
    }
}
