using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
namespace TuxStream.Plugin
{
    public class TmdbApi
    {
        private string APIKey = "9990db75d12d4ecd4ed84628ebc96403";

        public TmdbApi()
        {

        }
        public List<SearchMovie> Search(string _query)
        {
            TMDbClient client = new TMDbClient(APIKey);
            SearchContainer<SearchMovie> results = client.SearchMovieAsync(_query).Result;

            foreach (SearchMovie result in results.Results)
            {
                Console.WriteLine(result.Title);
                Console.WriteLine(result.Id);
            }
            return results.Results.ToList();
        }

    }
}