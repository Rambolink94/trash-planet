using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    [SerializeField] float unitSize = 1.25f;
    [SerializeField] float placementDistance = 10f;
    [SerializeField] float rotationInterval = 45f;
    [SerializeField] GameObject[] buildObjectPrefabs;
    public Material ValidMat;
    public Material InvalidMat;
    [SerializeField] LayerMask placeableLayers;
    [SerializeField] LayerMask buildingLayers;
    [SerializeField] Camera mainCamera;

    private Transform buildingParent;
    private GameObject[] physicalBuildObjects;
    private GameObject selectedBuildObject;
    private Renderer[] selectedRenderers;
    private int buildIndex = 0;
    private bool isValid = false;
    private Vector3 hitlocation;
    private PlayerInputManager inputManager;

    private Quaternion lastRotation;
    private Grid closestSnapGrid;
    private bool snapDetected = false;
    private bool initialSnap = false;
    private bool positionOnGrid = false;
    private float snapDetectionRadius;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = GetComponent<PlayerInputManager>();
        snapDetectionRadius = unitSize * 4;

        buildingParent = new GameObject("Building").transform;

        GameObject parentObject = new GameObject("Buildible Blocks");
        physicalBuildObjects = new GameObject[buildObjectPrefabs.Length];

        for (int i = 0; i < buildObjectPrefabs.Length; i++)
        {
            GameObject buildObject = Instantiate(buildObjectPrefabs[i], parentObject.transform);
            Collider[] colliders = buildObject.GetComponentsInChildren<Collider>();
            foreach (Collider collider in colliders)
            {
                collider.enabled = false;
            }

            buildObject.SetActive(false);

            physicalBuildObjects[i] = buildObject;
        }

        SwitchBuildObject(0);
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Properly toggle build menu.

        if (inputManager.BuildMenu )
        {
            // Scroll Changed
            int switchState = inputManager.GetSwitchAction();
            if (switchState != 0) SwitchBuildObject(switchState);

            if (GetPlacementPosition())
            {
                if (inputManager.Scroll != 0)
                {
                    Debug.Log("Scroll: " + inputManager.Scroll);

                    lastRotation = CalculateRotation(lastRotation.eulerAngles.y, inputManager.Scroll);

                    if (snapDetected && initialSnap)
                    {
                        lastRotation = closestSnapGrid.transform.rotation;
                        initialSnap = false;
                    }
                }

                if (snapDetected)
                {
                    hitlocation = GetSnapPosition(hitlocation, closestSnapGrid.transform.position, unitSize);

                    if (positionOnGrid)
                    {
                        
                    }
                }

                selectedBuildObject.transform.position = hitlocation;
                selectedBuildObject.transform.rotation = lastRotation;

                for (int i = 0; i < selectedRenderers.Length; i++)
                {
                    selectedRenderers[i].material = isValid ? ValidMat : InvalidMat;
                }

                selectedBuildObject.SetActive(true);

                HandlePlacement();
                HandleDeletion();
            }
            else
            {
                selectedBuildObject.SetActive(false);
            }
        }
        else
        {
            selectedBuildObject.SetActive(false);
        }
    }

    void HandlePlacement()
    {
        if (inputManager.FireAction)
        {
            GameObject buildObject = Instantiate(buildObjectPrefabs[buildIndex], selectedBuildObject.transform.position, selectedBuildObject.transform.rotation, buildingParent);
            Grid grid = buildObject.AddComponent<Grid>();
            if (closestSnapGrid != null)
            {
                grid.ParentGrid = closestSnapGrid;
            }

            inputManager.FireAction = false;
        }
    }

    void HandleDeletion()
    {
        if (inputManager.MiddleMouse && positionOnGrid)
        {
            Destroy(closestSnapGrid.gameObject);
            positionOnGrid = false;
            inputManager.MiddleMouse = false;
        }
    }

    bool GetPlacementPosition()
    {
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, placementDistance))
        {
            isValid = placeableLayers == (placeableLayers | (1 << hitInfo.collider.gameObject.layer));

            hitlocation = hitInfo.point;
            Grid grid = hitInfo.collider.gameObject.GetComponent<Grid>();
            GetSnapGrid(grid);
            return true;
        }

        return false;
    }

    Vector3 GetSnapPosition(Vector3 position, Vector3 snapGridPosition, float unit)
    {
        float snapXDiff = snapGridPosition.x - (Mathf.Round(snapGridPosition.x / unit) * unit);
        float snapYDiff = snapGridPosition.y - (Mathf.Round(snapGridPosition.y / unit) * unit);
        float snapZDiff = snapGridPosition.z - (Mathf.Round(snapGridPosition.z / unit) * unit);

        // Add snap grid position as offset to rounded position
        float x = Mathf.Round(position.x / unit) * unit + snapXDiff;
        float y = Mathf.Round(position.y / unit) * unit + snapYDiff;
        float z = Mathf.Round(position.z / unit) * unit + snapZDiff;

        return new Vector3(x, y, z);
    }

    void GetSnapGrid(Grid grid = null)
    {
        // TODO: Turn this into a recursive function
        if (grid == null)
        {
            Collider[] colliders = Physics.OverlapSphere(hitlocation, snapDetectionRadius, buildingLayers);
            for (int i = 0; i < colliders.Length; i++)
            {
                grid = colliders[i].gameObject.GetComponentInParent<Grid>();
                if (grid != null)
                {
                    snapDetected = true;
                    break;
                }
            }

            if (grid == null)
            {
                snapDetected = false;
                initialSnap = true;
                positionOnGrid = false;
                return;
            }
        }
        else
        {
            positionOnGrid = true;
            snapDetected = true;
        }

        closestSnapGrid = grid;
    }

    void SwitchBuildObject(int switchState)
    {
        buildIndex += switchState;

        if (buildIndex < 0) buildIndex = physicalBuildObjects.Length - 1;
        else if (buildIndex > physicalBuildObjects.Length - 1) buildIndex = 0;

        if (selectedBuildObject != null) selectedBuildObject.SetActive(false);
        selectedBuildObject = physicalBuildObjects[buildIndex];
        selectedRenderers = selectedBuildObject.GetComponentsInChildren<Renderer>();
    }

    Quaternion CalculateRotation(float currentRot, float direction)
    {
        return Quaternion.Euler(new Vector3(0f, currentRot + (rotationInterval * direction), 0f));
    }
    
}
