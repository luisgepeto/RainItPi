using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using RainIt.Domain.Services;

namespace Web.Services.Controllers
{
    public class UserController : ApiController
    {
        public IEnumerable<User> GetUsers()
        {
            return DataRepository.Users;
        }

         
        public User GetUser(int userId)
        {
            var result = DataRepository.Users.SingleOrDefault(u => u.UserId == userId);
            if (result == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return result;
        }
        [System.Web.Http.HttpGet]
        public IEnumerable<User> SearchUser(string username)
        {
            return DataRepository.Users.Where(u => u.Username == username);
        }
    }

    class DataRepository
    {
        public static User[] Users = new User[]
        {
            new User{UserId = 1, Username = "luis"},
            new User{UserId = 2, Username = "roberto"},
            new User{UserId = 3, Username = "veronica"},
            new User{UserId = 4, Username = "leticia"},
        };
    }
}