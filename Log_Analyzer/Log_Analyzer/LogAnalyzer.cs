using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Log_Analyzer
{
    class LogAnalyzer
    {
        public const string UnixTimeName = "UnixTime";
        private List<string> noFilteringName = new List<string> { "UnknownNum" };
        internal ReadOnlyCollection<int> NoFiltering { get { return GetNoFilteringIndexes(); } }
        private List<string> keys = new List<string>{"UserID",
                                       "DestinationAddress",
                                       "ProxyAddress",
                                       "Action",
                                       "Status",
                                       "URL"};
        private Dictionary<string, int> loadingOrder = null;
        private List<int> ignoringOrder = new List<int> { };

        public string Name { get; set; } = "default";
        public ReadOnlyCollection<string> Keys { get { return new ReadOnlyCollection<string>(keys); } set { keys = value.Distinct().ToList(); } }
        public ReadOnlyCollection<int> IgnoringOrder { get { return new ReadOnlyCollection<int>(ignoringOrder); } set { ignoringOrder = value.Distinct().ToList(); } }
        public int UnixTimeOrder { get; set; } = 0;

        

        public List<LogData> Load(string filePath)
        {
            var itemCount = Keys.Count + 1 + IgnoringOrder.Count; //+1はunixtime
            if (loadingOrder == null)
            {
                loadingOrder = new Dictionary<string, int>();
                for (int i = 0, j = 0; i < itemCount; i++)
                {
                    if (IgnoringOrder.Contains(i) || i == UnixTimeOrder)
                        continue;
                    else
                    {
                        loadingOrder.Add(Keys[j], i);
                        j++;
                    }
                }
            }
            var result = new List<LogData>();
            var lines = File.ReadAllLines(filePath);
            foreach (var l in lines)
            {
                Dictionary<string, string> stringData = new Dictionary<string, string>();
                bool error = false;
                var data = l.Split(' ').Where(x => x != "").ToArray();
                if (data.Length != itemCount)
                    error = true;
                error = !double.TryParse(data[UnixTimeOrder], out var unixTime);
                foreach (var loading in loadingOrder)
                {
                    stringData.Add(loading.Key, data[loading.Value]);
                }
                if (error == true)
                {
                    throw new LogDataException(null);
                }
                result.Add(new LogData(unixTime, stringData,this));
            }
            return result;
        }

        public List<LogData> KeyFilter(List<LogData> data ,int keyIndex,string comparingString)
        {
            return data.Where(x =>
            {
                x.Data.TryGetValue(Keys[keyIndex], out var v);
                return v == comparingString;
            }).ToList();
        }

        internal void AddNoFilteringName(string[] names)
        {
            foreach (var n in names)
            {
                if (Keys.Contains(n) && !noFilteringName.Contains(n))
                    noFilteringName.Add(n);
            }
        }

        private ReadOnlyCollection<int> GetNoFilteringIndexes()
        {
            var result = new List<int>();
            foreach (var n in noFilteringName)
            {
                result.Add(Keys.IndexOf(n));
            }
            return new ReadOnlyCollection<int>(result);
        }
    }
}
