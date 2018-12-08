using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch_Counter
{
    public enum Type
    {
        OneCounter,
        TwoCounters,
        TwoCountersRatio,
        ThreeCounters
    }
    class OneCounter
    {
        public string Name { get; set; }
        public int Counter { get; set; }
        public string Format { get; set; }
        public int Type { get; set; }
    }

    class TwoCounters
    {
        public string Name { get; set; }
        public int CounterOne { get; set; }
        public int CounterTwo { get; set; }
        public string Format { get; set; }
        public int Type { get; set; }
    }

    class TwoCountersRatio
    {
        public string Name { get; set; }
        public int CounterOne { get; set; }
        public int CounterTwo { get; set; }
        public double CounterRatio { get; set; }
        public string Format { get; set; }
        public int Type { get; set; }
    }

    class ThreeCounters
    {
        public string Name { get; set; }
        public int CounterOne { get; set; }
        public int CounterTwo { get; set; }
        public int CounterThree { get; set; }
        public string Format { get; set; }
        public int Type { get; set; }
    }
}
