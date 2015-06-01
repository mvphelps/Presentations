using System;
using MFMock;
using MFMockTesters;
using MFUnit;
using Microsoft.SPOT;
using SoulStealer.Core;

namespace Tests
{
    public class DetectorTests
    {
        public void ShouldCalibrate()
        {
            var t = new AnalogInputTester(new[] { 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000 });   //Calibrates to 550

            var d = new Detector(t, null, 10, 10);
            int calibrateResult = d.Calibrate(t);

            Assert.AreEqual(550, calibrateResult);
        }

        public void ShouldDetect()
        {

            var t1 = new AnalogInputTester(new[] { 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000 });    //Calibrates to 550
            var t2 = new AnalogInputTester(new[] { 0, 100, 200, 300, 400, 500, 600, 700, 800, 900 });   //Calibrates to 450

            var d = new Detector(t1, t2, 50, 10);
            d.Calibrate();
            d.Detected += DetectHandler;

            //Coming
            DetectedFired = false;
            t1.RawSamples = new[] {520, 520, 490, 491};
            t2.RawSamples = new[] {420, 420, 420, 390 };
            var clock = new Clock();

            d.Detect(clock.Tick()); //520 & 420. No change.
            Assert.IsFalse(DetectedFired);
            d.Detect(clock.Tick()); //520 & 420. Still no change.
            Assert.IsFalse(DetectedFired);
            d.Detect(clock.Tick()); //490 & 420. Sensor 1 will be triggered.
            Assert.IsFalse(DetectedFired);
            d.Detect(clock.Tick()); //491 & 390. Sensor 2 triggered and within time. 
            Assert.IsTrue(DetectedFired);
            Assert.IsTrue(DetectedComingIn);

            //Going
            DetectedFired = false;
            t1.RawSamples = new[] { 520, 520, 520, 490 };
            t2.RawSamples = new[] { 420, 420, 390, 389 };

            d.Detect(clock.Tick()); 
            Assert.IsFalse(DetectedFired);
            d.Detect(clock.Tick()); 
            Assert.IsFalse(DetectedFired);
            d.Detect(clock.Tick()); 
            Assert.IsFalse(DetectedFired);
            d.Detect(clock.Tick()); 
            Assert.IsTrue(DetectedFired);
            Assert.IsFalse(DetectedComingIn);

            //Only fire once for each detection. 
            DetectedFired = false;
            t1.RawSamples = new[] { 490, 490, 490, 520 };
            t2.RawSamples = new[] { 389, 389, 420, 421 };
            d.Detect(clock.Tick());
            Assert.IsFalse(DetectedFired);
            d.Detect(clock.Tick());
            Assert.IsFalse(DetectedFired);
            d.Detect(clock.Tick());
            Assert.IsFalse(DetectedFired);
            d.Detect(clock.Tick());
            Assert.IsFalse(DetectedFired);

        }

        private bool DetectedFired = false;
        private bool DetectedComingIn = false;
        private void DetectHandler(bool comingin)
        {
            DetectedFired = true;
            DetectedComingIn = comingin;
        }
    }
}
