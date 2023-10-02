using Spectre.Console;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using TuxStream.Core.Obj;
namespace TuxStream.Core.UI
{
    class MovieActions
    {
        int linkIndex = 0;
        int providerIndex = 0;

        public MovieActions()
        {

        }
        public void Select(List<Links> links, ref ConsoleKeyInfo key)
        {
            AnsiConsole.MarkupLine($"[bold]P[/]lay movie , [bold]C[/]hose a link, [bold]N[/]ew link({links[linkIndex].links.Count}/{linkIndex}), [bold]D[/]ownload movie , [bold]S[/]earch again ,[bold]L[/]ist link, [bold]Q[/]uit");

            key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.P) { Play(links[providerIndex].links[linkIndex]); }
            else if (key.Key == ConsoleKey.N) { NewLink(links[providerIndex].links.Count); }
            else if (key.Key == ConsoleKey.D) { Download(links[providerIndex].links[linkIndex].link); }
            else if (key.Key == ConsoleKey.C) { linkIndex = PickUrl(links); }
            else if (key.Key == ConsoleKey.S) { return; }
            else if (key.Key == ConsoleKey.Q) { Environment.Exit(0); }
            else if (key.Key == ConsoleKey.L) { Console.WriteLine(links[linkIndex]); Thread.Sleep(5000); }


        }

        int PickUrl(List<Links> links )
        {
            ConsoleKeyInfo key = new ConsoleKeyInfo();
            int selectedLinkID = 0;
            do
            {

                foreach (var provider in links)
                {
                    var rule = new Rule($"Provider[red]{provider.name}[/]");
                    rule.Style = Style.Parse("red dim");
                    AnsiConsole.Write(rule);
                    for (int i = 0; i < provider.links.Count; i++)
                    {
                        if (i == selectedLinkID)
                        {
                            AnsiConsole.MarkupLine($"[red]==>[/][bold]{i})[/] {provider.links[i].quality}");
                            continue;
                        }

                        AnsiConsole.MarkupLine($"   {i}) {provider.links[i].quality}");
                    }





                }
                key = Console.ReadKey(true);
                Console.Clear();

                if (key.Key == ConsoleKey.DownArrow) { selectedLinkID++; }

            } while (key.Key != ConsoleKey.Enter);
            return selectedLinkID;

        }
        void NewLink(int linkCount)
        {
            if (linkIndex == linkCount - 1)
            {
                linkIndex = 0;
                return;
            }
            linkIndex++;

        }
        void Download(string link)
        {
            Download download = new Download();
            download.DownloadMovie(link);
        }
        void Play(Link link)
        {
            ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName = "mpv",
                Arguments = link.link,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };

            // Create a new process
            Process vlcProcess = new Process() { StartInfo = psi };

            // Start VLC
            vlcProcess.Start();

        }
    }
}