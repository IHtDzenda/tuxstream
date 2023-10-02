using Spectre.Console;
using TuxStream.Core.UI.Components;
using TuxStream.Core.UI;
using TuxStream.Plugin;
using static TuxStream.Plugin.TmdbObj;
using TuxStream.Core.Obj;

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
        public async Task Search(string query)
        {
            SelectPage selectPage = new SelectPage();
            ProviderManager providerManager = new ProviderManager();

            TmdbApi tmdbApi = new TmdbApi();
            List<Movie> movies = tmdbApi.Search(query).Result;



            int TMDbID = selectPage.Select(movies);
            if (TMDbID == 0) { return; }

            List<Links> links = providerManager.RunProviders(query, TMDbID);
            PlayMovie(links, TMDbID, movies);


        }
        public void PlayMovie(List<Links> links, int TMDbID, List<Movie> movies)
        {
            ConsoleKeyInfo key = new ConsoleKeyInfo();

            while (key.Key != ConsoleKey.S)
            {
                Console.Clear();
                MovieDetails movieDetails = new MovieDetails(TMDbID, movies);
                MovieActions movieActions = new MovieActions();
                movieActions.Select(links, ref key);


            }

        
        }
    }
}
