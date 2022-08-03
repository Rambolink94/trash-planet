using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Grid ParentGrid;
    public float Rotation;
    public float Offset;
    public float Interval;
    public bool IsParent;

    private List<Grid> childrenGrids = new List<Grid>();
    private Transform[] snapPositions;
    private BuildingController buildingController;
    private Dictionary<Renderer, Material> originalMaterials = new Dictionary<Renderer, Material>();

    // Start is called before the first frame update
    void Start()
    {
        snapPositions = transform.GetComponentsInChildren<Transform>();

        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            originalMaterials[renderer] = renderer.material;
        }
    }

    public void SetBuildingController(BuildingController controller)
    {
        buildingController = controller;
    }

    public Vector3 GetNearestSnapPosition(Vector3 hitPosition)
    {
        Transform nearestPos = null;
        float closestMagnitude = 0f;
        bool first = true;
        foreach (Transform snapPos in snapPositions)
        {
            float nextMagnitude = (snapPos.position - hitPosition).magnitude;
            if (first)
            {
                nearestPos = snapPos;
                closestMagnitude = nextMagnitude;

                first = false;
            }
            else
            {
                if (nextMagnitude < closestMagnitude)
                {
                    closestMagnitude = nextMagnitude;
                    nearestPos = snapPos;
                }
            }
        }

        return nearestPos.position;
    }

    public void ModifyRenderer(ValidState validState)
    {
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            switch (validState)
            {
                case ValidState.None:
                    renderer.material = originalMaterials[renderer];
                    break;
                case ValidState.Valid:
                    renderer.material = buildingController.ValidMat;
                    break;
                case ValidState.Invalid:
                    renderer.material = buildingController.InvalidMat;
                    break;
                default:
                    break;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (snapPositions != null)
        {
            foreach (Transform snapPos in snapPositions)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawCube(snapPos.position, Vector3.one);
            }
        }
    }
}

public enum ValidState
{
    None,
    Valid,
    Invalid
}
