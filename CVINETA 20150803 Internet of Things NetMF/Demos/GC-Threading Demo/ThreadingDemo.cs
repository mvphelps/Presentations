using System;
using System.Threading;
using Microsoft.SPOT;
using GT = Gadgeteer;

namespace GarbageCollectionDemo
{
    public class ThreadingDemo
    {
        private static int NumberOfMs = 5000;
        private static int NumberOfMsMinus1 = NumberOfMs - 1;
        long start;
        private long[] times = new long[NumberOfMs];
        Thread waster;
        private long WasteCounter=0;
        public void Run()
        {

            Debug.Print("Starting");

            //Fire up a thread that uses lots of CPU.
            waster = new Thread(WasteTime);
            waster.Start();

            //Try to collect every millisecond.
            GT.Timer timer = new GT.Timer(1); 
            timer.Tick += RecordTime;
            start = DateTime.Now.Ticks;
            Debug.Print("Recording times");
            timer.Start();


        }

        private void WasteTime()
        {
            while (true)
            {
                WasteCounter++;
                //Sleeping yields control back to the thread scheduler. If 
                //you are using multiple threads, and you hit the end of a
                //work for loop in the thread, sleep to yield the CPU.
                //Compare with and without this line to see the difference.
                Thread.Sleep(0);                
            }
        }

        private void RecordTime(GT.Timer timer)
        {

            long ms = (DateTime.Now.Ticks - start) / 10000;
            if (ms >= NumberOfMsMinus1)
            {
                timer.Stop();
                if (waster!=null)
                {
                    waster.Suspend();    
                }
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
                Debug.Print("WasteCounter: " + WasteCounter);
            }

        }
        private bool printed = false;


        
    }
}
