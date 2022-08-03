using System.Collections.Generic;
using UnityEngine;

public class GarbageSpawner : MonoBehaviour
{
    [SerializeField] bool spawnOnStart = true;
    [SerializeField] int spawnCount = 100;
    [SerializeField] float spawnOffset = 0.2f;
    [SerializeField] GameObject[] trashPilePrefabs;
    [SerializeField] GameObject[] breakablePrefabs;
    [SerializeField] Transform collectibleParent;
    [SerializeField] Collider terrainPlaneCollider;
    [SerializeField] Transform playerCharacter;

    private List<GameObject> heaps = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        if (spawnOnStart)
        {
            for (int i = 0; i < spawnCount; i++)
            {
                Vector3 spawnPoint = HelperFunctions.RandomPointInBounds(terrainPlaneCollider.bounds);
                Vector3 raycastPoint = GetRaycastPoint(spawnPoint);

                if (spawnPoint == raycastPoint) continue;

                GameObject heap = SpawnTrashPile(false, raycastPoint);

                // Add to list for later use
                heaps.Add(heap);
            }

            if (playerCharacter == null)
            {
                playerCharacter = GameObject.FindGameObjectWithTag("Player").transform;
            }
        }
    }

    public float GetDistanceFromPlayer(Vector3 chosenVector)
    {
        return Vector3.Distance(chosenVector, playerCharacter.position);
    }

    private Vector3 GetRaycastPoint(Vector3 spawnPoint)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(spawnPoint + new Vector3(0f, 20f, 0f), Vector3.down, out hitInfo, 50f))
        {
            return hitInfo.point;
        }

        return spawnPoint;
    }

    public GameObject SpawnTrashPile(bool disableOnCreation = false, Vector3 spawnPos = default)
    {
        int trashIndex = Random.Range(0, trashPilePrefabs.Length);

        spawnPos = new Vector3(spawnPos.x, spawnPos.y - spawnOffset, spawnPos.z);

        Vector3 rotationVector = new Vector3(0f, Random.Range(0f, 360f), 0f);
        GameObject trashPile = Instantiate(trashPilePrefabs[trashIndex], spawnPos, Quaternion.Euler(rotationVector), transform);

        if (disableOnCreation) trashPile.SetActive(false);

        return trashPile;
    }

    public GameObject SpawnCollectible(Item item, Vector3 spawnPos, bool disableOnCreation = false)
    {
        GameObject collectible = Instantiate(item.itemPrefab, spawnPos, Random.rotation, collectibleParent);

        if (disableOnCreation) collectible.SetActive(false);

        return collectible;
    }

    public GameObject SpawnBreakable(Transform parent, Vector3 spawnPos, bool disableOnCreation = false)
    {
        int breakableIndex = Random.Range(0, breakablePrefabs.Length);
        GameObject breakable = Instantiate(breakablePrefabs[breakableIndex], spawnPos, Random.rotation, parent);

        if (disableOnCreation) breakable.SetActive(false);

        return breakable;
    }
}
