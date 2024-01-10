using System.Net;
using Spectre.Console;
using TuxStream.Core.Obj;
using TuxStream.Plugin;
using static TuxStream.Plugin.TmdbObj;

namespace TuxStream.Core.UI
{
    public class GetSubtitles
    {
        Setting settings = new Setting();
        public string cachePath = "";

        public GetSubtitles()
        {
            cachePath = settings.GetCachePath();
        }
        public void GetSubs(int movieId, List<Subtitles> subtitles)
        {
            if (Directory.Exists(cachePath+movieId) == false)
            {
                Directory.CreateDirectory(cachePath);
            }

            foreach (Subtitles subtitle in subtitles)
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(new Uri(subtitle.link), cachePath + movieId + "_" + subtitle.language + ".srt");
                }
            }

        }

    }
}