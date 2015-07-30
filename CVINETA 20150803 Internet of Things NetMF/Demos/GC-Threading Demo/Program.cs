using System;
using System.Collections;
using System.Threading;
using Microsoft.SPOT;


namespace GarbageCollectionDemo
{
    public partial class Program
    {

        void ProgramStarted()
        {
            new GarbageCollectorDemo().Run();
            //new ThreadingDemo().Run();
            
        }
    }

}
