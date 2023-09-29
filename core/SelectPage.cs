using System.ComponentModel;
using Spectre.Console;
using TMDbLib.Client;
using TMDbLib.Objects.Discover;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

namespace TuxStream.Core.UI
{
    public class SelectPage
    {
        public SelectPage()
        {

        }
        public int Select(SearchContainer<SearchMovie> movies)
        {
            Console.Clear();
            List<string> movieList = new List<string>();

            movieList.Add("Search again");
            foreach (SearchMovie movie in movies.Results)
            {
                if (movie.ReleaseDate == null)
                {
                    movieList.Add(movie.Title);
                    continue;
                }
                movieList.Add(movie.Title + "-" + movie.ReleaseDate.Value.Year.ToString());
            }

            var movieSelector = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Selet the movie by using [green]↓[/]?")
                .PageSize(15)
                .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
                .AddChoices(movieList));


            foreach (SearchMovie movie in movies.Results)
            {
                if (movie.Title == movieSelector || movie.Title + "-" + movie.ReleaseDate.Value.Year.ToString() == movieSelector)
                {
                    Console.WriteLine("found!" + movie.Id);
                    Thread.Sleep(50);

                    return movie.Id;
                }

            }
            return 0;

        }
    }
}