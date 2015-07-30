using System;
using Gadgeteer;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace IODemo
{
    public class LightFade
    {
        private readonly PWM mPwm;
        private Timer t;
        public LightFade(PWM pwm)
        {
            mPwm = pwm;
            pwm.Start();
            t=new Timer(10);
            t.Tick += FadeLight;
            t.Start();
        }

        private double increment = 0.01;
        private void FadeLight(Timer timer)
        {
            double newDutyCycle = mPwm.DutyCycle + increment;
            if (newDutyCycle >= 1)
            {
                newDutyCycle = 1;
                increment *= -1;
            }
            else if (newDutyCycle <=0)
            {
                newDutyCycle = 0;
                increment *= -1;
            }
            mPwm.DutyCycle = newDutyCycle;
            
        }
    }
}
