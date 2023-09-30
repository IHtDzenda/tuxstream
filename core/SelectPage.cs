using System.ComponentModel;
using Spectre.Console;
using TuxStream.Plugin;
using static TuxStream.Plugin.TmdbObj;

namespace TuxStream.Core.UI
{
    public class SelectPage
    {
        public SelectPage()
        {

        }
        public int Select(List<Movie>  movies)
        {
            Console.Clear();
            List<string> movieList = new List<string>();

            movieList.Add("Search again");
            foreach (Movie movie in movies)
            {
                if (movie.ReleaseDate == null || movie.ReleaseDate == ""|| movie.ReleaseDate.Length <4)
                {
                    movieList.Add(movie.Title);
                    continue;
                }
                movieList.Add(movie.Title + " - " + movie.ReleaseDate?.Substring(0, 4));
            }

            var movieSelector = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Selet the movie by using [green]↓[/]?")
                .PageSize(15)
                .MoreChoicesText("[grey](Move up and down to reveal more movies)[/]")
                .AddChoices(movieList));


            foreach (Movie movie in movies)
            {
                if (movie.Title == movieSelector || movie.Title + " - " + movie.ReleaseDate.Substring(0, 4) == movieSelector)
                {
                    return movie.Id;
                }

            }
            return 0;

        }
    }
}