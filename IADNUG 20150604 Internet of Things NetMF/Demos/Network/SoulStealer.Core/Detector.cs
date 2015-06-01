using System;
using System.Threading;
using MFMock;
using Microsoft.SPOT;

namespace SoulStealer.Core
{
    public class Detector
    {
        public int TriggerMillis { get; private set; }

        public delegate void DetectedEvent(bool comingIn);
        public event DetectedEvent Detected;

        private readonly IAnalogInput mLightSense1;
        private readonly IAnalogInput mLightSense2;
        private int mThreshHold1 = 0;
        private int mThreshHold2 = 0;
        private bool mRunning;
        private readonly long mTriggerTime;
        private readonly int mDetectionLevel;
        private Thread mWorker;
            
        public Detector(IAnalogInput lightSense1, IAnalogInput lightSense2, int detectionLevel, int triggerMillis)
        {
            TriggerMillis = triggerMillis;
            mLightSense1 = lightSense1;
            mLightSense2 = lightSense2;
            mTriggerTime = TimeSpan.TicksPerMillisecond * triggerMillis;
            mDetectionLevel = detectionLevel;
        }
        public void Start()
        {
            Calibrate();

            if (Detected == null)
            {
                throw new InvalidOperationException("Must connect Detected event before invoking Start().");
            }
            Debug.Print("Sensor 1=" + mThreshHold1 + "    Sensor 2=" + mThreshHold2);

            mRunning = true;
            mWorker = new Thread(DetectThread);
            mWorker.Start();
        }
        public void Stop()
        {
            mRunning = false;
        }
        public void Calibrate()
        {
            mThreshHold1 = Calibrate(mLightSense1);
            mThreshHold2 = Calibrate(mLightSense2);
        }
        public int Calibrate(IAnalogInput input)
        {
            int value = 0;
            const int CalibrationReads = 10;
            for (int i = 0; i < CalibrationReads; i++)
            {
                value += input.ReadRaw();
            }
            return value / CalibrationReads;
        }

        private long mTriggered1;
        private long mTriggered2;
        
        private void DetectThread()
        {
            ResetTriggerTimes();
            while (mRunning)
            {
                Detect(DateTime.Now.Ticks);
                Thread.Sleep(0);
            }
        }

        private void ResetTriggerTimes()
        {   //Not using 0, as if we are at the very beginning of our 
            //time (and have no backup for the system clock) then
            //the times will be less that the trigger time, and will
            //trigger false detections.
            mTriggered1 = -mTriggerTime;
            mTriggered2 = -mTriggerTime;
        }

        public void Detect(long now)
        {
            int one = mLightSense1.ReadRaw();
            mTriggered1 = CheckForTrigger(one, mThreshHold1, mTriggered1, now);

            int two = mLightSense2.ReadRaw();
            mTriggered2 = CheckForTrigger(two, mThreshHold2, mTriggered2, now);

            if (System.Math.Abs(mTriggered1 - mTriggered2) < mTriggerTime && mTriggered1!=mTriggered2)
            {
                bool comingIn = mTriggered1 < mTriggered2;
                Debug.Print("Now=" + now + "    1=" + mThreshHold1 + ", " + one + "    2=" + mThreshHold2 + ", " + two + (comingIn ? " Coming" : " Going"));
                Detected(comingIn);
                ResetTriggerTimes();
            }
        }

        private long CheckForTrigger(int sensorValue, int threshHold, long oldEvent, long now)
        {
            
            if (System.Math.Abs(threshHold - sensorValue) > mDetectionLevel)
            {
                if (oldEvent+mTriggerTime<now)
                {
                    return now;
                }
                return oldEvent;
            }
            return oldEvent;
        }
    }

}
