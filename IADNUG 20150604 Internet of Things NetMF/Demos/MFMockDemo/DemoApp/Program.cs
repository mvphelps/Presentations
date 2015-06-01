using System;
using System.Threading;
using DemoAppLibrary;
using MFMock;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace DemoApp
{
    public class Program
    {
        private static LedOutput msLight;
        private static Timer msTimer;
        public static void Main()
        {
            var light = new PWM(Cpu.PWMChannel.PWM_0, 700, 0, false).Wrap();
            var potentiometer = new AnalogInput(Cpu.AnalogChannel.ANALOG_0).Wrap();
            msLight = new LedOutput(light, potentiometer);
            
            msTimer = new Timer(Toggle, null, TimeSpan.Zero, new TimeSpan(0,0,0,1));
        }

        public static void Toggle(object state)
        {
            msLight.Toggle();
        }
    }
}
