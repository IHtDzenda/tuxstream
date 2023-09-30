using TuxStream.Plugin;
using Newtonsoft.Json;

namespace TuxStream.Plugin
{
    public class TmdbApi
    {
        private string APIKey = "9990db75d12d4ecd4ed84628ebc96403";

        public TmdbApi()
        {
            
        }
        public List<TmdbObj.Movie> Search(string _query)
        {
            string json = getMovieData(_query).Result;

            List<TmdbObj.Movie> movies = new List<TmdbObj.Movie>();

            movies = JsonConvert.DeserializeObject<List<TmdbObj.Movie>>(json);

            return movies;


        }
        public async Task<string> getMovieData(string query)
        {
            string json = "";
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.themoviedb.org/3/");


                string endpoint = $"search/movie?query={query}";
                string api = $"&api_key={APIKey}";

                HttpResponseMessage response = await client.GetAsync(endpoint + api);

                if (response.IsSuccessStatusCode)
                {
                    json = await response.Content.ReadAsStringAsync();
                    int startIndex = json.IndexOf("[");
                    int endIndex = json.LastIndexOf("]") + 1;
                    string jsonArray = json.Substring(startIndex, endIndex - startIndex);
                    return jsonArray;
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                }
            }
            return json;
        }



    }

}