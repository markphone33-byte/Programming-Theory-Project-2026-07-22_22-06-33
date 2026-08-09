using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }
    private List<Item> inventory = new List<Item>();
    private GameObject[] inventoryUIObjects = new GameObject[0];
    private int selectedItemSlot = 0;
    private InputSystem_Actions controls;
    [SerializeField] private GameObject emptyItemSlot;
    private GameObject itemSlotsParent;
    private Color selectSlotColor;
    private Color defaultSlotColor;

    // Enabled player input for switching between items
    void OnEnable()
    {
        controls.Player.ItemSwitch.Enable();
    }
    // Disables player input for switching between items
    void OnDisable()
    {
        controls.Player.ItemSwitch.Disable();
    }

    // Initalizes variables
    void Awake()
    {
        Instance = this;
        controls = new InputSystem_Actions();
        selectSlotColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        defaultSlotColor = new Color(0f, 0f, 0f, 0.5f);
    }

    // Initalizes references and starts player off with their fists
    void Start()
    {
        itemSlotsParent = GameObject.Find("ItemSlots");
        PickUpItem("Fists", null); // Player starts with fists and can't drop them
    }

    // Update is called once per frame
    void Update()
    {
        // When the player presses "K" or "L" they swtich between their item slots
        if (controls.Player.ItemSwitch.WasPressedThisFrame())
        {
            SwitchSelectedItem();
        }
    }

    // Called by collectibles when the player picks them up. Adds the item to the player's inventory
    public void PickUpItem(string name, GameObject item)
    {
        Item newItem = new Item(name, 1, item);
        // If the same item is already in an item slot then add it to that same slot just by increasing the amount
        for (int i = 0; i < inventory.Count; i++)
        {
            Item itemInInventory = inventory[i];
            if (itemInInventory.name == name && itemInInventory.item == item)
            {
                inventory[i].increaseAmount(1);
                UpdateInventoryUI();
                return;
            }
        }

        // Otherwise make a new item slot for the item
        inventory.Add(newItem);
        UpdateInventoryUI();
    }

    // Takes the items in the current inventory list and adds UI item slots on the player's screen to represent it
    private void UpdateInventoryUI()
    {
        RectTransform parentTransform = itemSlotsParent.GetComponent<RectTransform>();

        //Destroys all the old item slot UI displays
        if (inventoryUIObjects.Length > 0)
        {
            foreach (GameObject itemSlot in inventoryUIObjects)
            {
                Destroy(itemSlot);
            }
        }

        //For each item in the player's inventory make a new item slot UI display for it
        inventoryUIObjects = new GameObject[inventory.Count];
        for (int i = 0; i < inventory.Count; i++)
        {
            Vector2 itemSlotPosition = new Vector2(100 * i, 0); // Positions each new item slot UI display next to each other but not overlapping
            GameObject itemSlot = Instantiate(emptyItemSlot, itemSlotsParent.transform); // Makes a default empty item slot UI display
            RectTransform itemSlotTransform = itemSlot.GetComponent<RectTransform>();
            // Customizes the UI display to match the item being displayed
            itemSlotTransform.anchoredPosition = itemSlotPosition;
            itemSlotTransform.Find("ItemName").GetComponent<TextMeshProUGUI>().text = inventory[i].name;
            itemSlotTransform.Find("ItemNumber").GetComponent<TextMeshProUGUI>().text = "" + (i + 1);
            itemSlotTransform.Find("ItemAmount").GetComponent<TextMeshProUGUI>().text = inventory[i].amount + "x";
            inventoryUIObjects[i] = itemSlot;
        }
        // Repositions the parent transform to make everything centered at the bottom middle of the screen
        Vector2 parentPosition = new Vector2(-50 * (inventory.Count - 1), 100);
        parentTransform.anchoredPosition = parentPosition;

        GetItemSlot(selectedItemSlot).GetComponent<Image>().color = selectSlotColor;
    }

    // Returns the item slot UI display game object given its slot number
    private GameObject GetItemSlot(int slotNumber)
    {
        if (slotNumber < inventoryUIObjects.Length && slotNumber >= 0)
        {
            return inventoryUIObjects[slotNumber];
        }
        return null; // If there are no item slots or if the provided slot number does not exist then return null
    }

    private void SwitchSelectedItem()
    {
        GetItemSlot(selectedItemSlot).GetComponent<Image>().color = defaultSlotColor; // Current selected slot returns to normal color
        // The +1 is needed since the inventory list starts indexing at 0 but item slots are numbered 1, 2, 3, etc...

        // Changes the selected item slot to the next slot, +1 or -1 the current slot number, based on what the player inputted
        selectedItemSlot += (int)controls.Player.ItemSwitch.ReadValue<float>();
        // Item slots cycle back around so +1 on the last item goes back to the first and -1 on the first goes to the last
        if (selectedItemSlot >= inventory.Count)
        {
            selectedItemSlot = 0;
        }
        else if (selectedItemSlot < 0)
        {
            selectedItemSlot = inventory.Count - 1;
        }

        GetItemSlot(selectedItemSlot).GetComponent<Image>().color = selectSlotColor; // New selected slot takes on the selected color
    }
}