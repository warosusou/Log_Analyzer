﻿using System;
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
        private List<Label> sampleLabels;
        public string LoadingFilePath { get; private set; } = "";
        private const int TEXTBOX_MARGIN = 30;
        private const string IGNORING_LABEL_TEXT = "ignored";
        private readonly Color IGNORING_LABEL_COLOR = Color.Red;
        private const string SEPARATING_TEXT = "  ";
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

            ShowGroupbox1Items();
            if (LogAnalyzer.IgnoringOrder != null)
                ShowGroupbox2Items();

            ShowSamples();
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

        private void ShowGroupbox1Items()
        {
            keyBoxes = new List<TextBox>();
            int x = textBox1.Location.X;
            int y = textBox1.Location.Y;
            groupBox1.Controls.Clear();
            for (int i = 0; i < LogAnalyzer.Keys.Count; i++)
            {
                var t = new TextBox
                {
                    Text = LogAnalyzer.Keys[i],
                    Location = new Point(x, y),
                    Size = textBox1.Size
                };
                keyBoxes.Add(t);
                t.KeyUp += stringBoxes_KeyUp;
                y += TEXTBOX_MARGIN;
            }
            groupBox1.Controls.AddRange(keyBoxes.ToArray());
            textBox1.Visible = false;
        }

        private void ShowGroupbox2Items()
        {
            ignoreBoxes = new List<TextBox>();
            int x = textBox2.Location.X;
            int y = textBox2.Location.Y;
            groupBox2.Controls.Clear();
            for (int i = 0; i < LogAnalyzer.IgnoringOrder.Count; i++)
            {
                var t = new TextBox
                {
                    Text = LogAnalyzer.IgnoringOrder[i].ToString(),
                    Location = new Point(x, y),
                    Size = textBox2.Size
                };
                ignoreBoxes.Add(t);
                t.KeyPress += intBoxes_KeyPress;
                t.KeyUp += intBoxes_KeyUp;
                y += TEXTBOX_MARGIN;
            }
            groupBox2.Controls.AddRange(ignoreBoxes.ToArray());
            textBox2.Visible = false;
        }

        private void ShowSamples()
        {
            if (sampleLabels != null)
                foreach (var l in sampleLabels)
                    this.Controls.Remove(l);
            sampleLabels = new List<Label>();
            var count = LogAnalyzer.IgnoringOrder.Count + 1 + LogAnalyzer.Keys.Count;// +1はUnixTime
            int x = sampleAlignLabel.Location.X;
            int y = sampleAlignLabel.Location.Y;
            int dictionaryIndex = 0;
            for (int i = 0; i < count; i++)
            {
                string text = "";
                sampleLabels.Add(new Label());
                if (LogAnalyzer.IgnoringOrder.Contains(i))
                {
                    text = IGNORING_LABEL_TEXT;
                    sampleLabels[i].ForeColor = IGNORING_LABEL_COLOR;
                }
                else if (i == LogAnalyzer.UnixTimeOrder)
                    text = "UnixTime";
                else
                {
                    text = LogAnalyzer.Keys[dictionaryIndex];
                    dictionaryIndex++;
                }
                sampleLabels[i].Text = String.Format("{{{0}}}{1}", text, SEPARATING_TEXT);
                sampleLabels[i].Location = new Point(x, y);
                sampleLabels[i].Width = sampleLabels[i].PreferredWidth;
                x += sampleLabels[i].PreferredWidth;
            }
            sampleAlignLabel.Visible = false;
            this.Controls.AddRange(sampleLabels.ToArray());
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
            if ((e.KeyChar >= '0' && e.KeyChar <= '9') || (e.KeyChar >= 0 && e.KeyChar <= 31))
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
                CreateTextBox(list, groupBox, keyEventHandler, keyPressEventHandler);
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

        private void CreateTextBox(List<TextBox> list, GroupBox groupBox, KeyEventHandler keyEventHandler, KeyPressEventHandler keyPressEventHandler)
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
            keyBoxes.Remove(keyBoxes.Last());
            ignoreBoxes.Remove(ignoreBoxes.Last());
            try
            {
                if (!Int32.TryParse(textBox3.Text, out var unixTimeOrder))
                    throw new ConfigurationException("数字以外の文字が含まれています", "UnixTimeOrder");
                foreach (var i in ignoreBoxes)
                {
                    if (!Int32.TryParse(i.Text, out var s))
                        throw new ConfigurationException("数字以外の文字が含まれています", "IgnoringOrder");
                    else if (s == unixTimeOrder)
                    {
                        i.SelectAll();
                        i.Focus();
                        throw new ConfigurationException("UnixTimeOrderと重複があります", "IgnoringOrder");
                    }
                }
            }
            catch (ConfigurationException ce)
            {
                MessageBox.Show(String.Format("{0}{1}@{2}", ce.Message, Environment.NewLine, ce.Source), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

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

        private class ConfigurationException : Exception
        {
            public override string Message { get; }

            public ConfigurationException(string message, string source)
            {
                this.Message = message;
                this.Source = source;
            }
        }
    }
}