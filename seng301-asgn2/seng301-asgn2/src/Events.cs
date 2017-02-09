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
        VendingMachine a;

        public Events()
        {

        }

        public Events(VendingMachine a)
        {
            this.a = a;
        }

        public void CoinAccepted(object sender, CoinEventArgs e)
        {
            Console.WriteLine("Coin Slot accepted coin");
            a.CoinReceptacle.StoreCoins();
            a.CoinReceptacle.ReceptacleFull += CoinReceptacle_ReceptacleFull;
        }

        public void CoinReceptacle_ReceptacleFull(object sender, EventArgs e)
        {
            a.StorageBin.StoreCoins();
        }

        private void coinsRemoved(object sender, EventArgs e)
        {
            Console.WriteLine("coins removed from recep");
        }



        public void printButtonPressed(object sender, EventArgs e)
        {
            Console.WriteLine("Button pressed");
        }

    }
}
