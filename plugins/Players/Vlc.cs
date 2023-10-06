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
    public class Vlc
    {
        public Link link;
        public int tmdbId;
        public string MovieName;
        public ConfigObj.WatchHistoryObj activeRecord = new ConfigObj.WatchHistoryObj();

        public Players players;
        public Vlc(Link _link, int _tmdbId = 0, string _MovieName = "a")
        {
            link = _link;
            tmdbId = _tmdbId;
            MovieName = _MovieName;
            players = new Players(link, tmdbId, MovieName);
            activeRecord = players.GetActiveRecord();
        }
        public async Task Play()
        {
            string programPath = GetOsPaths();
            ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName = programPath,
                Arguments = $" --http-password=abc {link.link} --intf qt --extraintf http --http-host 127.0.0.1",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };

            Process playerProcess = new Process() { StartInfo = psi };

            playerProcess.Start();
            Task.Run(async () => await LogHistory());

        }

        private static string GetOsPaths ()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return  @"C:\Program Files\VideoLAN\VLC\vlc.exe";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return  "vlc";
            }
            else
            {
                throw new Exception("OS not supported!");
            }
        }
        public async Task LogHistory()
        {
            while (true)
            {
            {
                Thread.Sleep(15000);
                int watchTime = await GetWatchtime();
                players.WriteHistory(watchTime);
            }
            }
        }
        public async Task<int> GetWatchtime()
        {
            string url = $"http://:abc@127.0.0.1:8080/requests/status.json";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync(url);
                    var responseString = await response.Content.ReadAsStringAsync();
                    var responseObj = JsonSerializer.Deserialize<VlcObj>(responseString);
                    return responseObj.time;
                }
            }
            catch (System.Exception)
            {
                return 0;
            }
        }
        class VlcObj
        {
            public int time { get; set; }
        }



    }
}