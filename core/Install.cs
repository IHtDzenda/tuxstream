using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using TuxStream.Core.Obj;
using Newtonsoft.Json;

namespace TuxStream.Core
{
    public class Install
    {
        public static OSPlatform GetOs()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return OSPlatform.OSX;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return OSPlatform.Linux;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return OSPlatform.Windows;
            }
            throw new Exception("Cannot determine operating system!");
        }

        private string DetectLinuxDistribution()
        {
            string releaseFile = "/etc/os-release";
            if (File.Exists(releaseFile))
            {
                string[] lines = File.ReadAllLines(releaseFile);
                foreach (string line in lines)
                {
                    if (line.StartsWith("ID=", StringComparison.OrdinalIgnoreCase))
                    {
                        string[] parts = line.Split('=');
                        if (parts.Length > 1)
                        {
                            return parts[1].Trim('"');
                        }
                    }
                }
            }


            return "debian";
        }

        private void LinuxRun(string command, string arg)
        {

            ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName = command,
                Arguments = arg,
                UseShellExecute = true,
                CreateNoWindow = true
            };
            Process aptProcess = new Process() { StartInfo = psi };
            aptProcess.Start();

            aptProcess.WaitForExit();
            aptProcess.Close();
            Console.WriteLine("Installed ffmpeg");
        }
        public void InstallDependencies()
        {
            OSPlatform os = GetOs();
            if (os == OSPlatform.Windows)
            {

            }
            else if (os == OSPlatform.Linux)
            {
                string distribution = DetectLinuxDistribution();
                
                if (distribution == "debian") { LinuxRun("sudo", "apt-get install -y ffmpeg"); }
                else if (distribution == "arch")
                { LinuxRun("sudo", "pacman -S --noconfirm  ffmpeg"); }
                else { Console.WriteLine("Your distribution is not supported yet for auto install pleace install ffmpeg manually"); }

            }
            else if (os == OSPlatform.OSX)
            {
                Console.WriteLine("OSX");
            }


        }
        private bool IsInstalled()
        {
            OSPlatform os = GetOs();
            if (os == OSPlatform.Windows)
            {
                string configpath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\tuxstream";
                if (Directory.Exists(configpath))
                {
                    return true;
                }
                return false;

            }
            else if (os == OSPlatform.Linux)
            {
                string homepath = Environment.GetEnvironmentVariable("HOME");
                string configpath = homepath + "/.config/tuxstream";
                if (Directory.Exists(configpath))
                {
                    return true;
                }
                return false;

            }
            else if (os == OSPlatform.OSX)
            {
                throw new Exception("OSX is not supported yet");
            }
            return false;
        }

        public Install()
        {
            if (!IsInstalled())
            {
                MakeConfig();
                InstallDependencies();
            }
            else
            {
                Console.WriteLine("TuxStream is already installed");
                Thread.Sleep(100);
            }
        }
        public void MakeConfig()
        {
            OSPlatform os = GetOs();
            if (os == OSPlatform.Windows)
            {
                string homepath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string configpath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\tuxstream";
                MakeConfigfiles(configpath + @"\config.json", homepath);
            }
            else if (os == OSPlatform.Linux)
            {
                string homepath = Environment.GetEnvironmentVariable("HOME");
                string configpath = homepath + "/.config/tuxstream";
                MakeConfigfiles(configpath, "/config.json", homepath);
            }
            else if (os == OSPlatform.OSX)
            {
                throw new Exception("OSX is not supported yet");
            }
        }
        private void MakeConfigfiles(string configpath = "", string file = "config.json", string homepath = "")
        {

            if (!Directory.Exists(configpath))
            {
                try
                {
                    Directory.CreateDirectory(configpath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating folder: {ex.Message}");
                }
            }

            var configData = new ConfigObj.Config
            {
                Subtitles = new ConfigObj.Subtitles
                {
                    UserName = "JohnDoe",
                    Email = "johndoe@example.com"
                },
                SearchHistory = new ConfigObj.SearchHistory
                {
                    Searches = new string[] { }
                },
                Downloads = new ConfigObj.Downloads
                {
                    Path = $@"{homepath}/Downloads/",
                    DownloadedMovies = new ConfigObj.DownloadedMovie[] { }
                }
            };


            string confiDataJson = JsonConvert.SerializeObject(configData, Formatting.Indented);
            if (!File.Exists(configpath + file))
            {
                try
                {
                    File.WriteAllText(configpath + file, confiDataJson);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating config.json: {ex.Message}");
                }
            }
            else
            {
            }


        }

    }
}