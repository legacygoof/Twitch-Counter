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
        private static string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Twitch Counter";
        public string jsonFilePath = path + "\\Counters.json";
        public string format = "$c1 - Returns value of the counter";
        public int bind1 = 103;//103   np7
        public int bind2 = 104;//104   np8 so did it work?
        public int bind3 = 105;//105   np9
        OneCounter oc;
        TwoCounters tc;
        TwoCountersRatio tcr;
        ThreeCounters ttc;
        public Add_Counter()
        {
            InitializeComponent();
        }

        private void Add_Counter_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            comboBox1.SelectedIndex = 0;
            this.Icon = Twitch_Counter.Properties.Resources.twitch_LuI_icon;
        }

        private void initializeText(Type t)
        {
            string text = "";
            string name = "";
            switch (t)
            {
                case Twitch_Counter.Type.OneCounter:
                    {
                       // oc = (Twitch_Counter.OneCounter)counterList[index];
                        text = oc.Format;
                        text = text.Replace("$c1", oc.CounterOne.ToString());
                        name = oc.Name;
                    }
                    break;
                case Twitch_Counter.Type.TwoCounters:
                    {
                       // tc = (Twitch_Counter.TwoCounters)counterList[index];
                        text = tc.Format;
                        text = text.Replace("$c1", tc.CounterOne.ToString());
                        text = text.Replace("$c2", tc.CounterTwo.ToString());
                        name = tc.Name;
                    }
                    break;
                case Twitch_Counter.Type.TwoCountersRatio:
                    {
                        //tcr = (Twitch_Counter.TwoCountersRatio)counterList[index];
                        text = tcr.Format;
                        text = text.Replace("$c1", tcr.CounterOne.ToString());
                        text = text.Replace("$c2", tcr.CounterTwo.ToString());
                        text = text.Replace("$ratio", tcr.CounterRatio.ToString());
                        name = tcr.Name;
                    }
                    break;
                case Twitch_Counter.Type.ThreeCounters:
                    {
                        //ttc = (Twitch_Counter.ThreeCounters)counterList[index];
                        text = ttc.Format;
                        text = text.Replace("$c1", ttc.CounterOne.ToString());
                        text = text.Replace("$c2", ttc.CounterTwo.ToString());
                        text = text.Replace("$c3", ttc.CounterThree.ToString());
                        name = ttc.Name;

                    }
                    break;
            }
            File.WriteAllText(path + "\\Text Files/" + name + ".txt", text);
        }

        private void updateRichTextBox()
        {
            richTextBox1.Text = format;
        }

        private void comboBox_Index_Changed(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 1: { textBox2.Text = "$c1 - $c2"; textBox1.Text = " Two Counter"; label4.Show(); label5.Show(); label6.Hide(); label7.Hide(); format = "$c1 - Returns value of the counter\n$c2 - Returns value of the counter"; updateRichTextBox(); } break;
                case 3: { textBox2.Text = "Wins: $c1 - Ties: $c2 - Loss: $c3"; textBox1.Text = " Three Counter"; label4.Show(); label5.Show(); label6.Show(); label7.Show(); format = "$c1 -  Returns value of the counter\n$c2 -  Returns value of the counter\n$c3 -  Returns value of the counter"; updateRichTextBox(); } break;
                case 2: { textBox2.Text = "$c1 - $c2 W/L: $ratio"; textBox1.Text = " Two Counter Ratio"; label4.Show(); label5.Show(); label6.Hide(); label7.Hide(); format = "$c1 -Returns value of the counter\n$c2 - Returns value of the counter\n$ratio - Returns the ratio of $c1/$c2"; updateRichTextBox(); } break;
                case 0: { textBox2.Text = "Wins: $c1"; textBox1.Text = " One Counter"; label6.Hide(); label7.Hide(); label4.Hide(); label5.Hide(); format = "$c1 - Returns value of the counter"; updateRichTextBox(); } break;
                   
                default: { label7.Visible = false; label8.Visible = false; } break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string jsonText = File.ReadAllText(jsonFilePath);
            try
            {
                var obj = JsonConvert.DeserializeObject<dynamic>(jsonText);
                switch(comboBox1.SelectedIndex)
                {
                    case 0: { oc = new OneCounter { Name = textBox1.Text, CounterOne = 0, CounterOneBind = bind1, Format = textBox2.Text, Type = 0 }; obj.Counters.Add(JToken.Parse(JsonConvert.SerializeObject(oc, Formatting.Indented))); initializeText((Type)oc.Type); } break;
                    case 1: { tc = new TwoCounters { Name = textBox1.Text, CounterOne = 0, CounterTwo = 0, CounterTwoBind = bind2, CounterOneBind = bind1, Format = textBox2.Text, Type = 1 }; obj.Counters.Add(JToken.Parse(JsonConvert.SerializeObject(tc, Formatting.Indented))); initializeText((Type)tc.Type); } break;
                    case 2: { tcr = new TwoCountersRatio { Name = textBox1.Text, CounterOne = 0, CounterTwo = 0, CounterTwoBind = bind2, CounterOneBind = bind1, Format = textBox2.Text, Type = 2, CounterRatio = 0.0 }; obj.Counters.Add(JToken.Parse(JsonConvert.SerializeObject(tcr, Formatting.Indented))); initializeText((Type)tcr.Type); } break;
                    case 3: { ttc = new ThreeCounters { Name = textBox1.Text, CounterOne = 0, CounterTwo = 0, CounterTwoBind = bind2, CounterOneBind = bind1, CounterThreeBind = bind3, CounterThree = 0, Format = textBox2.Text, Type = 3 }; obj.Counters.Add(JToken.Parse(JsonConvert.SerializeObject(ttc, Formatting.Indented))); initializeText((Type)ttc.Type); } break;
                }
                //MessageBox.Show(obj.ToString());
                File.WriteAllText(jsonFilePath, obj.ToString());
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
