using System.Diagnostics;
using Microsoft.Win32;

namespace SteamSwitcher
{
    public static class Steam
    {
        public static void Kill()
        {
            foreach (var process in Process.GetProcessesByName("steam"))
            {
                process.Kill();
            }
        }
        
        public static void RegistryEdit(string username)
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

        public static void Start()
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