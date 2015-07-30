using System;
using System.Collections;
using System.Threading;
using Gadgeteer.SocketInterfaces;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using Gadgeteer.Modules.GHIElectronics;
using AnalogInput = Microsoft.SPOT.Hardware.AnalogInput;

namespace IODemo
{
    public partial class Program
    {
        private DigitalOutput ledGadgeteer;         //Variable for the Gadgeteer way.
        private InterruptInput buttonGadgeteer;
        
        private OutputPort ledSPOT;     //Variable for the SPOT way.
        private InterruptPort buttonSpot;

        void ProgramStarted()
        {

            InterruptWithGadgeteer();
            //InterruptWithSPOT();

            SetupFadingLight();

            SetupLightBar();

            Mainboard.SetDebugLED(true);
            Debug.Print("Running");
        }
        private void InterruptWithGadgeteer()
        {
            //Create the output for a LED that we are controlling with the switch.
            ledGadgeteer = breakout7.CreateDigitalOutput(GT.Socket.Pin.Eight, false); 

            
            buttonGadgeteer = breakout7.CreateInterruptInput(GT.Socket.Pin.Three, 
                GlitchFilterMode.On, ResistorMode.Disabled,
                //We want to know both when the buttonGadgeteer is pressed (rising 
                //egde), and when it is released (falling edge)
                InterruptMode.RisingAndFallingEdge);
            buttonGadgeteer.Interrupt += buttonGadgeteer_Interrupt;
        }
        void buttonGadgeteer_Interrupt(InterruptInput sender, bool value)
        {
            ledGadgeteer.Write(value);
            //Debug.Print("LED On: " + value);
        }

        private static void SetupFadingLight()
        {
            //Create the output...
            var pwm = new PWM(
                GT.Socket.GetSocket(7, true, null, null).PWM7, 1000, 0, false);
            //...and hand it off to someone else to do the logic. Moves the 
            //logic out of the Program.cs file/class.
            var fade = new LightFade(pwm);
        }

        private void SetupLightBar()
        {
            //Setup GHI Gadgeteer module
            maxO.Boards = 1;

            //Get our AnalogInput
            var speedInput = new AnalogInput(
                GT.Socket.GetSocket(14, true, null, null).AnalogInput3);

            var lb = new LightBar(maxO, speedInput);
        }

        

        

        

        void InterruptWithSPOT()
        {
            ledSPOT = new OutputPort(GT.Socket.GetSocket(7, true, null, null).CpuPins[8], false);

            var buttonSpot = new InterruptPort(GT.Socket.GetSocket(7, true, null, null).CpuPins[3], 
                true, Microsoft.SPOT.Hardware.Port.ResistorMode.Disabled,
                Microsoft.SPOT.Hardware.Port.InterruptMode.InterruptEdgeBoth);

            buttonSpot.OnInterrupt += buttonSPOT_Interrupt;
        }
        private void buttonSPOT_Interrupt(uint data1, uint data2, DateTime time)
        {
            var value = data2 == 1;
            ledSPOT.Write(value);
            //Debug.Print("LED On: " + value);
        }
        
    }
}
