namespace TuxStream.Core.Obj
{
    public class ConfigObj
    {
        public class Config
        {
            public Subtitles? Subtitles { get; set; }
            public SearchHistory? SearchHistory { get; set; }
            public Downloads? Downloads { get; set; }
        }

        public class Subtitles
        {
            public string? UserName { get; set; }
            public string? Email { get; set; }
        }

        public class SearchHistory
        {
            public string[]? Searches { get; set; }
        }

        public class Downloads
        {
            public string? Path { get; set; }
            public DownloadedMovie[]? DownloadedMovies { get; set; }
        }

        public class DownloadedMovie
        {
            public string? Name { get; set; }
            public string? Path { get; set; }
        }
    }


}