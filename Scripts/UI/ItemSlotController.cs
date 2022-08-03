using UnityEngine;

public class ItemSlotController : MonoBehaviour
{
    [SerializeField] GameObject itemSlotPrefab;

    private ItemSlot[] itemSlots;
    private Inventory inventory;

    public void InitItemSlots(IInventory _inventory)
    {
        inventory = _inventory.Inventory;
        _inventory.OnInventoryChanged += UpdateInventoryUI;

        itemSlots = new ItemSlot[inventory.Items.Length];
        int slotsToInstantiate = itemSlots.Length - transform.childCount;
        int i = 0;

        foreach (Transform child in transform)
        {
            AssignSlot(child.gameObject, i);
            i++;
        }

        for (; i < itemSlots.Length; i++)
        {
            GameObject itemSlotObject = Instantiate(itemSlotPrefab, transform);
            AssignSlot(itemSlotObject, i);
        }

        UpdateInventoryUI();
    }

    void UpdateInventoryUI()
    {
        Debug.Log("Updating Inventory UI...");
        for (int i = 0; i < inventory.Items.Length; i++)
        {
            ItemStack itemStack = inventory.Items[i];

            itemSlots[i].SetItem(itemStack);
        }
    }

    void AssignSlot(GameObject slotObject, int index)
    {
        slotObject.name = "Item Slot - " + (index + 1);

        ItemSlot itemSlot = slotObject.GetComponent<ItemSlot>();
        itemSlots[index] = itemSlot;
    }
}
