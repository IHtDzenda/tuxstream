using TuxStream.Core;
using Spectre.Console;
using TuxStream.Core.UI.Components;
using System.Runtime.InteropServices;
using TuxStream.Core.Obj;
using System.ComponentModel;
namespace TuxStream.Core.UI
{
    public class LibaryPage
    {
        public List<ConfigObj.WatchHistoryObj> watchHistory = new List<ConfigObj.WatchHistoryObj>();
        Images images = new Images();
        public int activeMovie = 1;
        public LibaryPage()
        {
            Setting setting = new Setting();
            watchHistory = setting.GetWatchHistory();
            watchHistory.Reverse();
        }
        public bool DonwoladTab(ref int selectedTab)
        {
            Console.Clear();
            ConsoleKeyInfo key = new ConsoleKeyInfo();
            TabsComponent tabs = new TabsComponent();

            if (watchHistory.Count == 1)
            {
                tabs.TabsShow(selectedTab);
                Console.WriteLine("No movies in watch history!");
                key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter) { }
                else if (key.Key == ConsoleKey.LeftArrow) { selectedTab--; return false; }
                else if (key.Key == ConsoleKey.RightArrow) { selectedTab++; return false; }
            }

            while (true)
            {
                PrintCards(activeMovie);
                key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.LeftArrow) { selectedTab--; return false; }
                else if (key.Key == ConsoleKey.RightArrow) { selectedTab++; return false; }
                else if (key.Key == ConsoleKey.UpArrow) { activeMovie = moveMovie(activeMovie, -1, watchHistory.Count); }
                else if (key.Key == ConsoleKey.DownArrow) { activeMovie = moveMovie(activeMovie, 1, watchHistory.Count - 1); }
                else if (key.Key == ConsoleKey.Enter) { Console.Clear(); MainUI mainUI = new MainUI(); mainUI.PlayMovieFromLibary(watchHistory[activeMovie - 1].MovieName, watchHistory[activeMovie - 1].tmdbId); }
                Console.Clear();
            }

        }
        private void PrintCards(int active = 1)
        {
            const int layoutItemsCount = 6;
            int shift = 0;
            if (active + 1 >= layoutItemsCount)
            {
                shift = active - 5;
            }
            Layout layout = GenerateLayout(active - shift);
            layout = SetLayoutContent(active, shift, layoutItemsCount, layout);

            AnsiConsole.Render(layout);
        }

        private Layout SetLayoutContent(int active, int shift, int layoutItemsCount, Layout layout)
        {
            int imageID = 0;

            for (int i = 1; i < layoutItemsCount; i++)
            {
                if (watchHistory.Count - 1 < i)
                {
                    layout[i.ToString()].Update(
                        new Panel(
                            Align.Left(
                                new Markup("\n???")))
                            .Expand());
                    continue;
                }
                if (active - shift == i)
                {
                    layout[(i).ToString() + "-active"].Update(
                        new Panel(
                            Align.Left(
                                new Markup($"{i + shift})\nTitle: " + (watchHistory[i].MovieName != null ? $"{watchHistory[i - 1 + shift].MovieName}" : "?????") + "\n"))).BorderColor(Color.Maroon).Expand());
                    imageID = watchHistory[i - 1 + shift].tmdbId;
                    continue;
                }
                layout[i.ToString()].Update(
                    new Panel(
                        Align.Left(
                            new Markup($"{i + shift})\nTitle: " + (watchHistory[i].MovieName != null ? $"{watchHistory[i - 1 + shift].MovieName}" : "?????") + "\n")))
                        .Expand());

            }

            string imagePath = images.GetPosterPathLocal(imageID);
            layout["Binds"].Update(
                    new Panel(
                        Align.Center(
                            new Markup("Binds: \n" +
                            "Left/Right - Change tab\n" +
                            "Up/Down - Select a Movie\n" +
                            "Enter - Select the Movie"
                            )
                    )));
            layout["Image"].Size( Console.WindowHeight -7);
            if (imagePath == "")
            {
                layout["Image"].Update(
                    new Panel(
                        Align.Center(
                            new Markup("Image not found!")))
                        .Expand());
                return layout;
            }

            layout["Image"].Update(
                new Panel(
                    Align.Center(new CanvasImage(imagePath))
                ).Expand()
            );

            return layout;
        }
        private Layout GenerateLayout(int active)
        {
            return new Layout("Root")
                            .SplitColumns(
                                new Layout("Left")
                                    .SplitRows(
                                        new Layout("1" + (active == 1 ? "-active" : "")),
                                        new Layout("2" + (active == 2 ? "-active" : "")),
                                        new Layout("3" + (active == 3 ? "-active" : "")),
                                        new Layout("4" + (active == 4 ? "-active" : "")),
                                        new Layout("5" + (active == 5 ? "-active" : ""))),
                                new Layout("Info")
                                    .SplitRows(
                                        new Layout("Image"),
                                        new Layout("Binds")));
        }
        private int moveMovie(int activeMovie, int moveValue, int max)
        {
            activeMovie += moveValue;
            if (activeMovie < 1)
            {
                activeMovie = 1;
            }
            if (activeMovie > max)
            {
                activeMovie = max;
            }
            return activeMovie;
        }

    }

}