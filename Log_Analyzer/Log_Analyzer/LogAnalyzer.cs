using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log_Analyzer
{
    static class LogAnalyzer
    {
        private static List<string> noFilteringName = new List<string> { "UnknownNum" };
        public static ReadOnlyCollection<int> NoFiltering { get { return GetNoFilteringIndexes(); } }
        internal static string[] Keys = {"ProcessID",
                                       "UserAddress",
                                       "Action",
                                       "UnknownNum",
                                       "Status",
                                       "URL",
                                       "UserID",
                                       "ProxyStatus",
                                       "DocumentType"};
        private static Dictionary<string, int> loadingOrder = null;
        private static List<int> ignoreIndex = new List<int> { 0, 1 };
        private static int unixTimeOrder = 2;

        static public List<LogData> Load(string filePath)
        {
            var itemCount = Keys.Length + 1 + ignoreIndex.Count; //+1はunixtime
            if (loadingOrder == null)
            {
                loadingOrder = new Dictionary<string, int>();
                for (int i = 0,j = 0; i < itemCount; i++)
                {
                    if (ignoreIndex.Contains(i) || i == unixTimeOrder)
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
                error = !double.TryParse(data[unixTimeOrder], out var unixTime);
                foreach(var loading in loadingOrder)
                {
                    stringData.Add(loading.Key, data[loading.Value]);
                }
                if (error == true)
                {
                    throw new LogDataException(null);
                }
                result.Add(new LogData(unixTime, stringData));
            }
            return result;
        }

        internal static void AddNoFilteringName(string[] names)
        {
            foreach (var n in names)
            {
                if (Keys.Contains(n) && !noFilteringName.Contains(n))
                    noFilteringName.Add(n);
            }
        }

        private static ReadOnlyCollection<int> GetNoFilteringIndexes()
        {
            var result = new List<int>();
            foreach (var n in noFilteringName)
            {
                result.Add(Array.IndexOf(Keys, n));
            }
            return new ReadOnlyCollection<int>(result);
        }
    }
}
