using System.Net;

using TuxStream.Core.Obj;

namespace TuxStream.Core
{
    public static class SubtitleManager
    {
        private static Setting settings = new Setting();
        public static string path =settings.GetCachePath()+Path.DirectorySeparatorChar+"Subtitles"+Path.DirectorySeparatorChar;

        public static async Task DonwoladSubtitles(List<Subtitles> subtitles,string Provider,int id)
        {
            path = path+Path.DirectorySeparatorChar+id+Path.DirectorySeparatorChar;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            foreach (var item in subtitles)
            {
                string subtitlePath = path + item.language.ToUpper()+'-' +Provider+ ".vtt";
                if (System.IO.File.Exists(subtitlePath))
                {
                    continue;
                }
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(new Uri(item.link), subtitlePath);
                }
            }

        }

    }
}