using System;
using Gadgeteer.Modules.GHIElectronics;
using Microsoft.SPOT.Hardware;
using GT = Gadgeteer;
using Microsoft.SPOT;

namespace IODemo
{
    public class LightBar
    {
        private readonly MaxO mMaxO;
        private readonly AnalogInput mSpeedInput;
        private int pin = 0;
        private int incrementer = 2;
        private int oldSpeed;

        public LightBar(MaxO maxO, AnalogInput speedInput)
        {
            mMaxO = maxO;
            mSpeedInput = speedInput;

            GT.Timer timer = new GT.Timer(100);
            timer.Tick += ChangeLED;
            timer.Start();
        }

        private void ChangeLED(GT.Timer timer)
        {
            //Use module, set output
            mMaxO.SetPin(1, pin, false);
            pin += incrementer;
            mMaxO.SetPin(1, pin, true);

            //Change direction
            if (pin == 30 || pin == 0)
            {
                incrementer *= -1;
            }


            //Read the desired speed.
            int speedRaw = mSpeedInput.ReadRaw();   //Gets the raw integer from the ADC
            double speedDouble = mSpeedInput.Read();    //Gets the scaled version - from 0 to 1.

            //Convert the speed to what we want to use.
            int speedConverted = (int)(speedDouble*200);
            if (speedConverted==0)
            {
                speedConverted = 1;
            }

            //Only change speed if it changed. Avoids creating another TimeSpan struct.
            if (oldSpeed!=speedConverted)
            {
                oldSpeed = speedConverted;
                timer.Interval = new TimeSpan(0, 0, 0, 0, speedConverted);
                Debug.Print("speedRaw: " + speedRaw + " speedDouble: " + speedDouble + " speedConverted: " + speedConverted);

                //Question: Add the debug line above, then
                //watch the debug window. Why is the 
                //speed changing when we aren't touching the 
                //potentiometer? How do we deal with this?
            }
            
            
            
            
        }
    }
}
