using TuxStream.Core.Obj;
using TuxStream.Core;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.IO.Pipes;
using System.Drawing;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace TuxStream.Plugin
{
    public class Mpv
    {
        private Setting setting = new Setting();
        public Link link;
        public int tmdbId;
        public string MovieName;
        public ConfigObj.WatchHistoryObj activeRecord = new ConfigObj.WatchHistoryObj();
        public Players players;
        public Mpv(Link _link, int _tmdbId = 0, string _MovieName = "a")
        {
            link = _link;
            tmdbId = _tmdbId;
            MovieName = _MovieName;
            players = new Players(link, tmdbId, MovieName);
            activeRecord = players.GetActiveRecord();
        }

        public async Task Play()
        {
            string pipeName = "";
            string programPath = "";
            string subtitles=  $" --sub-auto=all --sub-file-paths={setting.GetCachePath()}/Subtitles/{tmdbId}";
            GetOsPaths(ref pipeName, ref programPath);
            ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName = programPath,
                Arguments = $"{link.link} --input-ipc-server={pipeName} , --start={activeRecord.WatchtimeSec} {subtitles}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };

            Process playerProcess = new Process() { StartInfo = psi };

            playerProcess.Start();
            Task.Run(async () => await LogHistory(pipeName));

        }

        private static void GetOsPaths(ref string pipeName, ref string programPath)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                programPath = "mpv.exe";
                pipeName = @"\pipe\mpvsocket";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                programPath = "mpv";
                pipeName = "/tmp/ipc";
            }
            else
            {
                throw new Exception("OS not supported!");
            }
        }



        public async Task<int> GetWatchtime(string pipeName)
        {
            

            try
            {
                using (NamedPipeClientStream pipeClient = new NamedPipeClientStream(pipeName))
                {
                    pipeClient.Connect();
                    using (StreamWriter writer = new StreamWriter(pipeClient))
                    using (StreamReader reader = new StreamReader(pipeClient))
                    {
                        string command1 = "{\"command\": [\"get_property\", \"playback-time\"]}";
                        writer.WriteLine(command1);
                        writer.Flush();

                        string response1 = reader.ReadLine();
                        var responseJson1 = JsonSerializer.Deserialize<MpvSocktetResponce>(
                            response1
                        );
                        writer.Close();
                        reader.Close();
                        return (int)responseJson1.data;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return 0;
            }
        }

        class MpvSocktetResponce
        {
            public double data { get; set; }
            public int request_id { get; set; }
            public string error { get; set; }
        }

        public async Task LogHistory(string pipeName)
        {

            List<ConfigObj.WatchHistoryObj> viewHistory = setting.GetWatchHistory();

            while (true)
            {
                Thread.Sleep(15000);
                int watchTime = await GetWatchtime(pipeName);
                players.WriteHistory(watchTime);
            }
        }
    }
}
