using System.ComponentModel;
using Spectre.Console;
using TuxStream.Core;
using TuxStream.Plugin;
using static TuxStream.Plugin.TmdbObj;

namespace TuxStream.Core.UI
{
    public class SelectPage
    {
        public SelectPage() { }

        public int Select(List<Movie> movies, ref string SelectedQuery)
        {
            int activeMovie = 0;
            int activeSubIndex = 0;
            Images images = new Images();
            ConsoleKeyInfo key = new ConsoleKeyInfo();
            if (movies.Count == 0)
            {
                AnsiConsole.MarkupLine("[bold yellow]Waring[/] no tmdbId found defaulting to search query");
                Thread.Sleep(1000);
                Console.Clear();
                return 0;
            }       
            while (true)
            {
                Console.Clear();
                string imagePath = images.GetPosterPath(movies[activeMovie]);
                string MoviesSelectorContent = ShowMovies(movies, activeMovie, activeSubIndex);

                var layout = new Layout("Root").SplitColumns(
                    new Layout("Info").SplitRows(
                        new Layout("MoviesSelector"),
                        new Layout("Description"),
                        new Layout("Binds")
                    ),
                    new Layout("Image")
                );
                layout["MoviesSelector"].Update(
                    new Panel(new Markup(MoviesSelectorContent)).Expand()
                );
                layout["MoviesSelector"].Size(25);

                layout["Description"].Update(
                    new Panel(
                        Align.Center(new Markup($"Description: \n{movies[activeMovie].Overview}"))
                    )
                );
                string bindMessage =
                    activeSubIndex == 0
                        ? ""
                        : "\nPress [bold red]backspace[/] if you want to see all pages";
                layout["Binds"].Update(
                    new Panel(
                        new Markup(
                            $"Select the movie by using [bold red]↓ ↑[/] and [bold red]Enter[/]\nOr you can select a movie by inputing the [bold red]page number[/]{bindMessage}"
                        )
                    )
                );
                layout["Image"].Size(60);

                if (imagePath != null)
                {
                    layout["Image"].Update(
                        new Panel(Align.Center(new CanvasImage(imagePath))).Expand()
                    );
                }
                else
                {
                    layout["Image"].Update(
                        new Panel(Align.Center(new Markup("[bold red]No image found[/]"))).Expand()
                    );
                }

                AnsiConsole.Write(layout);

                key = Console.ReadKey(true);
                char numberKey = key.KeyChar;
                if (char.IsDigit(numberKey))
                {
                    int inputedNumber = int.Parse(numberKey.ToString());

                    if (activeSubIndex != 0)
                    {
                        return movies[(activeSubIndex - 1) * 10 + inputedNumber].Id;
                    }
                    if (inputedNumber * 10 <= movies.Count && inputedNumber != 0)
                    {
                        activeSubIndex = inputedNumber;
                        activeMovie = (inputedNumber - 1) * 10;
                    }
                }
                else if (key.Key == ConsoleKey.Backspace)
                {
                    activeSubIndex = 0;
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    moveCursor(ref activeMovie, movies.Count - 1, +1);
                }
                else if (key.Key == ConsoleKey.UpArrow)
                {
                    moveCursor(ref activeMovie, movies.Count - 1, -1);
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    return movies[activeMovie].Id;
                }
                else if (key.Key == ConsoleKey.S || key.Key == ConsoleKey.Escape)
                {
                    return 0;
                }
            }
        }

        private void moveCursor(ref int activeMovie, int maxMovies, int moveBy)
        {
            if (activeMovie + moveBy < 0)
            {
                activeMovie = maxMovies;
            }
            else if (activeMovie + moveBy >= maxMovies)
            {
                activeMovie = 0;
            }
            else
            {
                activeMovie += moveBy;
            }
        }

        private string ShowMovies(List<Movie> movies, int activeMovie, int activeSubIndex = 0)
        {
            string content = "";

            int subIndex = 0;
            if (activeSubIndex == 0)
            {
                content += ListMovies(movies, activeMovie, 0, movies.Count);
            }
            else
            {
                content += ListMovies(
                    movies,
                    activeMovie,
                    activeSubIndex * 10 - 10,
                    activeSubIndex * 10
                );
            }
            return content;
        }

        private string ListMovies(List<Movie> movies, int activeMovie, int startAt, int EndAt)
        {
            string content = "";

            int subIndex = 0;
            for (int i = startAt; i < EndAt; i++)
            {
                if (
                    movies[i].ReleaseDate == null
                    || movies[i].ReleaseDate == ""
                    || movies[i].ReleaseDate.Length < 4
                )
                {
                    movies[i].ReleaseDate = "????";
                }
                if (i % 10 == 0)
                {
                    subIndex = movies.Count % 10 == 0 ? i / 10 + 1 : i / 10 + 2;
                    content += ($"[red bold]Page {subIndex}[/]\n");
                }
                if (i == activeMovie)
                {
                    content += (
                        $"[red]==>{i % 10}) {movies[i].Title} - {movies[i].ReleaseDate.Substring(0, 4)}[/]\n"
                    );
                }
                else
                {
                    content += (
                        $"   {i % 10}) {movies[i].Title} - {movies[i].ReleaseDate.Substring(0, 4)}\n"
                    );
                }
            }
            return content;
        }
    }
}
