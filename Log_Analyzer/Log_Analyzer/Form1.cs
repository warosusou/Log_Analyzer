using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Log_Analyzer
{
    public partial class Form1 : Form
    {
        private List<LogData> source;
        private List<LogData> showing;
        public Form1()
        {
            InitializeComponent();
            source = LogAnalyzer.Load("1_access.log");
            showing = source;
            ShowData();
        }

        private void ShowData()
        {
            foreach(var s in showing)
            {
                dataGridView1.Rows.Add(s.DateTime,
                                        s.UnixTime,
                                        s.ProcessID,
                                        s.UserAddress,
                                        s.Action,
                                        s.UnknownNum,
                                        s.Status,
                                        s.Url,
                                        s.UserID,
                                        s.ProxyStatus,
                                        s.DocumentType);
            }
        }
    }
}
