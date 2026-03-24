using System.Collections.Generic;
using System.Linq;
using TodoApp.Data;
using TodoApp.Data.Models;

namespace TodoApp.Services
{
    public class UserService
    {
        public bool Register(string username, string password)
        {
            using var context = new AppDbContext();

            bool exists = context.Users.Any(u => u.Username == username);
            if (exists)
            {
                return false;
            }

            var user = new User
            {
                Username = username,
                Password = password
            };

            context.Users.Add(user);
            context.SaveChanges();

            return true;
        }

        public User? Login(string username, string password)
        {
            using var context = new AppDbContext();

            return context.Users.FirstOrDefault(u =>
                u.Username == username && u.Password == password);
        }

        public List<User> GetAllUsers()
        {
            using var context = new AppDbContext();
            return context.Users.ToList();
        }
    }
}