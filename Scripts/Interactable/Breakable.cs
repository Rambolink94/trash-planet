using UnityEngine;

public class Breakable : Interactable
{
    [SerializeField] LootTable lootTable;
    [SerializeField] GameObject fixedVersion;
    [SerializeField] GameObject brokenVersion;
    [SerializeField] int collectibleSpawnMin = 1;
    [SerializeField] int collectibleSpawnMax;

    bool isBroken = false;

    private void Start()
    {
        interactionType = InteractionType.Break;
        if (brokenVersion != null)
            brokenVersion.SetActive(false);
    }

    public override void Interact(IInventory inventory = null, Vector3 interactionPoint = default(Vector3))
    {
        if (!isBroken)
        {
            fixedVersion.SetActive(false);
            if (brokenVersion != null)
                brokenVersion.SetActive(true);

            int spawnCount = Random.Range(collectibleSpawnMin, collectibleSpawnMax);
            for (int i = 0; i <= spawnCount; i++)
            {
                Vector3 spawnPoint = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
                Item item = lootTable.GetItem();
                GameManager.Instance.GarbageSpawner.SpawnCollectible(item, spawnPoint);
            }

            isBroken = true;
        }
    }
}
