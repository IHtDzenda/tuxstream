using Newtonsoft.Json;

namespace TuxStream.Plugin
{
    public class TmdbObj
    {
        public class Movie
        {
            public bool Adult { get; set; }
            public string BackdropPath { get; set; }
            public List<int> GenreIds { get; set; }

            [JsonProperty("id")]
            [JsonConverter(typeof(IntConverter))]
            public int Id { get; set; }
            public string OriginalLanguage { get; set; }
            public string OriginalTitle { get; set; }
            public string Overview { get; set; }

            [JsonProperty("popularity")]
            [JsonConverter(typeof(DoubleConverter))]
            public double Popularity { get; set; }
            public string PosterPath { get; set; }
            [JsonProperty("release_date")]
            public string? ReleaseDate { get; set; }
            public string Title { get; set; }
            public bool Video { get; set; }

            [JsonProperty("vote_average")]
            [JsonConverter(typeof(DoubleConverter))]
            public double VoteAverage { get; set; }
            public int VoteCount { get; set; }
        }


        public class Movies
        {
            public int Page { get; set; }
            public List<Movie> Results { get; set; }
            public int TotalPages { get; set; }
            public int TotalResults { get; set; }
        }

    }

}