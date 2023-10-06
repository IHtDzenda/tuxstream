using TuxStream.Core.Obj;
using TuxStream.Core;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.IO.Pipes;
using System.Drawing;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace TuxStream.Plugin
{
    public class Players
    {
        public Link link;
        public int tmdbId;
        public string MovieName;
        public ConfigObj.WatchHistoryObj activeRecord = new ConfigObj.WatchHistoryObj();
        public Players(Link _link, int _tmdbId = 0, string _MovieName = "a")
        {
            link = _link;
            tmdbId = _tmdbId;
            MovieName = _MovieName;
            activeRecord = GetActiveRecord();
        }
        public ConfigObj.WatchHistoryObj GetActiveRecord()
        {
            Setting setting = new Setting();

            List<ConfigObj.WatchHistoryObj> viewHistory = setting.GetWatchHistory();

            foreach (var item in viewHistory)
            {
                if (item.tmdbId == tmdbId)
                {
                    return item;
                }
                if (item.tmdbId == 0 && item.MovieName == MovieName)
                {
                    return item;
                }

            }
            CreateHistoryRecord();
            return GetActiveRecord();
        }
        public void CreateHistoryRecord()
        {
            Setting setting = new Setting();

            List<ConfigObj.WatchHistoryObj> viewHistory = setting.GetWatchHistory();
            viewHistory.Add(
                new ConfigObj.WatchHistoryObj
                {
                    MovieName = this.MovieName,
                    tmdbId = this.tmdbId,
                    WatchtimeSec = 0
                }
            );
            setting.AddWatchHistory(viewHistory);
        }
        public void WriteHistory( int watchTime)
        {
            Setting setting = new Setting();
            List<ConfigObj.WatchHistoryObj> viewHistory = setting.GetWatchHistory();
            activeRecord.WatchtimeSec = watchTime;
            foreach (var item in viewHistory)
            {
                if (item.tmdbId == 0 && item.MovieName == MovieName)
                {
                    item.WatchtimeSec = watchTime;
                    setting.AddWatchHistory(viewHistory);
                }
                if (item.tmdbId == tmdbId)
                {
                    item.WatchtimeSec = watchTime;
                    setting.AddWatchHistory(viewHistory);
                }
            }
        }

    }
}