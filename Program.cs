using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Linq;
using static SteamSwitcher.Models;

namespace SteamSwitcher
{
    class Program
    {
        static void Main()
        {
            var liteDatabaseService = new LiteDatabaseService(new DatabaseSettings("user.db", "users"));

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
                    Console.WriteLine("No users in database, type new for creating any one\n");
                }

                var inputCommand = Console.ReadLine();

                if (!string.IsNullOrEmpty(inputCommand))
                {
                    var inputSplitted = inputCommand.Split();

                    switch (inputSplitted[0])
                    {
                        case "q":
                        {
                            Environment.Exit(0);
                            break;
                        }

                        case "s":
                        {
                            var userId = inputSplitted[1];
                            var result = int.TryParse(userId, out int id);

                            if (result)
                            {
                                Console.WriteLine($"Selecting user with ID: {id}");

                                var user = liteDatabaseService.FindById(id);

                                KillSteam();
                                RegistryEdit(user.Name);
                                StartSteam();
                            }

                            Console.Clear();
                            break;
                        }

                        case "n":
                        {
                            var userName = inputSplitted[1];
                            var hint = inputSplitted[2];

                            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(hint))
                            {
                                Console.WriteLine($"Creating user with username: {userName}");
                                liteDatabaseService.Insert(new SteamUser {Name = userName, Hint = hint});
                            }

                            Console.Clear();
                            break;
                        }

                        case "d":
                        {
                            var userId = inputSplitted[1];
                            var result = int.TryParse(userId, out int id);

                            if (result)
                            {
                                Console.WriteLine($"Removing user with ID: {id}");
                                liteDatabaseService.Delete(id);
                            }

                            Console.Clear();
                            break;
                        }

                        default:
                        {
                            Console.Clear();
                            break;
                        }
                    }
                }
                else
                {
                    Console.Clear();
                }
            }
        }


        static void KillSteam()
        {
            foreach (var process in Process.GetProcessesByName("steam"))
            {
                process.Kill();
            }
        }


        static void RegistryEdit(string username)
        {
            using (var steamSubKey = Registry.CurrentUser.OpenSubKey("Software\\Valve\\Steam", true))
            {
                if (steamSubKey != null)
                {
                    steamSubKey.SetValue("AutoLoginUser", username, RegistryValueKind.String);
                    steamSubKey.SetValue("RememberPassword", 1, RegistryValueKind.DWord);

                    steamSubKey.Close();
                }
            }
        }

        static void StartSteam()
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo("steam://open/main")
                {
                    UseShellExecute = true
                }
            };

            process.Start();
        }
    }
}