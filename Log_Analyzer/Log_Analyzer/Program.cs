using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Log_Analyzer
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var analyzer = FetchDefaultSetting();
            var title = new TitleForm(analyzer);
            Application.Run(title);
            if (title.LoadingFilePath != "")
            {
                Application.Run(new Form1(title.LoadingFilePath, title.Analyzer));
            }
        }

        private const string DIR = "settings";
        private const string DEFAULT_SETTING_FILE_NAME = "default.json";

        private static LogAnalyzer FetchDefaultSetting()
        {
            var defaultSettingFilePath = Path.Combine(DIR, DEFAULT_SETTING_FILE_NAME);
            LogAnalyzer analyzer = null;
            if(Directory.Exists(DIR) && File.Exists(defaultSettingFilePath))
            {
                analyzer = JsonConvert.DeserializeObject<LogAnalyzer>(File.ReadAllText(defaultSettingFilePath));
            }
            if(analyzer == null)
            {
                analyzer = new LogAnalyzer();
                WriteDefaultSettingFile(analyzer);
            }
            return analyzer;
        }

        private static void WriteDefaultSettingFile(LogAnalyzer analyzer)
        {
            if (!Directory.Exists(DIR))
            {
                Directory.CreateDirectory(DIR);
            }
            var defaultSettingFilePath =  Path.Combine(DIR,DEFAULT_SETTING_FILE_NAME);
            File.WriteAllText(defaultSettingFilePath,JsonConvert.SerializeObject(analyzer));
        }
    }
}
