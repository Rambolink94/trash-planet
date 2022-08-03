using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [SerializeField] float detectionRadius = 2f;
    [SerializeField] bool autoPickUp = true;
    [SerializeField] LayerMask interactableLayers;
    [Space]
    [SerializeField] ButtonPromptController promptController;
    [SerializeField] Camera mainCamera;

    PlayerInventory playerInventory;
    PlayerInputManager inputManager;
    AudioSource audioSource;

    private RaycastHit hitInfo;
    private Resource lastHitResource;

    void Start()
    {
        playerInventory = GetComponent<PlayerInventory>();
        inputManager = GetComponent<PlayerInputManager>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        CollectNearbyCollectibles();
        Interact();
    }

    Interactable RaycastForInteractable()
    {
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out hitInfo, detectionRadius, interactableLayers))
        {
            Interactable interactable = hitInfo.collider.gameObject.GetComponent<Interactable>();
            if (interactable == null)
            {
                interactable = hitInfo.collider.gameObject.GetComponentInParent<Interactable>();
            }
            return interactable;
        }
        else
        {
            return null;
        }
    }

    void Interact()
    {
        // TODO: Redo all of this. It's trash.
        Interactable interactable = RaycastForInteractable();
        if (interactable != null)
        {
            Sprite promptSprite = interactable.GetPromtImage();
            if (promptSprite != null)
            {
                promptController.SetPromptSprite(promptSprite);
            }
            else
            {
                promptController.SetPromptText(interactable.GetPromptText());
            }

            if (inputManager.Interact && interactable == lastHitResource)
            {
                promptController.SetWaitFillActive(true);

                if (promptController.IncrementFill())
                {
                    lastHitResource.Interact(playerInventory, hitInfo.point + (hitInfo.normal * 0.1f));
                }
            }
            else if (inputManager.Interact && interactable is Collectible)
            {
                interactable.Interact(playerInventory);
            }
            else if (inputManager.FireAction && interactable is Breakable)
            {
                interactable.Interact();
            }
            else
            {
                promptController.ResetFill();
                promptController.SetWaitFillActive(false);
            }

            if (interactable is Resource)
            {
                lastHitResource = (Resource)interactable;
            }
        }
        else
        {
            promptController.SetPromptActive(false);
            promptController.SetWaitFillActive(false);

            inputManager.Interact = false;
            if (!inputManager.BuildMenu) inputManager.FireAction = false;
            lastHitResource = null;
        }
    }

    void CollectNearbyCollectibles()
    {
        if (autoPickUp)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, interactableLayers);
            foreach (Collider collider in colliders)
            {
                Collectible collectible = collider.gameObject.GetComponent<Collectible>();
                if (collectible != null)
                {
                    collectible.Interact(playerInventory);
                }
            }
        }
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
