using Spectre.Console;

namespace TuxStream.Core.UI.Components
{
    public class TabsComponent
    {
        string[] tabs = new string[] { "Search", "Donwolad", "Settings" };

        public string Tabs(int _selectedTab)
        {
            string tabstext = "";
            for (int i = 0; i < tabs.Length; i++)
            {
                if (i == _selectedTab)
                {
                    tabstext += $"[red]{tabs[i]}[/] ──";
                    continue;
                }
                tabstext += $"[gray]{tabs[i]}[/] ──";
            }
            tabstext = tabstext.Substring(0, tabstext.Length - 3);

            return tabstext;

        }
        public void TabsShow(int _selectedTab)
        {
            string Text = Tabs(_selectedTab);
            var rule = new Rule(Text);
            rule.LeftJustified();
            AnsiConsole.Write(rule);
        }
    }
}