using System.Runtime.InteropServices;
using TuxStream.Core.Obj;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace TuxStream.Core
{
    public class Setting
    {
        public  ConfigObj.Config Config = new ConfigObj.Config();
        private string Path;
        public Setting()
        {
            Path = GetConfigPath();
            if (System.IO.File.Exists(Path))
            {
                Config = JsonSerializer.Deserialize<ConfigObj.Config>(System.IO.File.ReadAllText(Path));
            }
            else
            {
                throw new Exception("Config file not found!");
            }
        }
        private void SaveConfig()
        {
            System.IO.File.WriteAllText(Path, JsonSerializer.Serialize(Config));
        }
        private string GetConfigPath()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\tuxstream\config.json";
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return "/home/" + Environment.UserName + "/.config/tuxstream/config.json";
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                throw new Exception("MacOS is not supported! D:");
            }
            return "";
        }
        public ConfigObj.SearchHistory GetSearchHistory()
        {
            ConfigObj.SearchHistory searchHistory = new ConfigObj.SearchHistory();
            searchHistory = Config.SearchHistory;
            return searchHistory;
        }
        public List<ConfigObj.WatchHistoryObj> GetWatchHistory()
        {
            List<ConfigObj.WatchHistoryObj> watchHistory = new List<ConfigObj.WatchHistoryObj>();

            watchHistory  = Config.WatchHistory.History;
            return watchHistory;
        }
        public void AddWatchHistory(List<ConfigObj.WatchHistoryObj> watchHistoryObj)
        {
            Config.WatchHistory.History = watchHistoryObj;
            SaveConfig();
        }
        public string GetPlayer()
        {
            return Config.Player;
        }
        public string[] GetLanguages()
        {
            return Config.PreferedLanguages;
        }
        public string GetCachePath()
        {
            return Config.CachePath;
        }


    }
}