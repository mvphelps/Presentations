using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;

namespace Blinky2
{
    public partial class Program
    {
        //This method is invoked for setup. Create the objects you need, then return.
        //Never put an infinite loop here, or the event dispatcher won't start.
        void ProgramStarted()
        {
            GT.Timer timer = new GT.Timer(250); //Our delay is 1/4 second
            timer.Tick += BlinkLED;
            timer.Start();
        }

        //This method is invoked by the timer at each interval.
        void BlinkLED(GT.Timer timer)
        {
            Mainboard.SetDebugLED(mOn);
            mOn = !mOn;
        }

        private bool mOn = true;
        
    }
}
