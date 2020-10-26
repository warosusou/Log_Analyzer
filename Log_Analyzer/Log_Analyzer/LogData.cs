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

        public LogData(double unixTime, Dictionary<string, string> data,LogAnalyzer analyzer)
        {
            if (data.Count < analyzer.Keys.Count)
            {
                var s = new List<string>(analyzer.Keys);
                foreach (var d in data)
                {
                    s.Remove(d.Key);
                }
                throw new LogDataException(s.ToArray(), true);
            }
            else if (data.Count < analyzer.Keys.Count)
            {
                var dataKeys = new List<string>(data.Keys);
                foreach (var k in analyzer.Keys)
                {
                    dataKeys.Remove(k);
                }
                throw new LogDataException(dataKeys.ToArray(), false);
            }
            DateTimeOffset = DateTimeOffset.FromUnixTimeSeconds((long)unixTime).ToLocalTime();
            UnixTime = unixTime;
            _Data = data;
        }
    }
}
