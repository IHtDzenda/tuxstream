using Spectre.Console;
using System.Diagnostics;

namespace TuxStream.Core.UI
{
    class MovieActions
    {
        int linkIndex = 0;
        public MovieActions()
        {

        }
        public void Select(List<string> links)
        {
            AnsiConsole.MarkupLine($"[bold]P[/]lay movie ,[bold]N[/]ew link({links.Count}/{linkIndex}), [bold]D[/]ownload movie , [bold]S[/]earch again ,[bold]L[/]ist link, [bold]Q[/]uit");
            ConsoleKeyInfo key = new ConsoleKeyInfo();

            while (true)
            {
                key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.P) { Play(links[linkIndex].ToString()); }
                else if (key.Key == ConsoleKey.N) { NewLink(links.Count); }
                else if (key.Key == ConsoleKey.D) { Download(links[linkIndex]); }
                else if (key.Key == ConsoleKey.S) { return; }
                else if (key.Key == ConsoleKey.Q) { Environment.Exit(0); }
                else if (key.Key == ConsoleKey.L) { Console.WriteLine(links[linkIndex]);}

            }
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
        void Play(string link)
        {
            ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName = "mpv",
                Arguments = link,
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