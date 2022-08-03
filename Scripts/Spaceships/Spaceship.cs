using UnityEngine;
using DG.Tweening;
using System.Collections;

public class Spaceship : MonoBehaviour
{
    [Header("Ship Controls")]
    [SerializeField] float movementSpeed = 10f;
    [SerializeField] float rotationDuration = 2f;
    [SerializeField] float departureSpeedMult = 1f;
    [SerializeField] float departureHeight = 100f;
    [SerializeField] float departureDistance = 100f;
    [SerializeField] float minHoverDist = 10f;
    [SerializeField] float maxHoverDist = 12f;
    [SerializeField] float dumpDurationPerYield = 0.1f;

    [Header("Door Controls")]
    [SerializeField] Transform doorHinge;
    [SerializeField] float hingeRotation = -90f;
    [SerializeField] float hingeRotationSpeed = 5f;

    [Header("Engine Controls")]
    [SerializeField] Transform[] engines;
    [SerializeField] float engineRotation = -90f;
    [SerializeField] float engineRotationSpeed = 5f;

    [Header("Trash Spawning")]
    [SerializeField] GameObject trashParticleObject;
    [SerializeField] int minCollectibleSpawn = 1;
    [SerializeField] int maxCollectibleSpawn = 10;
    [SerializeField] Transform raycastOrigin;
    [SerializeField] LayerMask trashSpawnLayers;
    [SerializeField] float distanceToAllowSpawn = 20f;

    private Vector3 dropPoint;
    private float distanceFromPlayer;
    private ParticleSystem trashParticleSystem;
    private float dumpDuration;
    Collider terrainPlaneCollider;
    GarbageSpawner garbageSpawner;

    public void InitShip(GarbageSpawner garbageSpawner, Collider collider)
    {
        this.garbageSpawner = garbageSpawner;
        terrainPlaneCollider = collider;

        dropPoint = HelperFunctions.RandomPointInBounds(terrainPlaneCollider.bounds);
        dropPoint = new Vector3(dropPoint.x, dropPoint.y + Random.Range(minHoverDist, maxHoverDist), dropPoint.z);

        distanceFromPlayer = garbageSpawner.GetDistanceFromPlayer(dropPoint);

        trashParticleSystem = trashParticleObject.GetComponent<ParticleSystem>();

        PlayShipAnimation();
    }

    void PlayShipAnimation()
    {
        // TODO: Seperate into even more functions and make more dynamic
        Sequence sequence = DOTween.Sequence();
        sequence.Pause();

        // Spawn a trash stack
        Vector3 rotationVector = new Vector3(0f, Random.Range(0f, 360f), 0f);
        GameObject trashStackObject = garbageSpawner.SpawnTrashPile(true);
        Resource resource = trashStackObject.GetComponentInChildren<Resource>();

        int extraItemSpawnCount = Random.Range(minCollectibleSpawn, maxCollectibleSpawn);
        // If extra spawning is too far away
        if (distanceFromPlayer > distanceToAllowSpawn)
        {
            resource.Yield += extraItemSpawnCount;
        }

        dumpDuration = resource.Yield * dumpDurationPerYield;

        // Configure Particle Effect
        ParticleSystem.MainModule mainSystem = trashParticleSystem.main;
        mainSystem.duration = dumpDuration;

        // Arrival
        float time = CalculateTime(movementSpeed, (transform.position - dropPoint).magnitude);
        sequence.Append(transform.DOMove(dropPoint, time).SetEase(Ease.InOutSine));
        sequence.Join(transform.DOLookAt(dropPoint, rotationDuration).SetEase(Ease.OutCubic));

        // Engines
        SequenceShipEngines(sequence);

        // Door
        Tween openDoorTween = doorHinge.DOLocalRotate(new Vector3(0f, 0f, hingeRotation), hingeRotationSpeed).SetEase(Ease.OutBack);
        Tween closeDoorTween = doorHinge.DOLocalRotate(new Vector3(0f, 0f, 0f), hingeRotationSpeed).SetEase(Ease.InBack);
        sequence.Append(openDoorTween);
        sequence.AppendCallback(() => StartTrashDump(resource.LootTable, extraItemSpawnCount));
        sequence.AppendInterval(dumpDuration);
        sequence.AppendCallback(() => PlaceTrashStack(trashStackObject));
        sequence.Append(closeDoorTween);

        // Departer
        Vector3 departerLocation = new Vector3(transform.position.x + departureDistance, departureHeight, transform.position.z + departureDistance);
        time = CalculateTime(movementSpeed * departureSpeedMult, (transform.position - departerLocation).magnitude);
        sequence.Append(transform.DOMove(departerLocation, time).SetEase(Ease.InQuad));
        sequence.Join(transform.DOLookAt(departerLocation, rotationDuration).SetEase(Ease.InOutQuad));

        // Engines again
        SequenceShipEngines(sequence, true);

        sequence.Append(transform.DOScale(new Vector3(0.01f, 0.01f, 0.01f), time));

        sequence.Play();

        sequence.onKill += ShipSequenceComplete;
    }

    void StartTrashDump(LootTable lootTable, int spawnCount)
    {
        trashParticleSystem.Play();

        StartCoroutine(SpawnCollectibles(lootTable, spawnCount));
    }

    IEnumerator SpawnCollectibles(LootTable lootTable, int spawnCount)
    {
        float timeBetweenSpawns = dumpDuration / spawnCount;

        int amountSpawned = 0;
        while(amountSpawned < spawnCount)
        {
            Item item = lootTable.GetItem();
            garbageSpawner.SpawnCollectible(item, raycastOrigin.position);

            amountSpawned++;

            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    void ShipSequenceComplete()
    {
        gameObject.SetActive(false);
    }

    float CalculateTime(float speed, float distance)
    {
        return distance / speed;
    }

    void SequenceShipEngines(Sequence sequence, bool reverse = false)
    {
        float rotation = reverse ? 0f : engineRotation;
        Vector3 engineRotateVector = new Vector3(0f, 0f, rotation);
        for (int i = 0; i < engines.Length; i++)
        {
            sequence.Join(engines[i].DOLocalRotate(engineRotateVector, engineRotationSpeed).SetEase(Ease.InExpo));
        }
    }

    void PlaceTrashStack(GameObject objectToPlace)
    {
        Debug.Log("Dumping Trash");

        RaycastHit hitInfo;
        if (Physics.Raycast(raycastOrigin.position, Vector3.down, out hitInfo, 100f, trashSpawnLayers))
        {
            objectToPlace.transform.position = hitInfo.point;
            objectToPlace.SetActive(true);
        }
    }

    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(raycastOrigin.position, Vector3.down * 10f);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.forward * 10f);
    }
}
