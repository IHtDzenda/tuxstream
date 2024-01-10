using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Spectre.Console;
using Spectre.Console.Rendering;
using TuxStream;
using TuxStream.Core;
using TuxStream.Core.Obj;
using TuxStream.Plugin;
using TuxStream.Provider;

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
            bool queryOnly = _tmdbid == 0 ? true : false;
            List<IProvider> plugins = LoadProviders();
            List<Links> links = new List<Links>();
            if (queryOnly)
            {
                plugins = plugins.Where(s => s.Config.Type == ProviderType.Query).ToList();
            }


            await AnsiConsole
                .Status()
                .StartAsync(
                    "Loading Providers...",
                    async ctx =>
                    {
                        for (int i = 0; i < plugins.Count; i++)
                        {

                            string[] providerLang = plugins[i].Config.Languages;
                            if (langues.Any(s => providerLang.Contains(s)))
                            {
                                ctx.Status($"Loaded {i} source out of {plugins.Count}\n working on {plugins[i].Config.ProviderName}");
                                links.Add(await plugins[i].Main(_query, _tmdbid));
                            }
                        }
                    }
                );

            return links;
        }
    }
}
