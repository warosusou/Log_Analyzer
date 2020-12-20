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
    public partial class UnixTimeFilteringForm : Form
    {
        public DateTime? First { get; private set; }
        public DateTime? Last { get; private set; }
        public UnixTimeFilteringForm(double firstUnixTime,double lastUnixTime)
        {
            InitializeComponent();
            this.First = null;
            this.Last = null;
            var first = (long)(firstUnixTime * 1000);
            var last = (long)(lastUnixTime * 1000);
            dateTimePicker1.Value = DateTimeOffset.FromUnixTimeMilliseconds(first).DateTime.ToLocalTime();
            dateTimePicker2.Value = DateTimeOffset.FromUnixTimeMilliseconds(last).DateTime.ToLocalTime();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            var temp = dateTimePicker1.Value;
            dateTimePicker1.Value = dateTimePicker2.Value;
            dateTimePicker2.Value = temp;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if(dateTimePicker1.Value > dateTimePicker2.Value)
            {
                MessageBox.Show("時間指定が無効です", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.First = dateTimePicker1.Value;
            this.Last = dateTimePicker2.Value;
            this.Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
