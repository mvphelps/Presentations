using System;
using MFMock;

namespace DemoAppLibrary
{
    public class LedOutput : IDisposable
    {
        private IPWM mLight;
        private readonly IAnalogInput mBrightness;
        private bool mIsOn;

        public LedOutput(IPWM light, IAnalogInput brightness)
        {
            mLight = light;
            mBrightness = brightness;
        }

        public void TurnOn()
        {
            mLight.DutyCycle = mBrightness.Read();
            mLight.Start();
            mIsOn = true;
        }
        public void TurnOff()
        {
            mLight.Stop();
            mIsOn = false;
        }

        public bool IsOn()
        {
            return mIsOn;
        }

        public void Toggle()
        {
            if (mIsOn)
            {
                TurnOff();
            }
            else
            {
                TurnOn();
            }
        }

        public void ToggleTimes(int times)
        {
            for (int i = 0; i < times; i++)
            {
                Toggle();
            }
        }

        public void Dispose()
        {
            if (mLight != null) mLight.Dispose();
            if (mBrightness != null) mBrightness.Dispose();
        }
    }
}
