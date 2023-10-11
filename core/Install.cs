using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using TuxStream.Core.Obj;
using Spectre.Console;

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
            var InstallMessageRule = new Rule("[red]Instalation setup...[/]");
            InstallMessageRule.Justification = Justify.Left;
            AnsiConsole.Write(InstallMessageRule);

            OSPlatform os = GetOs();
            if (os == OSPlatform.Windows)
            {
                AnsiConsole.MarkupLine("Platform detected :[bold red]Windows[/]");
                AnsiConsole.MarkupLine(
                    "To install dependencies run [bold red]winget install ffmpeg vlc[/]"
                );
            }
            else if (os == OSPlatform.Linux)
            {
                string distribution = DetectLinuxDistribution();
                AnsiConsole.MarkupLine($"Platform detected :[bold red]Linux - {distribution}[/]");

                if (distribution == "arch")
                {
                    AnsiConsole.MarkupLine("Install dependencies by running:");
                    AnsiConsole.MarkupLine("sudo pacman -S  ffmpeg mpv");
                }
                else
                {
                    AnsiConsole.MarkupLine("Install this dependencies by running:");
                    AnsiConsole.MarkupLine("[bold red]sudo apt-get install -y ffmpeg mpv[/]");
                    AnsiConsole.MarkupLine("Or by using your package manager");
                }
            }
            else if (os == OSPlatform.OSX)
            {
                Console.WriteLine("OSX");
            }
            var rule = new Rule();
            AnsiConsole.Write(rule);
            AnsiConsole.MarkupLine("Press [red]any[/] key to continue");
            Console.ReadKey();
        }

        private bool IsInstalled()
        {
            OSPlatform os = GetOs();
            if (os == OSPlatform.Windows)
            {
                string configpath =
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
                    + @"\tuxstream";
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
                string configpath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "tuxstream"
                );
                MakeConfigfiles(configpath, @"\config.json", homepath);
                MakeFolder(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
                        + @"\tuxstream\cache"
                ); //not sure if this is the right path
            }
            else if (os == OSPlatform.Linux)
            {
                string? homepath = Environment.GetEnvironmentVariable("HOME");
                string configpath = homepath + "/.config/tuxstream";
                MakeConfigfiles(configpath, "/config.json", homepath);
                MakeFolder(homepath + "/.cache/tuxstream");
            }
            else if (os == OSPlatform.OSX)
            {
                throw new Exception("OSX is not supported yet");
            }
        }

        public void MakeFolder(string configpath = "")
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
        }

        private void MakeConfigfiles(string configpath = "", string file = "", string homepath = "")
        {
            MakeFolder(configpath);
            ConfigObj.Config configData = GenerateConfigObj(homepath);

            string confiDataJson = System.Text.Json.JsonSerializer.Serialize(configData);
            if (!File.Exists(configpath + file))
            {
                try
                {
                    File.WriteAllText(
                        Path.Combine(configpath + file),
                        confiDataJson,
                        Encoding.UTF8
                    );
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating config.json: {ex.Message}");
                }
            }
        }

        private static ConfigObj.Config GenerateConfigObj(string homepath)
        {
            string defaltPlayer = "vlc";
            string caechePath =
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
                + @"\tuxstream\cache\";
            OSPlatform os = GetOs();

            if (os == OSPlatform.Linux)
            {
                defaltPlayer = "mpv";
                caechePath = homepath + "/.cache/tuxstream/";
            }
            return new ConfigObj.Config
            {
                Subtitles = new ConfigObj.Subtitles
                {
                    UserName = "JohnDoe",
                    Email = "johndoe@example.com"
                },
                SearchHistory = new ConfigObj.SearchHistory { Searches = new string[] { } },
                WatchHistory = new ConfigObj.WatchHistory
                {
                    History = new List<ConfigObj.WatchHistoryObj>
                    {
                        new ConfigObj.WatchHistoryObj
                        {
                            MovieName = "",
                            tmdbId = 0,
                            WatchtimeSec = 0
                        }
                    }
                },
                Downloads = new ConfigObj.Downloads
                {
                    Path = $@"{homepath}/Downloads/",
                    DownloadedMovies = new ConfigObj.DownloadedMovie[] { }
                },
                Player = defaltPlayer,
                CachePath = caechePath,
                PreferedLanguages = new string[] { "en" }
            };
        }
    }
}
