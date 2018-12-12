using System;
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
using System.Threading;
using System.Security.Principal;
using System.Net;

namespace Twitch_Counter
{
    public partial class Main : Form
    {
        string version = "Version 1.0";
        public List<Counter> counterList = new List<Counter>();
        string jsonTxt;
        ContextMenuStrip cm = new ContextMenuStrip();
        Edit_From editForm;
        Add_Counter addForm = new Add_Counter();
        Thread hotkeys;
        int selected = -1;
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        private static string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Twitch Counter";
        private static string jsonFile = " { \"Counters\": [] }";
        public string jsonFilePath = path + "\\Counters.json";
        public Main()
        {
            InitializeComponent();
        }
        public static bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            
            Directory.CreateDirectory(path);
            Directory.CreateDirectory(path+"\\Text Files");
            if(!File.Exists(path+"\\Counters.json"))
            {
                using (FileStream fs = File.Create(path + "\\Counters.json"))
                {
                    Byte[] info = new UTF8Encoding(true).GetBytes(jsonFile);
                    fs.Write(info, 0, info.Length);
                }
            }
            if (!IsAdministrator())
            {               
                //MessageBox.Show("Please Run Program As Administrator");
                //this.Close();
            }
            this.Icon = Twitch_Counter.Properties.Resources.twitch_LuI_icon;
            UpdateJson();
            cm.ItemClicked += new ToolStripItemClickedEventHandler(ContextMenu_ItemClicked);
            cm.Items.Add("Edit");
            cm.Items.Add("Reset Values");
            cm.Items.Add("Remove");
            hotkeys = new Thread(Hotkeys.Start);
            hotkeys.IsBackground = true;
            hotkeys.ApartmentState = ApartmentState.STA;
            hotkeys.Start();

            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = 2000;
            timer.Enabled = true;
            timer.Start();
            checkUpdate();
        }

