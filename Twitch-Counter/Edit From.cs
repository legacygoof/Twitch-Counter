using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Twitch_Counter
{
    public partial class Edit_From : Form
    {
        List<Counter> counterList;
        int index;
        int bind1;
        int bind2;
        int bind3;
        public Edit_From(List<Counter> cl, int i)
        {
            KeysConverter kc = new KeysConverter();
            counterList = cl;
            index = i;
            InitializeComponent();
            textBox1.Text = cl[i].Name;
            textBox2.Text = cl[i].Format;
            Type countType = (Type)cl[i].Type;
            switch(countType)
            {
                case Type.OneCounter: label5.Hide(); label6.Hide(); label7.Hide(); label8.Hide(); OneCounter oc = (OneCounter)cl[i]; label4.Text = kc.ConvertToString(oc.CounterOneBind); break;
                case Type.TwoCounters: label7.Hide(); label8.Hide(); TwoCounters tc = (TwoCounters)cl[i]; label6.Text = kc.ConvertToString(tc.CounterTwoBind); label4.Text = kc.ConvertToString(tc.CounterOneBind); MessageBox.Show(tc.CounterOneBind.ToString()); break;
                case Type.TwoCountersRatio: label7.Hide(); label8.Hide(); TwoCountersRatio tcr = (TwoCountersRatio)cl[i]; label6.Text = kc.ConvertToString(tcr.CounterTwoBind); label4.Text = kc.ConvertToString(tcr.CounterOneBind); break;
                case Type.ThreeCounters: ThreeCounters ttc = (ThreeCounters)cl[i]; label8.Text = kc.ConvertToString(ttc.CounterThreeBind); label6.Text = kc.ConvertToString(ttc.CounterTwoBind); label4.Text = kc.ConvertToString(ttc.CounterOneBind); break;
            }
        }

        private void Edit_From_Load(object sender, EventArgs e)
        {
            this.Icon = Twitch_Counter.Properties.Resources.twitch_LuI_icon;
        }

        private void label4_Click(object sender, EventArgs e)
        {
            label4.Focus();
            label4.PreviewKeyDown += label4_PreviewKeyDown;
        }

        private void label4_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bind1 = e.KeyValue;
            label4.Text = e.KeyCode.ToString();
            label3.Focus();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            label6.Focus();
            label6.PreviewKeyDown += label6_PreviewKeyDown;
        }

        private void label6_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bind2 = e.KeyValue;
            label6.Text = e.KeyCode.ToString();
            label3.Focus();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            label4.Focus();
            label4.PreviewKeyDown += label8_PreviewKeyDown;
        }

        private void label8_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bind3 = e.KeyValue;
            label8.Text = e.KeyCode.ToString();
            label3.Focus();
        }
    }
}
