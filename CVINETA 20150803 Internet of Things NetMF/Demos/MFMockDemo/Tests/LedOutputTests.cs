using System;
using DemoAppLibrary;
using MFMock;
using MFMockTesters;
using MFUnit;

namespace Tests
{
    public class LedOutputTests
    {
        public void TurnItOnAndOff()
        {
            var brightness = new AnalogInputTester(new[] { 0.1 });
            var light = new PWMTester();
            using (var o = new LedOutput(light, brightness))
            {
                Assert.AreEqual(false, o.IsOn());

                o.TurnOn();
                Assert.AreEqual(true, o.IsOn());

                o.TurnOff();
                Assert.AreEqual(false, o.IsOn());    
            }
            
        }
        public void Toggle()
        {
            var brightness = new AnalogInputTester(new[] { 0.1, 0.2 });
            var light = new PWMTester();
            using (var o = new LedOutput(light, brightness))
            {
                Assert.AreEqual(false, o.IsOn());

                o.Toggle();
                Assert.AreEqual(true, o.IsOn());

                o.Toggle();
                Assert.AreEqual(false, o.IsOn());

                o.Toggle();
                Assert.AreEqual(true, o.IsOn());
            }
        }

        public void SetBrightness_PerSampleVerification()
        {
            var brightness = new AnalogInputTester(new[] { 0.1, 0.2 });
            var light = new PWMTester();
            using (var o = new LedOutput(light, brightness))
            {
                o.Toggle();
                Assert.AreEqual(true, light.IsStarted);
                Assert.AreEqual(0.1, light.DutyCycle);

                o.Toggle();
                Assert.AreEqual(true, light.IsStopped);
                Assert.AreEqual(0.1, light.DutyCycle);

                o.Toggle();
                Assert.AreEqual(true, light.IsStarted);
                Assert.AreEqual(0.2, light.DutyCycle);
            }
        }
        public void SetBrightness_JournalVerification()
        {   //Journal verification is useful if you have 
            //a method that loops or makes multiple 
            //output changes at once. 
            var brightness = new AnalogInputTester(new[] { 0.1, 0.2 });
            var light = new PWMTester();
            using (var o = new LedOutput(light, brightness))
            {
                o.ToggleTimes(3);

                Assert.AreEqual(0.1, light.ChangeLog[0].DutyCycle);
                Assert.AreEqual(true, light.ChangeLog[1].Started);
                Assert.AreEqual(true, light.ChangeLog[2].Stopped);
                Assert.AreEqual(0.2, light.ChangeLog[3].DutyCycle);
                Assert.AreEqual(true, light.ChangeLog[4].Started);
            }
        }
    }
}
