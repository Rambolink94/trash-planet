using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] private string promptText;
    [SerializeField] private Sprite promptImage;
    [SerializeField] protected Item item;
    [SerializeField] private AudioClip interactionSound;
    public Item Item { get => item; set => item = value; }
    public AudioClip InteractionSound { get => interactionSound; set => interactionSound = value; }

    protected InteractionType interactionType;

    public abstract void Interact(IInventory inventory = null, Vector3 interactionPoint = default(Vector3));
    public string GetPromptText()
    {
        return promptText;
    }

    public Sprite GetPromtImage()
    {
        return promptImage;
    }
}

public enum InteractionType
{
    Single,
    Fill,
    Break,
    Mash
}
