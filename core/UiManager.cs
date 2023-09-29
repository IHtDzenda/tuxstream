using Spectre.Console;
using TuxStream.Core.UI.Components;
using TuxStream.Core.UI;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TuxStream.Plugin;

namespace TuxStream.Core.UI
{
    class MainUI
    {
        public MainUI()
        {
        }
        public void HomePage()
        {
            Console.Clear();
            SearchPage searchPage = new SearchPage();
            SettingsPage settingsPage = new SettingsPage();
            DonwoladPage donwoladPage = new DonwoladPage();

            int selectedTab = 0;
            string SearchQuery = "";

            while (true)
            {
                if (selectedTab == 0)
                {
                    if (searchPage.SearchTab(ref selectedTab, ref SearchQuery))
                    {
                        Search(SearchQuery);
                    }
                }
                else if (selectedTab == 1)
                {
                    donwoladPage.DonwoladTab(ref selectedTab);
                    continue;
                }
                else if (selectedTab == 2)
                {
                    settingsPage.SettingsTab(ref selectedTab);
                    continue;

                }

            }
        }
        public void Search(string query)
        {
            SelectPage selectPage = new SelectPage();
            ProviderManager providerManager = new ProviderManager();
            TMDbClient client = new TMDbClient("9990db75d12d4ecd4ed84628ebc96403");//not mine :D

            SearchContainer<SearchMovie> movies = client.SearchMovieAsync(query).Result;
            int TMDbID = selectPage.Select(movies);
            if (TMDbID == 0) { return; }

            List<string> links = providerManager.RunProviders(query, TMDbID);
            PlayMovie(links, TMDbID, movies);

        }
        public void PlayMovie(List<string> links, int TMDbID, SearchContainer<SearchMovie> movies)
        {
            Console.Clear();
            MovieDetails movieDetails = new MovieDetails(TMDbID, movies);
            MovieActions movieActions = new MovieActions();
            movieActions.Select(links);
        
        }
    }
}
