using System;
using System.Linq;
using TodoApp.Services;
using Xunit;

namespace TodoApp.Tests
{
    public class TaskServiceTests
    {
        [Fact]
        public void AddTask_ShouldAddTaskForUser()
        {
            var userService = new UserService();
            var taskService = new TaskService();

            string username = "task_" + Guid.NewGuid().ToString("N");
            string password = "1234";

            userService.Register(username, password);
            var user = userService.Login(username, password);

            taskService.AddTask("Test Task", "Test Description", user!.Id);

            var tasks = taskService.GetTasksByUser(user.Id);

            Assert.Contains(tasks, t => t.Title == "Test Task");
        }

        [Fact]
        public void CompleteTask_ShouldReturnTrue_WhenTaskExists()
        {
            var userService = new UserService();
            var taskService = new TaskService();

            string username = "complete_" + Guid.NewGuid().ToString("N");
            string password = "1234";

            userService.Register(username, password);
            var user = userService.Login(username, password);

            taskService.AddTask("Task To Complete", "Description", user!.Id);
            var task = taskService.GetTasksByUser(user.Id).Last();

            bool result = taskService.CompleteTask(task.Id, user.Id);

            Assert.True(result);
        }

        [Fact]
        public void EditTask_ShouldReturnTrue_WhenTaskExists()
        {
            var userService = new UserService();
            var taskService = new TaskService();

            string username = "edit_" + Guid.NewGuid().ToString("N");
            string password = "1234";

            userService.Register(username, password);
            var user = userService.Login(username, password);

            taskService.AddTask("Old Title", "Old Description", user!.Id);
            var task = taskService.GetTasksByUser(user.Id).Last();

            bool result = taskService.EditTask(task.Id, user.Id, "New Title", "New Description");

            Assert.True(result);
        }

        [Fact]
        public void DeleteTask_ShouldReturnTrue_WhenTaskExists()
        {
            var userService = new UserService();
            var taskService = new TaskService();

            string username = "delete_" + Guid.NewGuid().ToString("N");
            string password = "1234";

            userService.Register(username, password);
            var user = userService.Login(username, password);

            taskService.AddTask("Task To Delete", "Description", user!.Id);
            var task = taskService.GetTasksByUser(user.Id).Last();

            bool result = taskService.DeleteTask(task.Id, user.Id);

            Assert.True(result);
        }
    }
}
