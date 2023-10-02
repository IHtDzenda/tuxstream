namespace TuxStream.Core.Obj
{
    public class Links
    {
        public string? name { get; set; }
        public List<Link>? links { get; set; }
        public List<Subtitles>? subtitles { get; set; }
    }
    public class Link
    {
        public string? link { get; set; }
        public string? quality { get; set; }
        public string? language { get; set; }
    }
    public class Subtitles
    {
        public string? link { get; set; }
        public string? language { get; set; }

    }
}