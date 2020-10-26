using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            var analyzer = new LogAnalyzer();
            var title = new TitleForm(analyzer);
            Application.Run(title);
            if (title.LoadingFilePath != "")
                Application.Run(new Form1(title.LoadingFilePath,title.Analyzer));
        }
    }
}
