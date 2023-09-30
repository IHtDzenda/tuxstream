using Spectre.Console;
using TuxStream.Plugin;
using static TuxStream.Plugin.TmdbObj;
namespace TuxStream.Core.UI
{
    public class MovieDetails
    {
        public MovieDetails(int TMDbID, List<Movie> movies)
        {
            foreach (Movie movie in movies)
            {
                if (movie.Id == TMDbID)
                {
                    AnsiConsole.MarkupLine($"[bold underline]{movie.Title}[/]");
                    AnsiConsole.MarkupLine($"[bold]Year:[/] {movie.ReleaseDate} ");
                    AnsiConsole.MarkupLine($"[bold]Original Lang:[/] {movie.OriginalLanguage} ");
                    AnsiConsole.MarkupLine($"[gray italic]\"{movie.Overview}\"[/]");
                    AnsiConsole.Write(new BreakdownChart()
                        .Width(30)
                        .AddItem("Rating",movie.VoteAverage, Color.Yellow)
                        .AddItem("", 10 - movie.VoteAverage, Color.Black));
                }
            }



        }
    }
}