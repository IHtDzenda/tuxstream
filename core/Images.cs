using TuxStream.Core;
using TuxStream.Core.Obj;
using static TuxStream.Plugin.TmdbObj;
using System.Net;
namespace TuxStream.Core
{
    public class Images
    {
        Setting settings = new Setting();
        public string CachePath = "";

        public Images()
        {
            CachePath = settings.GetCachePath();
        }

        public string GetPosterPath(Movie movie)
        {
            string posterPath="";
            posterPath = GetPosterPathLocal(movie.Id);
            if (posterPath=="")
            {
                posterPath = GetPosterPathOnline(movie.Id,movie.PosterPath);
            }
            return posterPath;
        }

        public string GetPosterPathLocal(int TMDbID)
        {
            string Path = CachePath + TMDbID + ".jpg";
            if (System.IO.File.Exists(Path))
            {
                return Path;
            }

            return "";
        }

        private string GetPosterPathOnline(int TMDbID,string posterPath)
        {
            string url = "https://image.tmdb.org/t/p/w500" + posterPath ;
            string imagePath = CachePath + TMDbID + ".jpg";
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(new Uri(url),imagePath);
            }
            return imagePath;
        }
    }
}
