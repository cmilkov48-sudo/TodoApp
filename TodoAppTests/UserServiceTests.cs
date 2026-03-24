using System;
using TodoApp.Services;
using Xunit;

namespace TodoApp.Tests
{
    public class UserServiceTests
    {
        [Fact]
        public void Register_ShouldReturnTrue_WhenUserIsNew()
        {
            var service = new UserService();

            string username = "user_" + Guid.NewGuid().ToString("N");
            string password = "1234";

            bool result = service.Register(username, password);

            Assert.True(result);
        }

        [Fact]
        public void Register_ShouldReturnFalse_WhenUsernameAlreadyExists()
        {
            var service = new UserService();

            string username = "user_" + Guid.NewGuid().ToString("N");
            string password = "1234";

            service.Register(username, password);
            bool result = service.Register(username, password);

            Assert.False(result);
        }

        [Fact]
        public void Login_ShouldReturnUser_WhenCredentialsAreCorrect()
        {
            var service = new UserService();

            string username = "login_" + Guid.NewGuid().ToString("N");
            string password = "1234";

            service.Register(username, password);
            var user = service.Login(username, password);

            Assert.NotNull(user);
            Assert.Equal(username, user!.Username);
        }

        [Fact]
        public void Login_ShouldReturnNull_WhenCredentialsAreIncorrect()
        {
            var service = new UserService();

            string username = "wrong_" + Guid.NewGuid().ToString("N");
            string password = "1234";

            service.Register(username, password);
            var user = service.Login(username, "wrongpass");

            Assert.Null(user);
        }
    }
}