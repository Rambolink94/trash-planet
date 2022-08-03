using UnityEngine;

public class Collectible : Interactable
{
    private void Start()
    {
        interactionType = InteractionType.Single;
    }

    public override void Interact(IInventory inventory, Vector3 interactionPoint = default(Vector3))
    {
        bool success = inventory.Add(item);
        if (success)
        {
            GameManager.Instance.LogController.AddLog(item, 2f);
            gameObject.SetActive(false);
        }
    }

    public void MergeWithNeighbors()
    {
        // TODO: After some time, find nearby collectibles and merge into a trash pile
        // TODO: -- Get Neightbours on spawn, loop through them, determine a root, and store that.
    }
}