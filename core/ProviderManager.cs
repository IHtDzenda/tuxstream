using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TuxStream.Plugin;
using TuxStream.Provider;
using TuxStream;
using System.Globalization;
using TuxStream.Core;
using TuxStream.Core.Obj;

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
        public List<Links> RunProviders(string _query, int _tmdbid)
        {
            List<IProvider> plugins = LoadProviders();
            List<Links> links = new List<Links>();
            foreach (var plugin in plugins)
            {
                links.Add(plugin.Main(_query, _tmdbid));   
            }
            return links;
        }
    }
}
