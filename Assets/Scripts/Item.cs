using System;
using UnityEngine;

public class Item
{
    public String name {get; private set;}
    public int amount {get; private set;}
    public int prefabIndex {get; private set;}

    public Item(string name, int amount, int prefabIndex)
    {
        this.name = name;
        this.amount = amount;
        this.prefabIndex = prefabIndex;
    }

    public void changeAmount(int num)
    {
        amount += num;
    }
}    
