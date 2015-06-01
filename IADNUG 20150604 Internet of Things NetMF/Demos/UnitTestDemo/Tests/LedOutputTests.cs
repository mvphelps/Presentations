using System;
using DemoAppLibrary;
using MFUnit;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace Tests
{
    public class LedOutputTests
    {
        public void TurnItOnAndOff()
        {
            var o = new LedOutput(new OutputPort(Cpu.Pin.GPIO_Pin7, false));
            Assert.AreEqual(false, o.IsOn());

            o.TurnOn();
            Assert.AreEqual(true, o.IsOn());

            o.TurnOff();
            Assert.AreEqual(false, o.IsOn());
        }
        public void Toggle()
        {
            //Why does this throw?
            var o = new LedOutput(new OutputPort(Cpu.Pin.GPIO_Pin7, false));
            Assert.AreEqual(false, o.IsOn());

            o.Toggle();
            Assert.AreEqual(true, o.IsOn());

            o.Toggle();
            Assert.AreEqual(false, o.IsOn());

            o.Toggle();
            Assert.AreEqual(true, o.IsOn());

        }

        //public void SetBrightness()
        //{
        //    //What if we want to do something like this? The LedOutput 
        //    //will Read() our input. But how do we supply a value?
        //    //And how do we assert we got the result we wanted?


        //    var brightness = new AnalogInput(Cpu.AnalogChannel.ANALOG_0);
        //    brightness.Read() = 0.1;    //This doesn't exist!
        //    var light = new OutputPort(Cpu.Pin.GPIO_Pin8, false);
        //    using (var o = new LedOutput(light, brightness))
        //    {
        //        Assert.AreEqual(false, o.IsOn());

        //        o.Toggle();
        //        Assert.AreEqual(true, o.IsOn());
        //        Assert.AreEqual(0.01, o.DutyCyle);
        //    }
        //}
    }
}
