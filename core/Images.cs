using System.Net;
using TuxStream.Core;
using TuxStream.Core.Obj;
using static TuxStream.Plugin.TmdbObj;

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
            string posterPath = "";
            posterPath = GetPosterPathLocal(movie.Id);
            if (posterPath == "")
            {
                posterPath = GetPosterPathOnlineAsync(movie.Id, movie.PosterPath).Result;
                if (posterPath == null) { return null;}
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

        private async Task<string> GetPosterPathOnlineAsync(int TMDbID, string posterPath)
        {
            string url = "https://image.tmdb.org/t/p/w500" + posterPath;
            string imagePath = CachePath + TMDbID + ".jpg";
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();
                    File.WriteAllBytes(imagePath, imageBytes);
                }
                else
                {
                    return null;
                }
            }
            return imagePath;
        }
    }
}
