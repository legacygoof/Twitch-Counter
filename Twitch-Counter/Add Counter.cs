using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

namespace Twitch_Counter
{
    public partial class Add_Counter : Form
    {
        public string format = "$c1 - Returns value of the counter";
        public int bind1 = 103;//103   np7
        public int bind2 = 104;//104   np8 so did it work?
        public int bind3 = 105;//105   np9
        public Add_Counter()
        {
            InitializeComponent();
        }

        private void Add_Counter_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            comboBox1.SelectedIndex = 0;
            this.Icon = Twitch_Counter.Properties.Resources.twitch_LuI_icon;
        }

        private void updateRichTextBox()
        {
            richTextBox1.Text = format;
        }

        private void comboBox_Index_Changed(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 1: { textBox2.Text = "$c1 - $c2"; label4.Show(); label5.Show(); label6.Hide(); label7.Hide(); format = "$c1 - Returns value of the counter\n$c2 - Returns value of the counter"; updateRichTextBox(); } break;
                case 3: { textBox2.Text = "Wins: $c1 - Ties: $c2 - Loss: $c3"; label4.Show(); label5.Show(); label6.Show(); label7.Show(); format = "$c1 -  Returns value of the counter\n$c2 -  Returns value of the counter\n$c3 -  Returns value of the counter"; updateRichTextBox(); } break;
                case 2: { textBox2.Text = "$c1 - $c2 W/L: $ratio"; label4.Show(); label5.Show(); label6.Hide(); label7.Hide(); format = "$c1 -Returns value of the counter\n$c2 - Returns value of the counter\n$ratio - Returns the ratio of $c1/$c2"; updateRichTextBox(); } break;
                case 0: { textBox2.Text = "Wins: $c1"; label6.Hide(); label7.Hide(); label4.Hide(); label5.Hide(); format = "$c1 - Returns value of the counter"; updateRichTextBox(); } break;
                   
                default: { label7.Visible = false; label8.Visible = false; } break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string jsonText = File.ReadAllText("Counters.json");
            try
            {
                var obj = JsonConvert.DeserializeObject<dynamic>(jsonText);
                switch(comboBox1.SelectedIndex)
                {
                    case 0: { OneCounter oc = new OneCounter { Name = textBox1.Text, CounterOne = 0, CounterOneBind = bind1, Format = textBox2.Text, Type = 0 }; obj.Counters.Add(JToken.Parse(JsonConvert.SerializeObject(oc, Formatting.Indented))); } break;
                    case 1: { TwoCounters tc = new TwoCounters { Name = textBox1.Text, CounterOne = 0, CounterTwo = 0, CounterTwoBind = bind2, CounterOneBind = bind1, Format = textBox2.Text, Type = 0 }; obj.Counters.Add(JToken.Parse(JsonConvert.SerializeObject(tc, Formatting.Indented))); } break;
                    case 2: { TwoCountersRatio tcr = new TwoCountersRatio { Name = textBox1.Text, CounterOne = 0, CounterTwo = 0, CounterTwoBind = bind2, CounterOneBind = bind1, Format = textBox2.Text, Type = 0, CounterRatio = 0.0 }; obj.Counters.Add(JToken.Parse(JsonConvert.SerializeObject(tcr, Formatting.Indented))); } break;
                    case 3: { ThreeCounters ttc = new ThreeCounters { Name = textBox1.Text, CounterOne = 0, CounterTwo = 0, CounterTwoBind = bind2, CounterOneBind = bind1, CounterThreeBind = bind3, CounterThree = 0, Format = textBox2.Text, Type = 0 }; obj.Counters.Add(JToken.Parse(JsonConvert.SerializeObject(ttc, Formatting.Indented))); } break;
                }
                //MessageBox.Show(obj.ToString());
                File.WriteAllText("Counters.json", obj.ToString());
                //MessageBox.Show(obj.ToString());
            }
            catch(JsonException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            this.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            label3.Focus();
            label3.PreviewKeyDown += label3_PreviewKeyDown;
        }

        private void label3_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bind1 = e.KeyValue;
            label3.Text = e.KeyCode.ToString();
            this.Focus();
        }
        private void label5_Click(object sender, EventArgs e)
        {
            label5.Focus();
            label5.PreviewKeyDown += label5_PreviewKeyDown;
        }

        private void label5_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bind2 = e.KeyValue;
            label5.Text = e.KeyCode.ToString();
            this.Focus();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            label7.Focus();
            label7.PreviewKeyDown += label7_PreviewKeyDown;
        }

        private void label7_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bind3 = e.KeyValue;
            label7.Text = e.KeyCode.ToString();
            this.Focus();
        }
    }
}
