using System;
using System.Collections;
using GT = Gadgeteer;
using Microsoft.SPOT;

namespace GarbageCollectionDemo
{
    public class GarbageCollectorDemo
    {
        private static int NumberOfMs = 5000;
        private static int NumberOfMsMinus1 = NumberOfMs - 1;
        long start;
        private long[] times = new long[NumberOfMs];
        GT.Timer gcTimer;

        public void Run()
        {

            Debug.Print("Starting");
            HogSomeMemory();
            Debug.Print("Memory hogged!");

            //Setup the GC to run hanlfway through our sample collection
            int runGCAt = NumberOfMs / 2;
            gcTimer = new GT.Timer(runGCAt);
            gcTimer.Tick += RunGC;
            gcTimer.Start();
            Debug.Print("GC set to run at " + runGCAt);

            //Try to collect every millisecond.
            GT.Timer timer = new GT.Timer(1); 
            timer.Tick += RecordTime;
            start = DateTime.Now.Ticks;
            Debug.Print("Recording times");
            timer.Start();


        }

        private void RunGC(GT.Timer timer)
        {
            Debug.GC(true);
            gcTimer.Stop();
        }

        private void HogSomeMemory()
        {
            var l = new ArrayList();
            for (int j = 0; j < 8000; j++)
            {
                l.Add(new SuperFatBlobularDude(DateTime.Now.Ticks));
            }
        }

        private void RecordTime(GT.Timer timer)
        {

            long ms = (DateTime.Now.Ticks - start) / 10000;
            if (ms >= NumberOfMsMinus1)
            {
                timer.Stop();
                PrintResults();
                return;
            }

            times[ms] = ms;
        }

        private void PrintResults()
        {
            if (!printed)
            {
                printed = true;
                for (int i = 0; i < NumberOfMsMinus1; i++)
                {
                    Debug.Print(i.ToString() + ": " + times[i]);
                }    
            }
            
        }

        private bool printed = false;
    }

    public class SuperFatBlobularDude
    {
        public string Junk;
        public string Junk2;
        public string Junk3;
        public string Junk4;
        public string Junk5;
        public string Junk6;
        public int a;
        public int b;
        public int c;
        public int d;
        public int e;
        public int f;
        public int g;
        public long y;
        public long now;
        public long later;

        public SuperFatBlobularDude(long ticks)
        {
            now = ticks;
            later = now + 1;
            Junk = ticks.ToString();
            y = (long) later*3424324;
            a = (int) now/394839;
            b = a + 1;
            c = b + 1;
            d = c + 1;
            e = d + 1;
            f = e + 1;
            g = f + 1;
            Junk2 = Junk + "2";
            Junk3 = Junk + "3";
            Junk4 = Junk + "4";
            Junk5 = Junk + "5";
            Junk6 = Junk + "6";
        }
    }
}
