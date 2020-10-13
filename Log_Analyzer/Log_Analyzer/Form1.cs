using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.XPath;

namespace Log_Analyzer
{
    public partial class Form1 : Form
    {
        private List<LogData> source;
        private List<LogData> showing;
        private string waitingBaseText;

        public Form1()
        {
            InitializeComponent();
            waitingBaseText = label1.Text;
            source = LogAnalyzer.Load("1_access.log");
            showing = source;
        }

        private async void Form1_Shown(object sender, EventArgs e)
        {
            await ShowData();
        }

        private async Task ShowData()
        {
            Show_Waiting();
            dataGridView1.SuspendLayout();
            dataGridView1.Rows.Clear();
            DataGridViewRow[] rows = new DataGridViewRow[showing.Count];
            await Task.Run(()=> 
            {
                double prevUnixTime = showing.First().UnixTime;
                for (int i = 0; i < showing.Count; i++)
                {
                    object[] cellValues = new object[]{ showing[i].DateTimeOffset.ToString("yyyy/MM/dd HH:mm:ss"),
                                                    showing[i].UnixTime,
                                                    (showing[i].UnixTime - prevUnixTime).ToString("F4"),
                                                    showing[i].ProcessID,
                                                    showing[i].UserAddress,
                                                    showing[i].Action,
                                                    showing[i].UnknownNum,
                                                    showing[i].Status,
                                                    showing[i].Url,
                                                    showing[i].UserID,
                                                    showing[i].ProxyStatus,
                                                    showing[i].DocumentType };
                    prevUnixTime = showing[i].UnixTime;
                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(dataGridView1);
                    row.SetValues(cellValues);
                    rows[i] = row;
                }
            });
            dataGridView1.Rows.AddRange(rows);
            dataGridView1.ResumeLayout();
            Close_Waiting();
        }

        private void Show_Waiting()
        {
            pictureBox1.Visible = true;
            label1.Visible = true;
            dataGridView1.Visible = false;
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.BringToFront();
            label1.BringToFront();
            var center = new Point((this.Width - label1.PreferredWidth) / 2, this.Height / 3);
            label1.Location = center;
            int lapse = 0;
            string dot = "";
            timer1.Interval = 250;
            timer1.Enabled = true;
            timer1.Tick += (s, e) =>
            {
                timer1.Enabled = false;
                lapse++;
                dot += '.';
                if(lapse % 4 == 0)
                {
                    lapse = 0;
                    dot = "";
                }
                label1.Text = waitingBaseText + dot;
                timer1.Enabled = true;
            };
        }

        private void Close_Waiting()
        {
            timer1.Enabled = false;
            label1.Visible = false;
            pictureBox1.Visible = false;
            dataGridView1.Visible = true;
        }
    }
}
