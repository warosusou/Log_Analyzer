using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Log_Analyzer
{
    public partial class TitleForm : Form
    {
        private TextBox[] keyBoxes;
        private TextBox[] ignoreBoxes;
        public string LoadingFilePath { get; private set; } = "";

        public TitleForm()
        {
            InitializeComponent();
        }

        private void TitleForm_Load(object sender, EventArgs e)
        {
            CreateScreen();
        }

        private void CreateScreen()
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            keyBoxes = new TextBox[LogAnalyzer.Keys.Count];
            int x1 = textBox1.Location.X;
            int y1 = textBox1.Location.Y;
            for (int i = 0; i < keyBoxes.Length; i++)
            {
                keyBoxes[i] = new TextBox
                {
                    Text = LogAnalyzer.Keys[i],
                    Location = new Point(x1, y1),
                    Size = textBox1.Size
                };
                keyBoxes[i].KeyDown += (s, e) => { button2.Enabled = true; button3.Enabled = true; };
                y1 += 30;
            }
            groupBox1.Controls.AddRange(keyBoxes);
            //this.Controls.AddRange(keyBoxes);

            if (LogAnalyzer.IgnoringOrder != null)
            {
                ignoreBoxes = new TextBox[LogAnalyzer.IgnoringOrder.Count];
                int x2 = textBox2.Location.X;
                int y2 = textBox2.Location.Y;
                for (int i = 0; i < ignoreBoxes.Length; i++)
                {
                    ignoreBoxes[i] = new TextBox
                    {
                        Text = LogAnalyzer.IgnoringOrder[i].ToString(),
                        Location = new Point(x2, y2),
                        Size = textBox2.Size
                    };
                    ignoreBoxes[i].KeyPress += intBoxes_KeyPress;
                    y2 += 30;
                }
                groupBox2.Controls.AddRange(ignoreBoxes);
                //this.Controls.AddRange(ignoreBoxes);
            }
            textBox1.Visible = false;
            textBox2.Visible = false;

            textBox3.Text = LogAnalyzer.UnixTimeOrder.ToString();
            textBox3.KeyPress += intBoxes_KeyPress;

            listBox1.Items.Clear();
            foreach (var d in Directory.GetFiles("./", "*.log"))
            {
                listBox1.Items.Add(Path.GetFileName(d));
            }
        }

        private void intBoxes_KeyPress(object sender, KeyPressEventArgs e)
        {
            button2.Enabled = true;
            button3.Enabled = true;
            if (e.KeyChar < '0' || e.KeyChar > '9')
                e.Handled = true;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
                button1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadingFilePath = (string)listBox1.SelectedItem;
            if (LoadingFilePath == "")
                CreateScreen();
            else
                this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LogAnalyzer.Keys = new List<string>();
            for (int i = 0; i < keyBoxes.Length; i++)
            {
                if (keyBoxes[i].Text != "")
                    LogAnalyzer.Keys.Add(keyBoxes[i].Text);
            }
            LogAnalyzer.IgnoringOrder = new List<int>();
            foreach (var i in ignoreBoxes)
            {
                Int32.TryParse(i.Text, out var index);
                LogAnalyzer.IgnoringOrder.Add(index);
            }
            Int32.TryParse(textBox3.Text, out var unixIndex);
            LogAnalyzer.UnixTimeOrder = unixIndex;
            CreateScreen();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CreateScreen();
        }
    }
}
