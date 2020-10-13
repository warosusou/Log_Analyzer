using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log_Analyzer
{
    static class LogAnalyzer
    {
       static public List<LogData> Load(string filePath)
        {
            var result = new List<LogData>();
            var lines =  File.ReadAllLines(filePath);
            foreach (var l in lines)
            {
                bool error = false;
                var data = l.Split(' ').Where(x => x != "").ToArray();
                if (data.Length != 11)
                    error = true;
                //data[0]はdatetime型 data[1]はタイムゾーン
                error = !(double.TryParse(data[2], out var unixTime));
                error = !(Int32.TryParse(data[3], out var processID));
                var userAddress = data[4];
                var action = data[5];
                error = !(Int32.TryParse(data[6], out var unknownNum));
                var status = data[7];
                var url = data[8];
                var userID = data[9];
                var proxyStatus = data[10];
                var documentType = data[11];
                if (error == true)
                    return null;
                result.Add(new LogData(unixTime,
                                        processID,
                                        userAddress,
                                        action,
                                        unknownNum,
                                        status,
                                        url,
                                        userID,
                                        proxyStatus,
                                        documentType));
            }
            return result;
        }
    }
}
