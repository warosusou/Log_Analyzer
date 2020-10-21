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
        private List<TextBox> keyBoxes;
        private List<TextBox> ignoreBoxes;
        public string LoadingFilePath { get; private set; } = "";
        private const int TEXTBOX_MARGIN = 30;
        private bool CreatingScreen = false;

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
            CreatingScreen = true;
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            keyBoxes = new List<TextBox>();
            int x1 = textBox1.Location.X;
            int y1 = textBox1.Location.Y;
            groupBox1.Controls.Clear();
            for (int i = 0; i < LogAnalyzer.Keys.Count; i++)
            {
                var t = new TextBox
                {
                    Text = LogAnalyzer.Keys[i],
                    Location = new Point(x1, y1),
                    Size = textBox1.Size
                };
                keyBoxes.Add(t);
                t.KeyUp += stringBoxes_KeyUp;
                y1 += TEXTBOX_MARGIN;
            }
            groupBox1.Controls.AddRange(keyBoxes.ToArray());

            if (LogAnalyzer.IgnoringOrder != null)
            {
                ignoreBoxes = new List<TextBox>();
                int x2 = textBox2.Location.X;
                int y2 = textBox2.Location.Y;
                groupBox2.Controls.Clear();
                for (int i = 0; i < LogAnalyzer.IgnoringOrder.Count; i++)
                {
                    var t = new TextBox
                    {
                        Text = LogAnalyzer.IgnoringOrder[i].ToString(),
                        Location = new Point(x2, y2),
                        Size = textBox2.Size
                    };
                    ignoreBoxes.Add(t);
                    t.KeyPress += intBoxes_KeyPress;
                    t.KeyUp += intBoxes_KeyUp;
                    y2 += TEXTBOX_MARGIN;
                }
                groupBox2.Controls.AddRange(ignoreBoxes.ToArray());
            }
            textBox1.Visible = false;
            textBox2.Visible = false;

            textBox3.Text = LogAnalyzer.UnixTimeOrder.ToString();
            textBox3.KeyPress += intBoxes_KeyPress;
            stringBoxes_KeyUp(keyBoxes.Last(), new KeyEventArgs(Keys.None));
            intBoxes_KeyUp(ignoreBoxes.Last(), new KeyEventArgs(Keys.None));
            listBox1.Items.Clear();
            foreach (var d in Directory.GetFiles("./", "*.log"))
            {
                listBox1.Items.Add(Path.GetFileName(d));
            }
            CreatingScreen = false;
        }

        private void stringBoxes_KeyUp(object sender, KeyEventArgs e)
        {
            if (CreatingScreen)
                e.Handled = true;
            CheckTextBoxDeployment((TextBox)sender);
        }

        private void intBoxes_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
            if ((e.KeyChar >= '0' && e.KeyChar <= '9') ||(e.KeyChar >= 0 && e.KeyChar <= 31))
                e.Handled = false;
        }

        private void intBoxes_KeyUp(object sender, KeyEventArgs e)
        {
            if (CreatingScreen)
                e.Handled = true;
            var textBox = (TextBox)sender;
            if (ignoreBoxes.Contains(textBox))
                CheckTextBoxDeployment(textBox);
        }

        private void CheckTextBoxDeployment(TextBox textBox)
        {
            button2.Enabled = true;
            button3.Enabled = true;
            List<TextBox> list;
            GroupBox groupBox;
            KeyEventHandler keyEventHandler;
            KeyPressEventHandler keyPressEventHandler = null;

            if (keyBoxes.Contains(textBox))
            {
                list = keyBoxes;
                groupBox = groupBox1;
                keyEventHandler = stringBoxes_KeyUp;
            }
            else if (ignoreBoxes.Contains(textBox))
            {
                list = ignoreBoxes;
                groupBox = groupBox2;
                keyEventHandler = intBoxes_KeyUp;
                keyPressEventHandler = intBoxes_KeyPress;
            }
            else
                return;
            if (list.Last() != textBox && textBox.Text == "")
                RemoveTextBox(list, groupBox, textBox);
            else if (list.Last() == textBox && textBox.Text != "")
                CreateTextBox(list, groupBox, textBox, keyEventHandler, keyPressEventHandler);
        }

        private void RemoveTextBox(List<TextBox> list, GroupBox groupBox, TextBox textBox)
        {
            groupBox.Controls.Clear();
            int i;
            for (i = list.Count - 1; i > list.IndexOf(textBox); i--)
            {
                list[i].Location = new Point(list[i - 1].Location.X, list[i - 1].Location.Y);
            }
            groupBox.Controls.Remove(textBox);
            list.Remove(textBox);
            textBox.Dispose();
            groupBox.Controls.AddRange(list.ToArray());
            list[i].Focus();
        }

        private void CreateTextBox(List<TextBox> list, GroupBox groupBox, TextBox textBox, KeyEventHandler keyEventHandler, KeyPressEventHandler keyPressEventHandler)
        {
            var t = new TextBox
            {
                Text = "",
                Location = new Point(list.Last().Location.X, list.Last().Location.Y + TEXTBOX_MARGIN),
                Size = list.Last().Size
            };
            t.KeyUp += keyEventHandler;
            if (keyPressEventHandler != null)
                t.KeyPress += keyPressEventHandler;
            list.Add(t);
            groupBox.Controls.Add(t);
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
            for (int i = 0; i < keyBoxes.Count; i++)
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
