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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Twitch_Counter
{
    public partial class Edit_From : Form
    {
        private static string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Twitch Counter";
        public string jsonFilePath = path + "\\Counters.json";
        List<Counter> counterList;
        Type type;
        int index;
        int bind1;
        int bind2;
        int bind3;
        OneCounter oc;
        TwoCounters tc;
        TwoCountersRatio tcr;
        ThreeCounters ttc;
        public Edit_From(List<Counter> cl, int i)
        {
            KeysConverter kc = new KeysConverter();
            this.MaximizeBox = false;
            counterList = cl;
            index = i;
            InitializeComponent();
            textBox1.Text = cl[i].Name;
            textBox2.Text = cl[i].Format;
            Type countType = (Type)cl[i].Type;
            type = countType;
            switch(countType)
            {
                case Type.OneCounter: label5.Hide(); label6.Hide(); label7.Hide(); label8.Hide(); oc = (OneCounter)cl[i]; label4.Text = kc.ConvertToString(oc.CounterOneBind); bind1 = oc.CounterOneBind; break;
                case Type.TwoCounters: label7.Hide(); label8.Hide(); tc = (TwoCounters)cl[i]; label6.Text = kc.ConvertToString(tc.CounterTwoBind); label4.Text = kc.ConvertToString(tc.CounterOneBind);  bind1 = tc.CounterOneBind; bind2 = tc.CounterTwoBind; break;
                case Type.TwoCountersRatio: label7.Hide(); label8.Hide(); tcr = (TwoCountersRatio)cl[i]; label6.Text = kc.ConvertToString(tcr.CounterTwoBind); label4.Text = kc.ConvertToString(tcr.CounterOneBind); bind1 = tcr.CounterOneBind; bind2 = tcr.CounterTwoBind; break;
                case Type.ThreeCounters: ttc = (ThreeCounters)cl[i]; label8.Text = kc.ConvertToString(ttc.CounterThreeBind); label6.Text = kc.ConvertToString(ttc.CounterTwoBind); label4.Text = kc.ConvertToString(ttc.CounterOneBind); bind1 = ttc.CounterOneBind; bind2 = ttc.CounterTwoBind; bind3 = ttc.CounterThreeBind; break;
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
            label8.PreviewKeyDown += label8_PreviewKeyDown;
        }

        private void label8_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bind3 = e.KeyValue;
            label8.Text = e.KeyCode.ToString();
            label3.Focus();
        }

        private void saveEdits()
        {
            string jsonTxt = File.ReadAllText(jsonFilePath);

            try
            {
                var obj = JsonConvert.DeserializeObject<dynamic>(jsonTxt);
                obj.Counters.RemoveAt(index);
                switch(type)
                {
                    case Type.OneCounter: oc.CounterOneBind = bind1; oc.Name = textBox1.Text; oc.Format = textBox2.Text; obj.Counters.Insert(index, JToken.Parse(JsonConvert.SerializeObject(oc, Formatting.Indented))); break;
                    case Type.TwoCounters: tc.CounterOneBind = bind1; tc.CounterTwoBind = bind2; tc.Name = textBox1.Text; tc.Format = textBox2.Text; obj.Counters.Insert(index, JToken.Parse(JsonConvert.SerializeObject(tc, Formatting.Indented))); break;
                    case Type.TwoCountersRatio: tcr.CounterOneBind = bind1; tcr.CounterTwoBind = bind2; tcr.Name = textBox1.Text; tcr.Format = textBox2.Text; obj.Counters.Insert(index, JToken.Parse(JsonConvert.SerializeObject(tcr, Formatting.Indented))); break;
                    case Type.ThreeCounters: ttc.CounterOneBind = bind1; ttc.CounterTwoBind = bind2; ttc.CounterThreeBind = bind3; ttc.Name = textBox1.Text; ttc.Format = textBox2.Text; obj.Counters.Insert(index, JToken.Parse(JsonConvert.SerializeObject(ttc, Formatting.Indented))); break;
                }
                File.WriteAllText(jsonFilePath, obj.ToString());
                this.Close();
            }
            catch (JsonException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
         

        //save
        private void button1_Click(object sender, EventArgs e)
        {
            saveEdits();
        }
    }
}
