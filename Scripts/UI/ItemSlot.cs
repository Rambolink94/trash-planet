using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI itemCountText;

    private Item item;
    private int count;
    private Color transparentColor = new Color(0f, 0f, 0f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        UpdateUI();
    }

    public void SetItem(ItemStack itemStack)
    {
        item = itemStack?.Item;
        count = itemStack != null ? itemStack.Count : 0;

        UpdateUI();
    }

    void UpdateUI()
    {
        if (item != null)
        {
            itemImage.sprite = item.itemIcon;
            itemImage.color = Color.white;

            if (count <= 1)
            {
                itemCountText.gameObject.SetActive(false);
            }
            else
            {
                itemCountText.gameObject.SetActive(true);
                itemCountText.SetText(count.ToString());
            }
        }
        else
        {
            itemImage.sprite = null;
            itemImage.color = transparentColor;
            itemCountText.gameObject.SetActive(false);
        }
    }
}
