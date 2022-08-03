using UnityEngine;

[CreateAssetMenu()]
public class LootTable : ScriptableObject
{
    public ItemChance[] loot;

    public Item GetItem()
    {
        if (loot != null && loot.Length > 0)
        {
            float roll = Random.Range(0f, 1f);

            foreach (ItemChance itemChance in loot)
            {
                if (roll <= itemChance.chance)
                {
                    return itemChance.item;
                }
            }
        }

        return null;
    }
}

[System.Serializable]
public struct ItemChance
{
    [Range(0f, 1f)]
    public float chance;
    public Item item;
}