        private void checkUpdate()
        {
            string s = "";
            using (WebClient client = new WebClient())
            {
                s = client.DownloadString("http://pastebin.com/raw/PFVxLSej");
            }
            if (!s.Equals(version))
            {
                DialogResult dialogResult = MessageBox.Show("Theres a newer version of Twitch-Counter do you want to update?", "Update", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start("http://github.com/legacygoof/Twitch-Counter");
                }
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            UpdateJson();
            if(selected > -1 && listBox1.Items.Count > selected)
                listBox1.SelectedIndex = selected;
            updatePreviewText();
        }

        private void ContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                switch (e.ClickedItem.ToString())
                {
                    case "Edit": setupEditForm(listBox1.SelectedIndex); break;
                    case "Reset Values": resetValues(listBox1.SelectedIndex); break;
                    case "Remove": removeJsonItem(listBox1.SelectedIndex); break;
                }
            }
        }
        
        private void resetValues(int selectedIndex)
        {
            jsonTxt = File.ReadAllText(jsonFilePath);
            OneCounter oc;
            TwoCounters tc;
            TwoCountersRatio tcr;
            ThreeCounters ttc;
            var obj = JsonConvert.DeserializeObject<dynamic>(jsonTxt);
            obj.Counters.RemoveAt(selectedIndex);
            switch ((Type)counterList[selectedIndex].Type)
            {
                case Type.OneCounter: oc = (OneCounter)counterList[selectedIndex]; oc.CounterOne = 0; obj.Counters.Insert(selectedIndex, JToken.Parse(JsonConvert.SerializeObject(oc, Formatting.Indented))); break;
                case Type.TwoCounters: tc = (TwoCounters)counterList[selectedIndex]; tc.CounterOne = 0; tc.CounterTwo = 0; obj.Counters.Insert(selectedIndex, JToken.Parse(JsonConvert.SerializeObject(tc, Formatting.Indented))); break;
                case Type.TwoCountersRatio: tcr = (TwoCountersRatio)counterList[selectedIndex]; tcr.CounterOne = 0; tcr.CounterTwo = 0; tcr.CounterRatio = 0; obj.Counters.Insert(selectedIndex, JToken.Parse(JsonConvert.SerializeObject(tcr, Formatting.Indented))); break;
                case Type.ThreeCounters: ttc = (ThreeCounters)counterList[selectedIndex]; ttc.CounterOne = 0; ttc.CounterTwo = 0; ttc.CounterThree = 0; obj.Counters.Insert(selectedIndex, JToken.Parse(JsonConvert.SerializeObject(ttc, Formatting.Indented))); break;
            }
            File.WriteAllText(jsonFilePath, obj.ToString());
            UpdateJson();
        }

        private void setupEditForm(int index)
        {
            editForm = new Edit_From(counterList, index);
            editForm.FormClosed += EditForm_FormClosed;
            editForm.Show();
        }

        private void EditForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //updateText(counterList[listBox1.SelectedIndex].format);
            UpdateJson();
        }

        private void updatePreviewText()
        {
            if (listBox1.Items.Count == 0)
                richTextBox1.Text = "";
            if (listBox1.SelectedIndex != -1)
            {
                Type t = (Type)counterList[listBox1.SelectedIndex].Type;
                string s = counterList[listBox1.SelectedIndex].Format;
                switch (t)
                {
                    case Type.OneCounter:
                        {
                            OneCounter oc = (OneCounter)counterList[listBox1.SelectedIndex];
                            s = s.Replace("$c1", oc.CounterOne.ToString());
                        }
                        break;
                    case Type.TwoCounters:
                        {
                            TwoCounters tc = (TwoCounters)counterList[listBox1.SelectedIndex];
                            s = s.Replace("$c1", tc.CounterOne.ToString());
                            s = s.Replace("$c2", tc.CounterTwo.ToString());
                        }
                        break;
                    case Type.TwoCountersRatio:
                        {
                            TwoCountersRatio tcr = (TwoCountersRatio)counterList[listBox1.SelectedIndex];
                            s = s.Replace("$c1", tcr.CounterOne.ToString());
                            s = s.Replace("$c2", tcr.CounterTwo.ToString());
                            if (tcr.CounterTwo != 0)
                                tcr.CounterRatio = Math.Round((double)(tcr.CounterOne) / (double)(tcr.CounterTwo), 2);
                            else
                                tcr.CounterRatio = Math.Round((double)(tcr.CounterOne), 2);
                            s = s.Replace("$ratio", tcr.CounterRatio.ToString());
                        }
                        break;
                    case Type.ThreeCounters:
                        {
                            ThreeCounters ttc = (ThreeCounters)counterList[listBox1.SelectedIndex];
                            s = s.Replace("$c1", ttc.CounterOne.ToString());
                            s = s.Replace("$c2", ttc.CounterTwo.ToString());
                            s = s.Replace("$c3", ttc.CounterThree.ToString());
                        }
                        break;
                }
                richTextBox1.Text = s;
            }
        }

        private void UpdateList()
        {
            listBox1.Items.Clear();
            foreach (Counter c in counterList)
                listBox1.Items.Add(c.Name);
        }

        public static void updateJson()
        {
            
        }

        private void UpdateJson()
        {
            jsonTxt = File.ReadAllText(jsonFilePath);
            //MessageBox.Show(jsonTxt);
            counterList.Clear();
            try
            {
                var obj = JsonConvert.DeserializeObject<dynamic>(jsonTxt);
                foreach (var item in obj.Counters)
                {
                    if (item.Type.ToString() == "0")
                    {
                        counterList.Add(new OneCounter { CounterOne = Int16.Parse(item.CounterOne.ToString()), Format = item.Format.ToString(), Name = item.Name, Type = Int16.Parse(item.Type.ToString()), CounterOneBind = Int16.Parse(item.CounterOneBind.ToString()) }); Hotkeys.binds.Add(Int16.Parse(item.CounterOneBind.ToString()));
                    }
                    else if(item.Type.ToString() == "1")
                    {
                        counterList.Add(new TwoCounters { CounterOne = Int16.Parse(item.CounterOne.ToString()), CounterTwo = Int16.Parse(item.CounterTwo.ToString()), Format = item.Format.ToString(), Name = item.Name, Type = Int16.Parse(item.Type.ToString()), CounterOneBind = Int16.Parse(item.CounterOneBind.ToString()), CounterTwoBind = Int16.Parse(item.CounterTwoBind.ToString()) }); Hotkeys.binds.Add(Int16.Parse(item.CounterOneBind.ToString())); Hotkeys.binds.Add(Int16.Parse(item.CounterTwoBind.ToString()));
                    }
                    else if (item.Type.ToString() == "2")
                    {
                        counterList.Add(new TwoCountersRatio { CounterOne = Int16.Parse(item.CounterOne.ToString()), CounterTwo = Int16.Parse(item.CounterTwo.ToString()), CounterRatio = Double.Parse(item.CounterRatio.ToString()), Format = item.Format.ToString(), Name = item.Name, Type = Int16.Parse(item.Type.ToString()), CounterOneBind = Int16.Parse(item.CounterOneBind.ToString()), CounterTwoBind = Int16.Parse(item.CounterTwoBind.ToString()) }); Hotkeys.binds.Add(Int16.Parse(item.CounterOneBind.ToString())); Hotkeys.binds.Add(Int16.Parse(item.CounterTwoBind.ToString()));
                    }
                    else if (item.Type.ToString() == "3")
                    {
                        counterList.Add(new ThreeCounters { CounterOne = Int16.Parse(item.CounterOne.ToString()), CounterTwo = Int16.Parse(item.CounterTwo.ToString()), CounterThree = Int16.Parse(item.CounterThree.ToString()), Format = item.Format.ToString(), Name = item.Name, Type = Int16.Parse(item.Type.ToString()), CounterOneBind = Int16.Parse(item.CounterOneBind.ToString()), CounterTwoBind = Int16.Parse(item.CounterTwoBind.ToString()), CounterThreeBind = Int16.Parse(item.CounterThreeBind.ToString()) }); Hotkeys.binds.Add(Int16.Parse(item.CounterOneBind.ToString())); Hotkeys.binds.Add(Int16.Parse(item.CounterTwoBind.ToString())); Hotkeys.binds.Add(Int16.Parse(item.CounterThreeBind.ToString()));
                    }
                }
            }
            catch(JsonReaderException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            Hotkeys.counterList = counterList;
            UpdateList();

        }

        private void removeJsonItem(int i)
        {
            jsonTxt = File.ReadAllText(jsonFilePath);
            var jObject = JsonConvert.DeserializeObject<dynamic>(jsonTxt);
            File.Delete(path + "\\Text Files/" + jObject.Counters[i].Name + ".txt");
            jObject.Counters.RemoveAt(i);
            File.WriteAllText(jsonFilePath, jObject.ToString());
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

        private void button1_Click(object sender, EventArgs e)
        {
            addForm = new Add_Counter();
            addForm.Show();
            addForm.FormClosed += AddForm_FormClosed;
        }

        private void AddForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            UpdateJson();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Hotkeys.index = listBox1.SelectedIndex;
            selected = listBox1.SelectedIndex;
            updatePreviewText();
        }
        //support
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://github.com/legacygoof/Twitch-Counter");
        }
        //donate
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://paypal.me/GoofSta");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://github.com/legacygoof/Twitch-Counter");
        }
    }
}
