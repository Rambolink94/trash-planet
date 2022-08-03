using UnityEngine;

public class Resource : Interactable
{
    // TODO: Write Editor Script for this.
    public LootTable LootTable;

    [Header("Yield Controls")]
    [SerializeField] private int maxYield = 20;
    [SerializeField] private int minYield = 1;
    [SerializeField] private int maxSpawnCount = 5;
    [SerializeField] private int minSpawnCount = 1;

    [Header("Scale Controls")]
    [SerializeField] private int baseScaleRatio = 10;
    [SerializeField] private float horizontalScaleFactor = 0.01f;
    [SerializeField] private float verticalScaleFactor = 0.1f;

    public int Yield = 10;

    private int currentYield;
    private Transform[] breakablesSpawnPoints;

    private void Start()
    {
        SetBreakablesSpawnPoints();

        Yield = Random.Range(minYield, maxYield);
        currentYield = Yield;

        DetermineStartScale();

        interactionType = InteractionType.Fill;
        SpawnBreakables();
    }

    public override void Interact(IInventory inventory, Vector3 interactionPoint)
    {
        int spawnCount = Random.Range(minSpawnCount, maxSpawnCount);
        for (int i = 0; i < spawnCount; i++)
        {
            Item item = LootTable.GetItem();
            GameManager.Instance.GarbageSpawner.SpawnCollectible(item, interactionPoint);
        }

        // TODO: Figure all this parent shit out.
        transform.localScale = new Vector3(transform.localScale.x - horizontalScaleFactor, transform.localScale.y - verticalScaleFactor, transform.localScale.z - horizontalScaleFactor);
        currentYield--;

        if (currentYield <= 0) gameObject.SetActive(false);
    }

    void DetermineStartScale()
    {
        // Yield 10 = Default Scale
        // 0.1f = scale unit vertical
        int scaleDiff = Yield - baseScaleRatio;
        float horizontalScale = 1f + (scaleDiff * horizontalScaleFactor);
        float verticalScale = 1f + (scaleDiff * verticalScaleFactor);

        transform.localScale = new Vector3(horizontalScale, verticalScale, horizontalScale);
    }

    void SpawnBreakables()
    {
        if (breakablesSpawnPoints != null)
        {
            for (int i = 0; i < breakablesSpawnPoints.Length; i++)
            {
                if (0.5 > Random.value)
                    GameManager.Instance.GarbageSpawner.SpawnBreakable(transform, breakablesSpawnPoints[i].position, false);
            }
        }
    }

    private void OnValidate()
    {
        SetBreakablesSpawnPoints();
    }

    private void SetBreakablesSpawnPoints()
    {
        breakablesSpawnPoints = new Transform[transform.childCount];
        int index = 0;
        foreach (Transform child in transform)
        {
            if (child != transform)
                breakablesSpawnPoints[index++] = child;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (breakablesSpawnPoints != null)
        {
            for (int i = 0; i < breakablesSpawnPoints.Length; i++)
            {
                Gizmos.DrawSphere(breakablesSpawnPoints[i].position, 0.2f);
            }
        }
    }
}
