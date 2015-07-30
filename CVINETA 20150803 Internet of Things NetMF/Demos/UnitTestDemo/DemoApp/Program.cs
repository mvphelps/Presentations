using System;
using System.Collections;
using System.Threading;
using DemoAppLibrary;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using GT = Gadgeteer;

namespace DemoApp
{
    public partial class Program
    {
        LedOutput mLight;
            
        void ProgramStarted()
        {
            mLight = new LedOutput(new OutputPort(Cpu.Pin.GPIO_Pin7, false));
            var t = new GT.Timer(1000);
            t.Tick += ChangeLight;
            t.Start();
        }

        private void ChangeLight(GT.Timer timer)
        {
            mLight.Toggle();
        }
    }
}
