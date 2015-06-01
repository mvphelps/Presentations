using System;
using Microsoft.SPOT;

namespace Tests
{
    public class Clock
    {
        private long Base;
        public Clock()
        {
            Base = DateTime.Now.Ticks;
        }

        public Clock(long @base)
        {
            Base = @base;
        }

        public long Tick()
        {
            return ++Base;
        }
        //public long Plus(long ticks)
        //{
        //    return Base + ticks;
        //}
        public static long operator + (Clock value, long ticksToAdd)
        {
            return value.Base + ticksToAdd;
        }
    }
}
