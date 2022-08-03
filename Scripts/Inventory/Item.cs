using UnityEngine;

[CreateAssetMenu()]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public GameObject itemPrefab;
    public int stackLimit;
}
