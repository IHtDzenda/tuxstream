using Spectre.Console;
using TuxStream.Core.UI.Components;
namespace TuxStream.Core.UI
{
    public class SearchPage
    {

        public bool SearchTab(ref int selectedTab ,ref string SearchQuery )
        {
            Console.Clear();
            ConsoleKeyInfo key = new ConsoleKeyInfo();
            TabsComponent tabs = new TabsComponent();

            AnsiConsole.Write(
                new FigletText("TuxStream")
                    .LeftJustified()
                    .Color(Color.Red));

            while (true)
            {
                tabs.TabsShow(selectedTab);
                
                AnsiConsole.MarkupLine("[gray bold]Search: [/]");
                AnsiConsole.MarkupLine($"[red]==>[/]{SearchQuery}");
                AnsiConsole.MarkupLine($"   ");



                key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter) {  Console.Clear(); return true; }
                else if (key.Key == ConsoleKey.LeftArrow) { selectedTab=2; return false; }
                else if (key.Key == ConsoleKey.RightArrow) { selectedTab++; return false; }
                else if (key.Key == ConsoleKey.Backspace&&SearchQuery!="") { SearchQuery = SearchQuery.Substring(0, SearchQuery.Length - 1); }
                else { SearchQuery += key.KeyChar; }
                
                Console.Clear();
            }
        }
    }
}