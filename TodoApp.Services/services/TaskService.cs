using System.Collections.Generic;
using System.Linq;
using TodoApp.Data;
using TodoApp.Data.Models;

namespace TodoApp.Services
{
    public class TaskService
    {
        public void AddTask(string title, string description, int userId)
        {
            using var context = new AppDbContext();

            var task = new TaskItem
            {
                Title = title,
                Description = description,
                UserId = userId,
                IsDone = false
            };

            context.Tasks.Add(task);
            context.SaveChanges();
        }

        public List<TaskItem> GetTasksByUser(int userId)
        {
            using var context = new AppDbContext();

            return context.Tasks
                .Where(t => t.UserId == userId)
                .OrderBy(t => t.Id)
                .ToList();
        }

        public List<TaskItem> GetCompletedTasksByUser(int userId)
        {
            using var context = new AppDbContext();

            return context.Tasks
                .Where(t => t.UserId == userId && t.IsDone)
                .OrderBy(t => t.Id)
                .ToList();
        }

        public List<TaskItem> GetPendingTasksByUser(int userId)
        {
            using var context = new AppDbContext();

            return context.Tasks
                .Where(t => t.UserId == userId && !t.IsDone)
                .OrderBy(t => t.Id)
                .ToList();
        }

        public bool CompleteTask(int id, int userId)
        {
            using var context = new AppDbContext();

            var task = context.Tasks.FirstOrDefault(t => t.Id == id && t.UserId == userId);
            if (task == null)
            {
                return false;
            }

            task.IsDone = true;
            context.SaveChanges();
            return true;
        }

        public bool DeleteTask(int id, int userId)
        {
            using var context = new AppDbContext();

            var task = context.Tasks.FirstOrDefault(t => t.Id == id && t.UserId == userId);
            if (task == null)
            {
                return false;
            }

            context.Tasks.Remove(task);
            context.SaveChanges();
            return true;
        }

        public bool EditTask(int id, int userId, string newTitle, string newDescription)
        {
            using var context = new AppDbContext();

            var task = context.Tasks.FirstOrDefault(t => t.Id == id && t.UserId == userId);
            if (task == null)
            {
                return false;
            }

            task.Title = newTitle;
            task.Description = newDescription;
            context.SaveChanges();
            return true;
        }
    }
}