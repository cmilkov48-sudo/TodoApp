using System;
using System.Collections.Generic;
using TodoApp.Data.Models;
using TodoApp.Services;

namespace TodoApp
{
    internal class Program
    {
        private static readonly UserService userService = new UserService();
        private static readonly TaskService taskService = new TaskService();

        private static User? loggedUser = null;

        static void Main(string[] args)
        {
            while (true)
            {
                if (loggedUser == null)
                {
                    ShowWelcomeMenu();
                }
                else
                {
                    ShowUserMenu();
                }
            }
        }

        private static void ShowWelcomeMenu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("====================================");
            Console.WriteLine("            TODO APP");
            Console.WriteLine("====================================");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("1. Register");
            Console.WriteLine("2. Login");
            Console.WriteLine("0. Exit");
            Console.Write("Choose option: ");

            string? input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    Register();
                    break;
                case "2":
                    Login();
                    break;
                case "0":
                    Environment.Exit(0);
                    break;
                default:
                    ShowMessage("Invalid option.");
                    break;
            }
        }

        private static void Register()
        {
            Console.Clear();
            Console.WriteLine("=== Register ===");

            Console.Write("Username: ");
            string username = Console.ReadLine() ?? string.Empty;

            Console.Write("Password: ");
            string password = Console.ReadLine() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ShowMessage("Username and password cannot be empty.");
                return;
            }

            bool success = userService.Register(username, password);

            if (success)
            {
                ShowMessage("Registration successful.");
            }
            else
            {
                ShowMessage("Username already exists.");
            }
        }

        private static void Login()
        {
            Console.Clear();
            Console.WriteLine("=== Login ===");

            Console.Write("Username: ");
            string username = Console.ReadLine() ?? string.Empty;

            Console.Write("Password: ");
            string password = Console.ReadLine() ?? string.Empty;

            User? user = userService.Login(username, password);

            if (user == null)
            {
                ShowMessage("Invalid username or password.");
                return;
            }

            loggedUser = user;
            ShowMessage($"Welcome, {loggedUser.Username}!");
        }

        private static void ShowUserMenu()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("====================================");
            Console.WriteLine($"User: {loggedUser!.Username}");
            Console.WriteLine("====================================");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("1. Add Task");
            Console.WriteLine("2. View Tasks");
            Console.WriteLine("3. Manage Tasks");
            Console.WriteLine("4. Logout");
            Console.ResetColor();

            Console.Write("Choose option: ");

            string? input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    AddTask();
                    break;
                case "2":
                    ShowTasks(taskService.GetTasksByUser(loggedUser!.Id));
                    break;
                case "3":
                    ManageTasksMenu();
                    break;
                case "4":
                    loggedUser = null;
                    break;
            }
        }

        private static void AddTask()
        {
            Console.Clear();
            Console.WriteLine("=== Add Task ===");

            Console.Write("Title: ");
            string title = Console.ReadLine() ?? string.Empty;

            Console.Write("Description: ");
            string description = Console.ReadLine() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(title))
            {
                ShowMessage("Title cannot be empty.");
                return;
            }

            taskService.AddTask(title, description, loggedUser!.Id);
            ShowMessage("Task added successfully.");
        }

        private static void ShowTasks(List<TaskItem> tasks)
        {
            Console.Clear();
            Console.WriteLine("=== Tasks ===");

            if (tasks.Count == 0)
            {
                Console.WriteLine("No tasks found.");
            }
            else
            {
                foreach (var task in tasks)
                {
                    Console.WriteLine("------------------------------------");
                    Console.WriteLine($"Id: {task.Id}");
                    Console.WriteLine($"Title: {task.Title}");
                    Console.WriteLine($"Description: {task.Description}");
                    Console.WriteLine($"Done: {task.IsDone}");
                }
            }

            Console.WriteLine("------------------------------------");
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }

        private static void CompleteTask()
        {
            Console.Clear();
            Console.WriteLine("=== Complete Task ===");

            Console.Write("Enter task id: ");
            bool parsed = int.TryParse(Console.ReadLine(), out int id);

            if (!parsed)
            {
                ShowMessage("Invalid id.");
                return;
            }

            bool success = taskService.CompleteTask(id, loggedUser!.Id);

            if (success)
            {
                ShowMessage("Task completed.");
            }
            else
            {
                ShowMessage("Task not found.");
            }
        }

        private static void EditTask()
        {
            Console.Clear();
            Console.WriteLine("=== Edit Task ===");

            Console.Write("Enter task id: ");
            bool parsed = int.TryParse(Console.ReadLine(), out int id);

            if (!parsed)
            {
                ShowMessage("Invalid id.");
                return;
            }

            Console.Write("New title: ");
            string newTitle = Console.ReadLine() ?? string.Empty;

            Console.Write("New description: ");
            string newDescription = Console.ReadLine() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(newTitle))
            {
                ShowMessage("Title cannot be empty.");
                return;
            }

            bool success = taskService.EditTask(id, loggedUser!.Id, newTitle, newDescription);

            if (success)
            {
                ShowMessage("Task updated.");
            }
            else
            {
                ShowMessage("Task not found.");
            }
        }

        private static void DeleteTask()
        {
            Console.Clear();
            Console.WriteLine("=== Delete Task ===");

            Console.Write("Enter task id: ");
            bool parsed = int.TryParse(Console.ReadLine(), out int id);

            if (!parsed)
            {
                ShowMessage("Invalid id.");
                return;
            }

            bool success = taskService.DeleteTask(id, loggedUser!.Id);

            if (success)
            {
                ShowMessage("Task deleted.");
            }
            else
            {
                ShowMessage("Task not found.");
            }
        }

        private static void ShowMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine();
            Console.WriteLine(message);
            Console.ResetColor();

            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }
        private static void ManageTasksMenu()
        {
            while (true)
            {
                Console.Clear();

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("=== Manage Tasks ===");
                Console.ResetColor();

                Console.WriteLine("1. Show Completed Tasks");
                Console.WriteLine("2. Show Pending Tasks");
                Console.WriteLine("3. Complete Task");
                Console.WriteLine("4. Edit Task");
                Console.WriteLine("5. Delete Task");
                Console.WriteLine("0. Back");

                Console.Write("Choose option: ");
                string? input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        ShowTasks(taskService.GetCompletedTasksByUser(loggedUser!.Id));
                        break;
                    case "2":
                        ShowTasks(taskService.GetPendingTasksByUser(loggedUser!.Id));
                        break;
                    case "3":
                        CompleteTask();
                        break;
                    case "4":
                        EditTask();
                        break;
                    case "5":
                        DeleteTask();
                        break;
                    case "0":
                        return;
                    default:
                        ShowMessage("Invalid option.");
                        break;
                }
            }
        }
    }
}