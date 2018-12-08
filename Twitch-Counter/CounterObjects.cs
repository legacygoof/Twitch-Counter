using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch_Counter
{
    public interface Counter
    {
        string Name { get; set; }
        string Format { get; set; }
        int Type { get; set; }
    }
    public enum Type
    {
        OneCounter,
        TwoCounters,
        TwoCountersRatio,
        ThreeCounters
    }
    class OneCounter : Counter
    {
        public string Name { get; set; }
        public int CounterOne { get; set; }
        public int CounterOneBind { get; set; }
        public string Format { get; set; }
        public int Type { get; set; }
    }

    class TwoCounters : Counter
    {
        public string Name { get; set; }
        public int CounterOne { get; set; }
        public int CounterTwo { get; set; }
        public int CounterOneBind { get; set; }
        public int CounterTwoBind { get; set; }
        public string Format { get; set; }
        public int Type { get; set; }
    }

    class TwoCountersRatio : Counter
    {
        public string Name { get; set; }
        public int CounterOne { get; set; }
        public int CounterTwo { get; set; }
        public double CounterRatio { get; set; }
        public int CounterOneBind { get; set; }
        public int CounterTwoBind { get; set; }
        public string Format { get; set; }
        public int Type { get; set; }
    }

    class ThreeCounters : Counter
    {
        public string Name { get; set; }
        public int CounterOne { get; set; }
        public int CounterTwo { get; set; }
        public int CounterThree { get; set; }
        public int CounterOneBind { get; set; }
        public int CounterTwoBind { get; set; }
        public int CounterThreeBind { get; set; }
        public string Format { get; set; }
        public int Type { get; set; }
    }
}
