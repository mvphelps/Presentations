﻿using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;

namespace Music
{
    public partial class Program
    {
        void ProgramStarted()
        {
            var rtttl = new RtttlMelody("Last Demo:d=4,o=6,b=101:g5,c,8c,c,e,d,8c,d,8e,8d,c,8c,e,g,2a,a,g,8e,e,c,d,8c,d,8e,8d,c,8a5,a5,g5,2c");
            tunes.Play(rtttl.ToMelody());
        }
    }
}
