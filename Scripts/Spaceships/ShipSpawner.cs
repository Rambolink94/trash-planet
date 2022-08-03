using UnityEngine;

public class ShipSpawner : MonoBehaviour
{
    // TODO: Implement pooler class for these type of things
    [SerializeField] GameObject[] shipsToSpawn;
    [SerializeField] bool spawnOnStart = true;
    [SerializeField] float shipSpawnRate = 5f;
    [SerializeField] float spawnHeight = 1000f;
    [SerializeField] float spawnDistOffset = 20f;

    [SerializeField] Collider terrainPlaneCollider;

    private float timeSinceLastShipSpawn;
    private Bounds terrainBounds;

    // Start is called before the first frame update
    void Start()
    {
        terrainBounds = terrainPlaneCollider.bounds;

        timeSinceLastShipSpawn = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnOnStart && Time.time - timeSinceLastShipSpawn >= shipSpawnRate)
        {
            Debug.Log("Spawning Ship...");

            int index = Random.Range(0, shipsToSpawn.Length);
            GameObject shipPrefab = shipsToSpawn[index];

            Debug.Log(terrainBounds.center);

            float xPos = (terrainBounds.center.x + spawnDistOffset) * (Random.Range(0, 2) * 2 - 1);   // Makes this pos or neg
            float zPos = (terrainBounds.center.z + spawnDistOffset) * (Random.Range(0, 2) * 2 - 1);   // Makes this pos or neg
            Vector3 spawnPos = new Vector3(xPos, spawnHeight, zPos);

            GameObject shipObject = Instantiate(shipPrefab, spawnPos, Quaternion.LookRotation(terrainBounds.center), transform);
            Spaceship spaceship = shipObject.GetComponent<Spaceship>();

            spaceship.InitShip(GameManager.Instance.GarbageSpawner, terrainPlaneCollider);

            timeSinceLastShipSpawn = Time.time;
        }
    }
}
