﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Twitch_Counter
{
    public partial class Main : Form
    {

        public List<Counter> counterList = new List<Counter>();
        string jsonTxt;
        ContextMenuStrip cm = new ContextMenuStrip();
        Edit_From editForm;
        public Main()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Icon = Twitch_Counter.Properties.Resources.twitch_LuI_icon;
            UpdateJson();
            cm.ItemClicked += new ToolStripItemClickedEventHandler(ContextMenu_ItemClicked);
            cm.Items.Add("Edit");
            cm.Items.Add("Reset Values");
            cm.Items.Add("Remove");
        }

        private void ContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                switch (e.ClickedItem.ToString())
                {
                    case "Edit": setupEditForm(listBox1.SelectedIndex); break;
                    case "Reset Values": break;
                    case "Remove": removeJsonItem(listBox1.SelectedIndex); break;
                }
            }
        }

        private void setupEditForm(int index)
        {
            editForm = new Edit_From(counterList, index);
            editForm.FormClosed += EditForm_FormClosed;
            editForm.Show();
        }

        private void EditForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            MessageBox.Show("worked");
            //updateText(counterList[listBox1.SelectedIndex].format);
            UpdateJson();
        }

        private void UpdateList()
        {
            listBox1.Items.Clear();
            foreach (Counter c in counterList)
                listBox1.Items.Add(c.Name);
        }

        private void UpdateJson()
        {
            jsonTxt = File.ReadAllText("Counters.json");
            //MessageBox.Show(jsonTxt);
            counterList.Clear();
            try
            {
                var obj = JsonConvert.DeserializeObject<dynamic>(jsonTxt);
                foreach (var item in obj.Counters)
                {
                    if (item.type.ToString() == "0")
                    {
                        counterList.Add(new OneCounter { Counter = Int16.Parse(item.counter1.ToString()), Format = item.format.ToString(), Name = item.name, Type = Int16.Parse(item.type.ToString()), CounterOneBind = Int16.Parse(item.counter1_bind.ToString()) });
                    }
                    else if(item.type.ToString() == "1")
                    {
                        counterList.Add(new TwoCounters { CounterOne = Int16.Parse(item.counter1.ToString()), CounterTwo = Int16.Parse(item.counter1.ToString()), Format = item.format.ToString(), Name = item.name, Type = Int16.Parse(item.type.ToString()), CounterOneBind = Int16.Parse(item.counter1_bind.ToString()), CounterTwoBind = Int16.Parse(item.counter2_bind.ToString()) });
                    }
                    else if (item.type.ToString() == "2")
                    {
                        counterList.Add(new TwoCountersRatio { CounterOne = Int16.Parse(item.counter1.ToString()), CounterTwo = Int16.Parse(item.counter1.ToString()), CounterRatio = Double.Parse(item.ratio.ToString()), Format = item.format.ToString(), Name = item.name, Type = Int16.Parse(item.type.ToString()), CounterOneBind = Int16.Parse(item.counter1_bind.ToString()), CounterTwoBind = Int16.Parse(item.counter2_bind.ToString()) });
                    }
                    else if (item.type.ToString() == "3")
                    {
                        counterList.Add(new ThreeCounters { CounterOne = Int16.Parse(item.counter1.ToString()), CounterTwo = Int16.Parse(item.counter1.ToString()), CounterThree = Int16.Parse(item.counter1.ToString()), Format = item.format.ToString(), Name = item.name, Type = Int16.Parse(item.type.ToString()), CounterOneBind = Int16.Parse(item.counter1_bind.ToString()), CounterTwoBind = Int16.Parse(item.counter2_bind.ToString()), CounterThreeBind = Int16.Parse(item.counter3_bind.ToString()) });
                    }
                }
            }
            catch(JsonReaderException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            UpdateList();

        }

        private void removeJsonItem(int i)
        {
            jsonTxt = File.ReadAllText("Counters.json");
            var jObject = JsonConvert.DeserializeObject<dynamic>(jsonTxt);
            jObject.Counters.RemoveAt(i);
            File.WriteAllText("Counters.json", jObject.ToString());
            //MessageBox.Show(jObject.ToString());
            UpdateJson();
        }

        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            listBox1.SelectedIndex = listBox1.IndexFromPoint(e.X, e.Y);
             if (listBox1.SelectedIndex != -1)
            {
            if (e.Button == MouseButtons.Right)
                    cm.Show(this, new Point(e.X + 15, e.Y + 30));

            }
        }
    }
}
