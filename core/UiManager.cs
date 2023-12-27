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
        public MainUI() { }

        public void HomePage()
        {
            Console.Clear();
            SearchPage searchPage = new SearchPage();
            SettingsPage settingsPage = new SettingsPage();

            int selectedTab = 0;
            string SearchQuery = "";

            while (true)
            {
                if (selectedTab == 0)
                {
                    bool continueSearch = searchPage.SearchTab(ref selectedTab, ref SearchQuery);
                    if (continueSearch)
                    {
                        Search(SearchQuery);
                    }
                }
                else if (selectedTab == 1)
                {
                    LibaryPage libaryPage = new LibaryPage();

                    libaryPage.DonwoladTab(ref selectedTab);
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

            TmdbApi tmdbApi = new TmdbApi();
            List<Movie> movies = tmdbApi.Search(query).Result;
            Movie movie = new Movie();

            int TMDbID = selectPage.Select(movies, ref query);
            if (TMDbID == 0)
            {
                return;
            }
            Console.Clear();
            movie = movies.Where(x => x.Id == TMDbID).FirstOrDefault();


            List<Links> links = providerManager.RunProviders(query, TMDbID).Result;
            PlayMovie(links, movie, TMDbID);
        }
        public async void PlayMovieFromLibary(string MovieName, int TMDbID)
        {
            ProviderManager providerManager = new ProviderManager();
            TmdbApi tmdbApi = new TmdbApi();
            Movie movie =  tmdbApi.Search(MovieName).Result.FirstOrDefault();
            List<Links> links = providerManager.RunProviders(MovieName, TMDbID).Result;
            PlayMovie(links, movie, TMDbID);

        }

        public void PlayMovie(List<Links> links, Movie movie, int TMDbID)
        {
            ConsoleKeyInfo key = new ConsoleKeyInfo();
            MovieActions movieActions = new MovieActions();
            MovieDetails movieDetails = new MovieDetails();
            
            int curentLink = 0;
            int curentProvider = 0;
            for (int i = 0; i < links.Count; i++)
            {
                if (links[i].links.Count == 0)
                {
                    links.RemoveAt(i);
                }

            }
            while (key.Key != ConsoleKey.S)
            {
                Console.Clear();
                movieDetails.ShowMovieDetails(movie);
                movieActions.Select(
                    links,
                    movie.Title,
                    ref key,
                    ref curentProvider,
                    ref curentLink,
                    TMDbID
                );
            }
        }
    }
}
