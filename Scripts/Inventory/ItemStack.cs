using UnityEngine;

public class ItemStack
{
    public Item Item { get; private set; }
    public int StackSize { get; private set; }
    public int Count { get; private set; }

    public ItemStack(Item item)
    {
        Item = item;

        StackSize = item.stackLimit;
        Count = 1;
    }

    public int AddCount(int amountToAdd = 1)
    {
        int availableSpace = StackSize - Count;
        int returnCount = availableSpace - amountToAdd;

        if (returnCount <= 0)
        {
            // Stack is now full, so return what wasn't added.
            Count = StackSize;
            return returnCount;
        }

        
        Count++;
        return 0;
    }

    public int RemoveCount(int amountToRemove)
    {
        int amountStillLeftToRemove = Mathf.Abs(Count - amountToRemove);

        return amountStillLeftToRemove;
    }

    public bool HasSpace(int amountToAdd = 1)
    {
        return Count + amountToAdd <= StackSize;
    }
}
