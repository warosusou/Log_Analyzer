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
    public partial class FilteringForm : Form
    {
        private string[] FilteringSources;
        private string FilteringTitle;
        public string SelectedFilter { get; private set; } = "";

        public FilteringForm(string filteringTitle , string[] filteringSources)
        {
            FilteringSources = filteringSources;
            FilteringTitle = filteringTitle;
            InitializeComponent();
        }

        private void FilteringForm_Load(object sender, EventArgs e)
        {
            label1.Text = FilteringTitle + label1.Text;
            listBox1.Items.AddRange(FilteringSources);            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SelectedFilter = (string)listBox1.SelectedItem;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
