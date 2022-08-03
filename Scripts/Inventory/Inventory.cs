using System;
using UnityEngine;

public class Inventory
{
    public ItemStack[] Items { get; private set; }
    public int Capacity { get; private set; }
    public int Size { get; private set; }
    
    public Inventory(int size, Item[] starterItems = null)
    {
        Capacity = size;

        Items = new ItemStack[size];
        if (starterItems != null && starterItems.Length > 0)
        {
            if (starterItems.Length > size) Debug.LogError("Starter Items are longer than given size.");

            for (int i = 0; i < starterItems.Length; i++)
            {
                ItemStack itemStack = new ItemStack(starterItems[i]);
                Items[i] = itemStack;
            }
        }
    }

    public bool Add(Item item, int amountToAdd = 1)
    {
        int nullIndex = -1;
        int amountLeftToAdd = 0;
        for (int i = 0; i < Items.Length; i++)
        {
            // Find first open position
            if (Items[i] == null && nullIndex == -1) nullIndex = i;

            if (Items[i] != null && Items[i].Item == item && Items[i].HasSpace())
            {
                amountLeftToAdd = Items[i].AddCount(amountToAdd - amountLeftToAdd);
                if (amountLeftToAdd <= 0) return true;
            }
        }

        if (Size < Capacity)
        {
            Items[nullIndex] = new ItemStack(item);
            Size++;
            return true;
        }

        // Inventory full
        return false;
    }

    public bool Remove(Item item, int amountToRemove = 1)
    {
        int amountLeftToRemove = amountToRemove;
        for (int i = 0; i < Items.Length; i++)
        {
            if (Items[i].Item == item)
            {
                // Loop through stacks and continuously remove amounts until all amounts removed
                amountLeftToRemove = Items[i].RemoveCount(amountLeftToRemove);
                if (amountLeftToRemove >= 0)
                {
                    Items[i] = null;
                    Size--;
                }

                if (amountLeftToRemove == 0) return true;
            }
        }

        // If different, at least some items were removed.
        if (amountLeftToRemove != amountToRemove) return true;

        // Item not in inventory
        return false;
    }

    public void PrintItems()
    {
        Debug.Log($"Inventory: {Size}/{Capacity}");
        for (int i = 0; i < Items.Length; i++)
        {
            ItemStack currentStack = Items[i];

            if (currentStack != null)
            {
                Debug.Log($"Stack: {currentStack.Count}/{currentStack.StackSize} - Item: {currentStack.Item.itemName}");
            }
        }
    }
}

public interface IInventory
{
    public event Action OnInventoryChanged;
    Inventory Inventory { get; set; }
    bool Add(Item item);
    bool Remove(Item item);
}
