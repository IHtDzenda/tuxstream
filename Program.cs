using System;
using TuxStream.Provider;
using TuxStream.Plugin;
using TMDbLib.Objects.Search;
using TuxStream.Core.UI;
namespace TuxStream
{
    internal class Program
    {
        static void Main()
        {
            MainUI mainUI = new MainUI();
            mainUI.HomePage();
            /*
            TmdbApi tmdbApi = new TmdbApi();
            
            List<SearchMovie> res = tmdbApi.Search("The Matrix");
            foreach (SearchMovie result in res)
            {
                Console.WriteLine(result.Title);
                Console.WriteLine(result.Id);
            }*/
            /*
            ProviderManager providerManager = new ProviderManager();
            providerManager.RunProviders("test", 0);   */         
        }
    }
}