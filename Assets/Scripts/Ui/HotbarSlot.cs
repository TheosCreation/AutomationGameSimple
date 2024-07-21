using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Items; // if you're using TextMeshPro for the quantity text

public class HotbarSlot : MonoBehaviour
{
    public Image itemIcon;
    public TextMeshProUGUI itemQuantity;

    public void SetItem(Item item, int quantity)
    {
        if (item != null)
        {
            itemIcon.sprite = item.PreviewSprite;
            itemIcon.enabled = true;
            itemQuantity.text = quantity.ToString();
            itemQuantity.enabled = true;
        }
        else
        {
            itemIcon.enabled = false;
            itemQuantity.enabled = false;
        }
    }
}