using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log_Analyzer
{
    class LogDataException : Exception
    {
        public string[] Items { get; }
        public bool? IsShortage { get; }
        public bool LoadingFault { get; }
        public override string Message { get; }

        public LogDataException(string[] items)
        {
            this.Items = items;
            IsShortage = null;
            Message = "ログファイルの形式が正しくありません";
            if (items == null)
                LoadingFault = true;
            else
                LoadingFault = false;
        }

        public LogDataException(string[] items, bool isShortage)
        {
            this.Items = items;
            IsShortage = isShortage;
            Message = "ログファイルの形式が正しくありません";
        }
    }
}
