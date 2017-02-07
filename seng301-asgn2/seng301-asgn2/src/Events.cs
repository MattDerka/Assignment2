using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Frontend2.Hardware;

namespace seng301_asgn2
{
    class Events : EventArgs
    {
        public void printCoinAccepted(object sender, CoinEventArgs e)
        {
            Console.WriteLine("Coin Slot accepted coin");
        }

    }
}
