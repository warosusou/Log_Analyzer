using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log_Analyzer
{
    class LogData
    {
        public DateTimeOffset DateTimeOffset { get; }
        public double UnixTime { get; }
        public int ProcessID { get; }
        public string UserAddress { get; }
        public string Action { get; }
        public int UnknownNum { get;}
        public string Status { get; }
        public string Url { get;  }
        public string UserID { get; }
        public string ProxyStatus { get;}
        public string DocumentType { get;}

        public LogData( double unixTime,
                        int processID,
                        string userAddress,
                        string action,
                        int unknownNum,
                        string status,
                        string url,
                        string userID,
                        string proxyStatus,
                        string documentType)
        {
            DateTimeOffset = DateTimeOffset.FromUnixTimeSeconds((long)unixTime).ToLocalTime();
            UnixTime = unixTime;
            ProcessID = processID;
            UserAddress = userAddress;
            Action = action;
            UnknownNum = unknownNum;
            Status = status;
            Url = url;
            UserID = userID;
            ProxyStatus = proxyStatus;
            DocumentType = documentType;
        }
    }
}
