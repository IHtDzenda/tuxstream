using Spectre.Console;
using TuxStream.Core.UI.Components;
namespace TuxStream.Core.UI
{
    public class DonwoladPage
    {
        public DonwoladPage()
        {

        }
        public bool DonwoladTab(ref int selectedTab)
        {
            Console.Clear();
            ConsoleKeyInfo key = new ConsoleKeyInfo();
            TabsComponent tabs = new TabsComponent();


            while (true)
            {
                tabs.TabsShow(selectedTab);

                AnsiConsole.MarkupLine("[gray bold]Donwolad: [/]");



                key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter) {  }
                else if (key.Key == ConsoleKey.LeftArrow) { selectedTab--; return false; }
                else if (key.Key == ConsoleKey.RightArrow) { selectedTab++; return false; }

                Console.Clear();
            }
        }
    }
}