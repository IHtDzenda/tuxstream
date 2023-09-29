using Spectre.Console;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
namespace TuxStream.Core.UI
{
    public class MovieDetails
    {
        public MovieDetails(int TMDbID, SearchContainer<SearchMovie> movies)
        {
            foreach (var item in movies.Results)
            {
                if (item.Id == TMDbID)
                {
                    AnsiConsole.MarkupLine($"[bold underline]{item.Title}[/]");
                    AnsiConsole.MarkupLine($"[bold]Year:[/] {item.ReleaseDate.Value.Year} ");
                    AnsiConsole.MarkupLine($"[bold]Original Lang:[/] {item.OriginalLanguage} ");
                    AnsiConsole.MarkupLine($"[gray italic]\"{item.Overview}\"[/]");
                    AnsiConsole.Write(new BreakdownChart()
                        .Width(30)
                        .AddItem("Rating", item.VoteAverage, Color.Yellow)
                        .AddItem("", 10 - item.VoteAverage, Color.Black));
                }
            }



        }
    }
}