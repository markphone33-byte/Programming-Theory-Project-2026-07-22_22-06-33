using System;
using UnityEngine;

public class Item
{
    public String name {get; private set;}
    public int amount {get; private set;}
    public GameObject item {get; private set;}

    public Item(string name, int amount, GameObject item)
    {
        this.name = name;
        this.amount = amount;
        this.item = item;
    }

    public void increaseAmount(int num)
    {
        amount += num;
    }
}    
