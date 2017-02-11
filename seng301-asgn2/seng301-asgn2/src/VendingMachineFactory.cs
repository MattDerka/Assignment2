using System;
using System.Collections.Generic;
using System.Collections;
using Frontend2;
using Frontend2.Hardware;
using seng301_asgn2;

public class VendingMachineFactory : IVendingMachineFactory {

    static int counter = 0;
    List<VendingMachine> vendingMachines = new List<VendingMachine>();

    public int CreateVendingMachine(List<int> coinKinds, int selectionButtonCount, int coinRackCapacity, int popRackCapcity, int receptacleCapacity) {

        var test = coinKinds.ToArray();
        vendingMachines.Add(new VendingMachine(test, selectionButtonCount, coinRackCapacity, popRackCapcity, receptacleCapacity));

        counter++;
        return counter;
    }

    public void ConfigureVendingMachine(int vmIndex, List<string> popNames, List<int> popCosts) {

        VendingMachine var = vendingMachines[vmIndex];
        var.Configure(popNames, popCosts);

    }


    public void LoadCoins(int vmIndex, int coinKindIndex, List<Coin> coins) {
        // TODO: Implement

        VendingMachine var = vendingMachines[vmIndex];

        int coinKind = var.GetCoinKindForCoinRack(coinKindIndex);
        var.GetCoinRackForCoinKind(coinKind).LoadCoins(coins);
    }

    public void LoadPops(int vmIndex, int popKindIndex, List<PopCan> pops) {
        // TODO: Implement

        VendingMachine var = vendingMachines[vmIndex];
        var a = var.PopCanRacks;
        a[popKindIndex].LoadPops(pops);
    }

    int total = 0;
    public void InsertCoin(int vmIndex, Coin coin) {
        // TODO: Implement
        VendingMachine var = vendingMachines[vmIndex];
        Events temp = new Events(var);

        var a = var.CoinSlot;

        var.CoinSlot.CoinAccepted += new EventHandler<CoinEventArgs>(temp.CoinAccepted);
        a.AddCoin(coin);
        var.CoinSlot.CoinAccepted -= new EventHandler<CoinEventArgs>(temp.CoinAccepted);


        total += coin.Value;
    }


    public void PressButton(int vmIndex, int value) {
        // TODO: Implement
        VendingMachine var = vendingMachines[vmIndex];
        Events temp = new Events(var);

        var.SelectionButtons[value].Pressed += new EventHandler(temp.printButtonPressed);
        var.SelectionButtons[value].Press();
        var.SelectionButtons[value].Pressed -= new EventHandler(temp.printButtonPressed);


        if (var.CoinReceptacle.Count != 0 && total >= var.PopCanCosts[value])
        {

            var changedNeeded = total - (var.PopCanCosts[value]);

            var.CoinReceptacle.ReceptacleFull += new EventHandler(temp.CoinReceptacle_ReceptacleFull);
            var.CoinReceptacle.StoreCoins();
            var.CoinReceptacle.ReceptacleFull -= new EventHandler(temp.CoinReceptacle_ReceptacleFull);


            var temp2 = var.PopCanRacks;
            temp2[value].DispensePopCan();

            List<int> b = new List<int>();
            int length = var.CoinRacks.Length;
            
            for(int i = 0; i < length; i++)
            {
                b.Add(var.GetCoinKindForCoinRack(i));
            }

            b.Sort();

            for(int i = b.Count-1; i >=0; i--)
            {
                int jk = b[i];
                while(changedNeeded % jk == 0 && changedNeeded != 0)
                {
                    var.GetCoinRackForCoinKind(jk).ReleaseCoin();

                    changedNeeded -= jk;
                }

                if(changedNeeded % jk != 0 && changedNeeded > jk)
                {
                    var.GetCoinRackForCoinKind(jk).ReleaseCoin();
                    changedNeeded -= jk;
                }

            }


        }

        total = 0;

    }

    private void CoinReceptacle_ReceptacleFull(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    public List<IDeliverable> ExtractFromDeliveryChute(int vmIndex) {
        // TODO: Implement

        VendingMachine var = vendingMachines[vmIndex];
        var temp = var.DeliveryChute;
        List<IDeliverable> temp2 = new List<IDeliverable>(temp.RemoveItems());
        //return new List<IDeliverable>();
        return temp2;

    }

    public VendingMachineStoredContents UnloadVendingMachine(int vmIndex) {
        // TODO: Implement

        VendingMachine var = vendingMachines[vmIndex];
        
        VendingMachineStoredContents temp = new VendingMachineStoredContents();

        foreach(var i in var.CoinRacks)
        {
            temp.CoinsInCoinRacks.Add(i.Unload());
        }
        List<Coin> d = var.StorageBin.Unload();

        foreach(Coin i in d)
        {
            temp.PaymentCoinsInStorageBin.Add(i);
        }

        foreach(var i in var.PopCanRacks)
        {
            temp.PopCansInPopCanRacks.Add(i.Unload());
        }

        return temp;
    }
}