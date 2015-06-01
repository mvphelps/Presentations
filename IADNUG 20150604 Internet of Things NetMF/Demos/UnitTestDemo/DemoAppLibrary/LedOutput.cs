using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace DemoAppLibrary
{
    public class LedOutput
    {
        private OutputPort mPort;
        private bool mIsOn;

        public LedOutput(OutputPort port)
        {
            mPort = port;
            mIsOn = port.InitialState;
        }

        public void TurnOn()
        {
            mPort.Write(true);
            mIsOn = true;
        }
        public void TurnOff()
        {
            mPort.Write(false);
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

    }
}
