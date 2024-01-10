namespace TuxStream.Core.Obj
{
    public enum ProviderType
    {
        TmdbId,
        Query
    }
    public enum Category
    {
        Movie,
        TvShow,
        Anime
    }
    public class ProviderConfig
    {
        public string ProviderName { get; set; }
        public string[] Languages { get; set; }
        public ProviderType Type { get; set; }
        public Category Category { get; set; }
    }
}
