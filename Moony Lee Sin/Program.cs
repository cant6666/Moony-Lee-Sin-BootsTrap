﻿using System;
using System.IO;
using System.Net;
using System.Reflection;
using EloBuddy;
using EloBuddy.SDK.Events;

namespace Moony_Lee_Sin
{
    class Program
    {
        static string GetOnlineVersion()
        {
            string version = string.Empty;
            WebRequest req = WebRequest.Create("https://raw.githubusercontent.com/DanThePman/MoonyLeeSinData/master/version.txt");
            req.Method = "GET";

            // ReSharper disable once AssignNullToNotNullAttribute
            using (StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream()))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    version = line;
                    break;
                }
            }
            return version;
        }

        private static string dllPath => GetDllPath;
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += eventArgs =>
            {
                bool JustUpdated = false;

                if (!File.Exists(dllPath))
                {
                    Download();
                    JustUpdated = true;
                }

                
                if (File.Exists(updateReminderPath))
                {
                    File.Delete(updateReminderPath);
                    Update();
                    JustUpdated = true;
                }

                Assembly SampleAssembly = Assembly.LoadFrom(dllPath);
                Type myType = SampleAssembly.GetType("l");

                if (!JustUpdated)
                {
                    var localVersion = (string)myType.GetMethod("a", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, null);
                    string onlineVersion = GetOnlineVersion();
                    if (localVersion != onlineVersion && !File.Exists(updateReminderPath))
                    {
                        CreateUpdateReminder();
                        return;
                    }
                }

                var main = myType.GetMethod("b", BindingFlags.NonPublic | BindingFlags.Static);
                main.Invoke(null, null);
            };
        }

        static string GetDllPath
        {
            get
            {
                string s = @":\Users\" + Environment.UserName + @"\AppData\Roaming\EloBuddy";
                string[] hardDiskLetters = new[]
                {
                    "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U",
                    "V", "W", "X", "Y", "Z"
                };

                foreach (var letter in hardDiskLetters)
                {
                    string path = letter + s;
                    if (Directory.Exists(path))
                        return path + @"\Addons\Libraries\MLS.dll";
                }

                Chat.Print("<b><font size='20' color='#FF0000'>[Moony Lee Sin] Elobuddy Path Not Found! Contact DanThePman</font></b>");
                return string.Empty;
            }
        }

        static string GetElobuddyPath
        {
            get
            {
                string s = @":\Users\" + Environment.UserName + @"\AppData\Roaming\EloBuddy\";
                string[] hardDiskLetters = new[]
                {
                    "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U",
                    "V", "W", "X", "Y", "Z"
                };

                foreach (var letter in hardDiskLetters)
                {
                    string path = letter + s;
                    if (Directory.Exists(path))
                        return path;
                }

                Chat.Print("<b><font size='20' color='#FF0000'>[Moony Lee Sin] Elobuddy Path 2 Not Found! Contact DanThePman</font></b>");
                return string.Empty;
            }
        }

        private static void Download()
        {
            Chat.Print("<b><font size='20' color='#008B8B'>[Moony Lee Sin] Downloading Core...</font></b>");
            using (WebClient w = new WebClient())
            {
                w.DownloadFile("https://github.com/DanThePman/MoonyLeeSinData/raw/master/MLS.dll", dllPath);
            }
            Chat.Print("<b><font size='20' color='#008B8B'>[Moony Lee Sin] Download Completed!</font></b>");
        }

        static string updateReminderPath = GetElobuddyPath + "MLS_UPDATE_PENDING.txt";
        private static void CreateUpdateReminder()
        {
            new StreamWriter(updateReminderPath).Close();
            Chat.Print("<b><font size='20' color='#FF0000'>[Moony Lee Sin] Reload This Addon To Update! (F5)</font></b>");
        }

        private static void Update()
        {
            Chat.Print("<b><font size='20' color='#008B8B'>[Moony Lee Sin] Downloading Update...</font></b>");
            using (WebClient w = new WebClient())
            {
                w.DownloadFile("https://github.com/DanThePman/MoonyLeeSinData/raw/master/MLS.dll", dllPath);
            }
            Chat.Print("<b><font size='20' color='#008B8B'>[Moony Lee Sin] Update Completed!</font></b>");
        }
    }
}
