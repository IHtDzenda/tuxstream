using Spectre.Console;
using System.Diagnostics;
using TuxStream.Core.Obj;
using TuxStream.Plugin;
namespace TuxStream.Core.UI
{
    class MovieActions
    {

        public MovieActions()
        {

        }
        public void Select(List<Links> links, string movieName, ref ConsoleKeyInfo key, ref int providerIndex, ref int activeLink, int TMDbID)
        {
            if (links.Count == 0 || links[activeLink].links.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No links found[/]");
                Thread.Sleep(5000);
                return;
            }


            AnsiConsole.MarkupLine($"Streaming from: {links[providerIndex].name} Quality {links[providerIndex].links[activeLink].quality} ");
            AnsiConsole.MarkupLine($"[bold underline]P[/]lay movie , [bold]C[/]hose a link, [bold]D[/]ownload movie , [bold]S[/]earch again ,[bold]L[/]ist link, [bold]Q[/]uit");
            key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.P) { Play(links[providerIndex].links[activeLink], TMDbID, movieName); }
            else if (key.Key == ConsoleKey.D) { Download(links[providerIndex].links[activeLink].link); }
            else if (key.Key == ConsoleKey.C) { activeLink = PickQuality(links); }
            else if (key.Key == ConsoleKey.S) { return; }
            else if (key.Key == ConsoleKey.Q) { Environment.Exit(0); }
            else if (key.Key == ConsoleKey.L) { Console.WriteLine(links[providerIndex].links[activeLink].link); Thread.Sleep(5000); }


        }

        private int PickQuality(List<Links> links, int providerIndex = 0)
        {
            ConsoleKeyInfo key = new ConsoleKeyInfo();
            int selectedLinkID = 0;
            do
            {
                var rule = new Rule($"Provider : [red]{links[providerIndex].name}[/]");
                rule.Style = Style.Parse("red dim");
                AnsiConsole.Write(rule);
                for (int i = 0; i < links[providerIndex].links.Count; i++)
                {
                    if (i == selectedLinkID)
                    {
                        AnsiConsole.MarkupLine($"[red]==>[/][bold]{i})[/] {links[providerIndex].links[i].quality}");
                        continue;
                    }

                    AnsiConsole.MarkupLine($"   {i}) {links[providerIndex].links[i].quality}");
                }

                key = Console.ReadKey(true);
                Console.Clear();

                if (key.Key == ConsoleKey.DownArrow) { MoveLink(ref selectedLinkID, 4, +1); }
                if (key.Key == ConsoleKey.UpArrow) { MoveLink(ref selectedLinkID, 4, -1); }


            } while (key.Key != ConsoleKey.Enter);
            return selectedLinkID;

        }
        private void MoveLink(ref int number, int max, int move)
        {
            if (number + move > max) { number = 0; }
            else if (number + move < 0) { number = max; }
            else { number += move; }
        }

        private void Download(string link)
        {
            Download download = new Download();
            download.DownloadMovie(link);
        }
        private async Task Play(Link link, int TMDbID, string movieName)
        {
            Setting setting = new Setting();
            string player = setting.GetPlayer();
            if (player == "vlc")
            {
                Vlc vlc = new Vlc(link, TMDbID, movieName);
                vlc.Play();
            }
            if (player == "mpv")
            {
                Mpv mpv = new Mpv(link, TMDbID, movieName);
                mpv.Play();
            }
            else
            {
                throw new Exception("Player not supported!");
            }


        }
    }
}