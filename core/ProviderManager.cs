using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TuxStream.Plugin;
using TuxStream.Provider;
using TuxStream;
using System.Globalization;

namespace TuxStream
{
    public class ProviderManager
    {
        public List<IProvider> LoadProviders()
        {
            List<IProvider> plugins = new List<IProvider>();


            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes()
                .Where(t => t.Namespace ==  "TuxStream.Provider" && t.GetInterfaces().Contains(typeof(IProvider)));

            foreach (var type in types)
            {
                var plugin = Activator.CreateInstance(type) as IProvider;
                if (plugin != null)
                {
                    plugins.Add(plugin);
                }
            }

            return plugins;
        }
        public List<string> RunProviders(string _query, int _tmdbid)
        {
            List<IProvider> plugins = LoadProviders();
            List<string> Links = new List<string>();
            foreach (var plugin in plugins)
            {
                List<string> strings = plugin.Main(_query , _tmdbid);
                foreach (var str in strings)
                {
                    Links.Add(str);
                }
                
            }
            return Links;
        }
    }
}
