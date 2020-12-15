using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private string currentFilePath;
        private string waitingBaseText;
        private int[] generatedColumns = { 0, 2 };
        private LogAnalyzer analyzer;
        private ReadOnlyCollection<string> defaultColumn = new ReadOnlyCollection<string>(new List<string>{ "DateTime", "UnixTime", "Interval" });

        internal Form1(string path,LogAnalyzer analyzer)
        {
            InitializeComponent();
            waitingBaseText = label1.Text;
            currentFilePath = path;
            this.analyzer = analyzer;
        }

        private async void Form1_Shown(object sender, EventArgs e)
        {
            this.Text = currentFilePath;
            source = analyzer.Load(currentFilePath);
            showing = source;
            var columnNames = new List<string>(defaultColumn);
            columnNames.AddRange(analyzer.Keys);
            dataGridView1.Columns.Clear();
            foreach (var c in columnNames)
            {
                var d = new DataGridViewTextBoxColumn();
                d.HeaderText = c;
                d.SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView1.Columns.Add(d);
            }
            await ShowData();
        }

        private async Task ShowData()
        {
            Show_Waiting();
            dataGridView1.SuspendLayout();
            dataGridView1.Rows.Clear();
            menuStrip1.Visible = false;
            DataGridViewRow[] rows = new DataGridViewRow[showing.Count];
            await Task.Run(() =>
            {
                if (showing.Count == 0)
                {
                    return;
                }
                showing = showing.OrderBy(x => x.UnixTime).ToList();
                double prevUnixTime = showing.First().UnixTime;
                int nonDictionaryDataCount = 3;
                for (int i = 0; i < showing.Count; i++)
                {
                    object[] cellValues = new object[nonDictionaryDataCount + analyzer.Keys.Count()];
                    cellValues[0] = showing[i].DateTimeOffset.ToString("yyyy/MM/dd HH:mm:ss");
                    cellValues[1] = showing[i].UnixTime;
                    cellValues[2] = IntervalToString(showing[i].UnixTime - prevUnixTime);
                    for (int j = nonDictionaryDataCount; j < analyzer.Keys.Count() + nonDictionaryDataCount; j++)
                    {
                        showing[i].Data.TryGetValue(analyzer.Keys[j - nonDictionaryDataCount], out var value);
                        cellValues[j] = value;
                    }
                    prevUnixTime = showing[i].UnixTime;
                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(dataGridView1);
                    row.SetValues(cellValues);
                    rows[i] = row;
                }
            });
            dataGridView1.Rows.AddRange(rows);
            dataGridView1.ResumeLayout();
            FilterToolStripMenuItem.DropDownItems.Clear();
            var menus = new ToolStripMenuItem[analyzer.Keys.Count + 1];
            for (int i = 0; i < analyzer.Keys.Count; i++)
            {
                menus[i] = new ToolStripMenuItem { Text = analyzer.Keys[i] };
                menus[i].Click += FilterMenuItemClicked;
            }
            menus[menus.Length - 1] = new ToolStripMenuItem { Text = "フィルタ初期化" };
            menus[menus.Length - 1].Click += async(s, e) => 
            {
                showing = source;
                await ShowData();
            };
            FilterToolStripMenuItem.DropDownItems.AddRange(menus);
            menuStrip1.Visible = true;
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
                if (lapse % 4 == 0)
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

        private string IntervalToString(double source)
        {
            const string format = "+{0}s";
            double limit = 10;
            if (source < limit)
                return source.ToString("F4");
            else
            {
                if ((int)limit == limit)
                    return String.Format(format, limit.ToString("F1"));
                else
                    return String.Format(format, limit.ToString());
            }
        }

        private int FindUnixTimeColumnIndex()
        {
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                if (dataGridView1.Columns[i].Name == LogAnalyzer.UnixTimeName)
                    return i;
            }
            return -1;
        }

        private async void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count <= e.RowIndex || e.RowIndex < 0)
                return;
            if (dataGridView1.Rows[e.RowIndex].Cells.Count <= e.ColumnIndex || e.ColumnIndex < 0)
                return;
            object data = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            if (data == null)
                return;
            if (e.ColumnIndex == FindUnixTimeColumnIndex())
                showing = analyzer.UnixTimeFilter(showing, (double)data);
            else
            {
                if (generatedColumns.Contains(e.ColumnIndex))
                    return;
                var target = analyzer.Keys.IndexOf(dataGridView1.Columns[e.ColumnIndex].Name);
                showing = analyzer.KeyFilter(showing, target, (string)data);
            }

            await ShowData();
        }

        private async void FilterMenuItemClicked(object sender, EventArgs e)
        {
            var t = (ToolStripMenuItem)sender;
            if (t == null)
                return;
            var index = analyzer.Keys.IndexOf(t.Text);
            var source = showing.Select(x =>
            {
                x.Data.TryGetValue(analyzer.Keys[index],out var s);
                return s;
            }).Distinct().ToArray();
            var filter = new FilteringForm(t.Text,source);
            filter.ShowDialog();
            if (filter.SelectedFilter == "")
                return;
            showing = analyzer.KeyFilter(showing, index, filter.SelectedFilter);
            await ShowData();
        }
    }
}
