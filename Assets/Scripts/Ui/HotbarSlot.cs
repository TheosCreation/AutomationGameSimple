using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Items;

public class HotbarSlot : MonoBehaviour
{
    public Image itemIcon;
    public TextMeshProUGUI itemQuantity;
    public int slotCount = 0;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(HotBarClicked);
    }

    private void HotBarClicked()
    {
        PlayerController.Instance.inventory.SelectItem(slotCount);
    }

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