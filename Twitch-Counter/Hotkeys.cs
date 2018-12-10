using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

//Low-Level Keyboard Hook in C#
//http://blogs.msdn.com/toub/archive/2006/05/03/589423.aspx?CommentPosted=true#commentmessage

//WindowsMessages (Enums)
//http://www.pinvoke.net/default.aspx/Enums/WindowsMessages.html

class Hotkeys
{
    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private static LowLevelKeyboardProc _proc = HookCallback;
    private static IntPtr _hookID = IntPtr.Zero;
    public static List<Twitch_Counter.Counter> counterList;
    public static int selected;
    public static int index { get; set; }
    public static List<int> binds = new List<int>();
    private static Twitch_Counter.OneCounter oc;
    private static Twitch_Counter.TwoCounters tc;
    private static Twitch_Counter.TwoCountersRatio tcr;
    private static Twitch_Counter.ThreeCounters ttc;
    public static void Start()
    {
        _hookID = SetHook(_proc);
        Application.Run();
        UnhookWindowsHookEx(_hookID);
    }

    private static IntPtr SetHook(LowLevelKeyboardProc proc)
    {
        using (Process curProcess = Process.GetCurrentProcess())
        using (ProcessModule curModule = curProcess.MainModule)
        {
            return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                GetModuleHandle(curModule.ModuleName), 0);
        }
    }

    private delegate IntPtr LowLevelKeyboardProc(
        int nCode, IntPtr wParam, IntPtr lParam);

    private static IntPtr HookCallback(
        int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
        {
            int vkCode = Marshal.ReadInt32(lParam);
            Keys pressed = (Keys)vkCode;
            if (binds.Contains(vkCode))
                selected = vkCode;
            if (vkCode == 107)
                Increase();
            else if (vkCode == 109)
                Decrease();
        }
        return CallNextHookEx(_hookID, nCode, wParam, lParam);
    }

    private static void Increase()
    {
        Twitch_Counter.Type t = (Twitch_Counter.Type)counterList[index].Type;
        string jsonTxt = File.ReadAllText("Counters.json");
        var obj = JsonConvert.DeserializeObject<dynamic>(jsonTxt);
        obj.Counters.RemoveAt(index);
        switch (t)
        {
            case Twitch_Counter.Type.OneCounter:
                {
                    oc = (Twitch_Counter.OneCounter)counterList[index];
                    oc.CounterOne++;
                    obj.Counters.Insert(index, JToken.Parse(JsonConvert.SerializeObject(oc, Formatting.Indented)));
                }
                break;
            case Twitch_Counter.Type.TwoCounters:
                {
                    tc = (Twitch_Counter.TwoCounters)counterList[index];
                    if (tc.CounterOneBind == selected)
                        tc.CounterOne++;
                    else
                        tc.CounterTwo++;
                    obj.Counters.Insert(index, JToken.Parse(JsonConvert.SerializeObject(tc, Formatting.Indented)));
                }
                break;
            case Twitch_Counter.Type.TwoCountersRatio:
                {
                    tcr = (Twitch_Counter.TwoCountersRatio)counterList[index];
                    if (tcr.CounterOneBind == selected)
                        tcr.CounterOne++;
                    else
                        tcr.CounterTwo++;
                    if (tcr.CounterTwo != 0)
                        tcr.CounterRatio = Math.Round((double)(tcr.CounterOne) / (double)(tcr.CounterTwo), 2);
                    else
                        tcr.CounterRatio = (Math.Round((double)(tcr.CounterOne), 2));
                    obj.Counters.Insert(index, JToken.Parse(JsonConvert.SerializeObject(tcr, Formatting.Indented)));

                }
                break;
            case Twitch_Counter.Type.ThreeCounters:
                {
                    ttc = (Twitch_Counter.ThreeCounters)counterList[index];
                    if (ttc.CounterOneBind == selected)
                        ttc.CounterOne++;
                    else if (ttc.CounterTwoBind == selected)
                        ttc.CounterTwo++;
                    else
                        ttc.CounterThree++;
                    obj.Counters.Insert(index, JToken.Parse(JsonConvert.SerializeObject(ttc, Formatting.Indented)));
                }
                break;
        }
        updateFile();
        File.WriteAllText("Counters.json", obj.ToString());
    }

    private static void Decrease()
    {
        Twitch_Counter.Type t = (Twitch_Counter.Type)counterList[index].Type;
        string jsonTxt = File.ReadAllText("Counters.json");
        var obj = JsonConvert.DeserializeObject<dynamic>(jsonTxt);
        obj.Counters.RemoveAt(index);
        switch (t)
        {
            case Twitch_Counter.Type.OneCounter:
                {
                    oc = (Twitch_Counter.OneCounter)counterList[index];
                    oc.CounterOne--;
                    obj.Counters.Insert(index, JToken.Parse(JsonConvert.SerializeObject(oc, Formatting.Indented)));
                }
                break;
            case Twitch_Counter.Type.TwoCounters:
                {
                    tc = (Twitch_Counter.TwoCounters)counterList[index];
                    if (tc.CounterOneBind == selected)
                        tc.CounterOne--;
                    else
                        tc.CounterTwo--;
                    obj.Counters.Insert(index, JToken.Parse(JsonConvert.SerializeObject(tc, Formatting.Indented)));
                }
                break;
            case Twitch_Counter.Type.TwoCountersRatio:
                {
                    tcr = (Twitch_Counter.TwoCountersRatio)counterList[index];
                    if (tcr.CounterOneBind == selected)
                        tcr.CounterOne--;
                    else
                        tcr.CounterTwo--;
                    if (tcr.CounterTwo != 0)
                        tcr.CounterRatio = Math.Round((double)(tcr.CounterOne) / (double)(tcr.CounterTwo), 2);
                    else
                        tcr.CounterRatio = (Math.Round((double)(tcr.CounterOne), 2));
                    obj.Counters.Insert(index, JToken.Parse(JsonConvert.SerializeObject(tcr, Formatting.Indented)));

                }
                break;
            case Twitch_Counter.Type.ThreeCounters:
                {
                    ttc = (Twitch_Counter.ThreeCounters)counterList[index];
                    if (ttc.CounterOneBind == selected)
                        ttc.CounterOne--;
                    else if (ttc.CounterTwoBind == selected)
                        ttc.CounterTwo--;
                    else
                        ttc.CounterThree--;
                    obj.Counters.Insert(index, JToken.Parse(JsonConvert.SerializeObject(ttc, Formatting.Indented)));
                }
                break;
        }
        updateFile();
        File.WriteAllText("Counters.json", obj.ToString());
    }

    private static void updateFile()
    {
        Twitch_Counter.Type t = (Twitch_Counter.Type)counterList[index].Type;
        string text = "";
        string name = "";
        switch (t)
        {
            case Twitch_Counter.Type.OneCounter:
                {
                    oc = (Twitch_Counter.OneCounter)counterList[index];
                    text = oc.Format;
                    text = text.Replace("$c1", oc.CounterOne.ToString());
                    name = oc.Name;
                }
                break;
            case Twitch_Counter.Type.TwoCounters:
                {
                    tc = (Twitch_Counter.TwoCounters)counterList[index];
                    text = tc.Format;
                    text = text.Replace("$c1", tc.CounterOne.ToString());
                    text = text.Replace("$c2", tc.CounterTwo.ToString());
                    name = tc.Name;
                }
                break;
            case Twitch_Counter.Type.TwoCountersRatio:
                {
                    tcr = (Twitch_Counter.TwoCountersRatio)counterList[index];
                    text = tcr.Format;
                    text = text.Replace("$c1", tcr.CounterOne.ToString());
                    text = text.Replace("$c2", tcr.CounterTwo.ToString());
                    text = text.Replace("$ratio", tcr.CounterRatio.ToString());
                    name = tcr.Name;
                }
                break;
            case Twitch_Counter.Type.ThreeCounters:
                {
                    ttc = (Twitch_Counter.ThreeCounters)counterList[index];
                    text = ttc.Format;
                    text = text.Replace("$c1", ttc.CounterOne.ToString());
                    text = text.Replace("$c2", ttc.CounterTwo.ToString());
                    text = text.Replace("$c3", ttc.CounterThree.ToString());
                    name = ttc.Name;

                }
                break;
        }
        File.WriteAllText("Text Files/" + name + ".txt", text);
    }


    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook,
        LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
        IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);
}