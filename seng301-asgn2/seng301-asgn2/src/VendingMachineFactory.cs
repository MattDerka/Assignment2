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

        total += coin.Value;
        var a = var.CoinSlot;


        var.CoinSlot.CoinAccepted += new EventHandler<CoinEventArgs>(temp.CoinAccepted);
        a.AddCoin(coin);
               
    }

    public void PressButton(int vmIndex, int value) {
        // TODO: Implement
        VendingMachine var = vendingMachines[vmIndex];
        Events temp = new Events(var);

        var.SelectionButtons[value].Pressed += new EventHandler(temp.printButtonPressed);
        var.SelectionButtons[value].Press();

        if(var.CoinReceptacle.Count != 0 && total >= var.PopCanCosts[value])
        {
                
            var temp2 = var.PopCanRacks;
            temp2[value].DispensePopCan();


            var.CoinRacks[2].ReleaseCoin();
            var.CoinRacks[2].ReleaseCoin();
        }

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

        return new VendingMachineStoredContents();
    }
}