using System;
using System.Linq;
using SteamSwitcher.Database;
using SteamSwitcher.Models;

namespace SteamSwitcher
{
    class Program
    {
        static void Main()
        {
            var liteDatabaseService = new LiteDatabaseService(new LiteDatabaseSettings("user.db", "users"));

            for (;;)
            {
                Console.WriteLine("Steam Account Switcher");
                Console.WriteLine("Available commands: quit, select, new, delete");
                Console.WriteLine("Usage: q - quit programm, s - select ID, n - create USERNAME HINT, d - remove ID");
                Console.WriteLine("Examples: s 1, n TheBottle main, d 1\n");

                var savedUsers = liteDatabaseService.FindAll();
                if (savedUsers != null && savedUsers.Any())
                {
                    Console.WriteLine("Available users in database:");

                    foreach (var user in savedUsers)
                    {
                        Console.WriteLine($"{user.Id}) {user.Name} ({user.Hint})");
                    }

                    Console.Write("\n");
                }
                else
                {
                    Console.WriteLine("No users in database, type n for creating any one\n");
                }

                var inputCommand = Console.ReadLine();

                if (!string.IsNullOrEmpty(inputCommand))
                {
                    var inputSplitted = inputCommand.Split();

                    ProcessCommand(liteDatabaseService, inputSplitted);
                }
                
                Console.Clear();
            }
        }

        static void ProcessCommand(IDatabaseService databaseService, string[] input)
        {
            switch (input[0])
            {
                case "q":
                {
                    Environment.Exit(0);
                    break;
                }

                case "s":
                {
                    string userId = input.Length > 1 ? input[1] : string.Empty;

                    if (!string.IsNullOrEmpty(userId) && int.TryParse(userId, out int id))
                    {
                        Console.WriteLine($"Selecting user with ID: {id}");

                        var user = databaseService.FindById(id);

                        Steam.Kill();
                        Steam.RegistryEdit(user.Name);
                        Steam.Start();
                    }

                    break;
                }

                case "n":
                {
                    var userName = input.Length > 1 ? input[1] : string.Empty;
                    var hint = input.Length > 2 ? input[2] : string.Empty;

                    if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(hint))
                    {
                        Console.WriteLine($"Creating user with username: {userName}");
                        databaseService.Insert(new SteamUser {Name = userName, Hint = hint});
                    }

                    break;
                }

                case "d":
                {
                    string userId = input.Length > 1 ? input[1] : string.Empty;
                    
                    if (!string.IsNullOrEmpty(userId) && int.TryParse(userId, out int id))
                    {
                        Console.WriteLine($"Removing user with ID: {id}");
                        databaseService.Delete(id);
                    }

                    break;
                }
            }
        }
    }
}