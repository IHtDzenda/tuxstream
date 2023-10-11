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
using Spectre.Console;

namespace TuxStream
{
    public class ProviderManager
    {
        public List<IProvider> LoadProviders()
        {
            List<IProvider> plugins = new List<IProvider>();

            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly
                .GetTypes()
                .Where(
                    t =>
                        t.Namespace == "TuxStream.Provider"
                        && t.GetInterfaces().Contains(typeof(IProvider))
                );

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

        public async Task<List<Links>> RunProviders(string _query, int _tmdbid)
        {
            Setting setting = new Setting();
            string[] langues = setting.GetLanguages();

            List<IProvider> plugins = LoadProviders();
            List<Links> links = new List<Links>();
            await AnsiConsole
                .Status()
                .StartAsync(
                    "Loading Providers...",
                    async ctx =>
                    {
                        for (int i = 0; i < plugins.Count; i++)
                        {
                            string[] providerLang = plugins[i].Languages();
                            if (langues.Any(s => providerLang.Contains(s)))
                            {
                                ctx.Status($"Loaded {i} source out of {plugins.Count}\n");
                                links.Add(await plugins[i].Main(_query, _tmdbid));
                            }
                        }
                    }
                );

            return links;
        }
    }
}
