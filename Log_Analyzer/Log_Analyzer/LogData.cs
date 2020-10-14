using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log_Analyzer
{
    class LogData
    {
        public DateTimeOffset DateTimeOffset { get; }
        public double UnixTime { get; }
        public ReadOnlyDictionary<string, string> Data { get { return new ReadOnlyDictionary<string, string>(_Data); } }
        private Dictionary<string, string> _Data;
        private static List<string> noFilteringName = new List<string> { "UnknownNum" };

        public static string[] Keys = {"ProcessID",
                                       "UserAddress",
                                       "Action",
                                       "UnknownNum",
                                       "Status",
                                       "URL",
                                       "UserID",
                                       "ProxyStatus",
                                       "DocumentType"};

        public static ReadOnlyCollection<int> NoFiltering { get { return GetNoFilteringIndexes(); } }

        public LogData(double unixTime, Dictionary<string, string> data)
        {
            if (data.Count != LogData.Keys.Length)
            {
                var s = new List<string>(LogData.Keys);
                foreach (var d in data)
                {
                    s.Remove(d.Key);
                }
                throw new DataItemShortageException(s.ToArray());
            }
            DateTimeOffset = DateTimeOffset.FromUnixTimeSeconds((long)unixTime).ToLocalTime();
            UnixTime = unixTime;
            _Data = data;
        }

        internal static void AddNoFilteringName(string[] names)
        {
            foreach (var n in names)
            {
                if (LogData.Keys.Contains(n) && !LogData.noFilteringName.Contains(n))
                    noFilteringName.Add(n);
            }
        }

        private static ReadOnlyCollection<int> GetNoFilteringIndexes()
        {
            var result = new List<int>();
            foreach (var n in noFilteringName)
            {
                result.Add(Array.IndexOf(LogData.Keys, n));
            }
            return new ReadOnlyCollection<int>(result);
        }
    }

    class DataItemShortageException : Exception
    {
        public string[] Shortage { get; }
        public override string Message { get { return "LogDataに必要な情報が足りません"; } }

        public DataItemShortageException(string[] shortage)
        {
            this.Shortage = shortage;
        }
    }
}
