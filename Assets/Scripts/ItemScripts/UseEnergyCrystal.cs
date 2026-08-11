 using UnityEngine;

public class UseEnergyCrystal : UseItem
{
    public override void Use()
    {
        if(PlayerInventory.Instance.GetSelectedItem().amount >= 3)
        {
            Debug.Log("5 Crystals!");
        }
        else
        {
            Debug.Log("Not Enough");
        }
    }
}
