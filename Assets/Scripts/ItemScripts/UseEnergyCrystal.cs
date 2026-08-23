 using UnityEngine;

public class UseEnergyCrystal : UseItem
{
    public override void Use()
    {
        if(PlayerInventory.Instance.RemoveItem(ItemManager.Instance.GetEnergyCrystalIndex(), LevelManager.Instance.crystalsNeeded))
        {
            LevelManager.Instance.NextLevel();
        }
        else
        {
            Debug.Log("Not Enough, Get: " + LevelManager.Instance.crystalsNeeded);
        }
    }
}
