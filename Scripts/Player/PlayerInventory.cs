using System;
using UnityEngine;

public class PlayerInventory : MonoBehaviour, IInventory
{
    [SerializeField] int inventorySize;
    [SerializeField] Item[] starterItems;
    [SerializeField] ItemSlotController itemSlotController;

    public event Action OnInventoryChanged;

    public Inventory Inventory { get; set; }

    private void Start()
    {
        Inventory = new Inventory(inventorySize, starterItems);
        itemSlotController.InitItemSlots(this);
    }

    public bool Add(Item item)
    {
        bool success = Inventory.Add(item);
        if (success) OnInventoryChanged?.Invoke();

        return success;
    }

    public bool Remove(Item item)
    {
        bool success = Inventory.Remove(item);
        if (success) OnInventoryChanged?.Invoke();

        return success;
    }
}
