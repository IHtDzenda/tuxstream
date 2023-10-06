using Spectre.Console;
using TuxStream.Plugin;
using static TuxStream.Plugin.TmdbObj;
namespace TuxStream.Core.UI
{
    public class MovieDetails
    {
        public Movie activeMovie;
        public MovieDetails(int TMDbID, List<Movie> movies)
        {
            foreach (Movie movie in movies)
            {
                if (movie.Id == TMDbID)
                {
                    activeMovie = movie;
                }
            }



        }
        public Movie GetActiveMovie()
        {
            return activeMovie;
        }
        public void ShowMovieDetails()
        {
            AnsiConsole.MarkupLine($"[bold underline]{activeMovie.Title}[/]");
            AnsiConsole.MarkupLine($"[bold]Year:[/] {activeMovie.ReleaseDate} ");
            AnsiConsole.MarkupLine($"[bold]Original Lang:[/] {activeMovie.OriginalLanguage} ");
            AnsiConsole.MarkupLine($"[gray italic]\"{activeMovie.Overview}\"[/]");
            AnsiConsole.Write(new BreakdownChart()
                .Width(30)
                .AddItem("Rating", activeMovie.VoteAverage, Color.Yellow)
                .AddItem("", 10 - activeMovie.VoteAverage, Color.Black));
        }
    }
}