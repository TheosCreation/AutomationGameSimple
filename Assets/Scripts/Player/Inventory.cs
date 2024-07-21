using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Items;

[System.Serializable]
public class InventoryItem
{
    public Item item;
    public int quantity;
}

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<InventoryItem> itemsList;

    private Dictionary<Item, int> items;
    [SerializeField] private PlayerInteractions playerInteractions;

    private int activeItemIndex = 0;
    private List<Item> itemKeys;

    private void OnEnable()
    {
        InputManager.Instance.playerInputActions.Game.NextItem.performed += OnNextItemPerformed;
        InputManager.Instance.playerInputActions.Game.PreviousItem.performed += OnPreviousItemPerformed;

        items = new Dictionary<Item, int>();
        foreach (var inventoryItem in itemsList)
        {
            items[inventoryItem.item] = inventoryItem.quantity;
        }

        itemKeys = new List<Item>(items.Keys);
        if (itemKeys.Count > 0)
        {
            playerInteractions.SetActiveItem(itemKeys[activeItemIndex]);
        }
        UiManager.Instance.UpdateHotbar(items);
    }

    private void OnNextItemPerformed(InputAction.CallbackContext ctx)
    {
        NextItem();
    }

    private void OnPreviousItemPerformed(InputAction.CallbackContext ctx)
    {
        PreviousItem();
    }

    private void NextItem()
    {
        if (itemKeys.Count == 0) return;

        activeItemIndex = (activeItemIndex + 1) % itemKeys.Count;
        playerInteractions.SetActiveItem(itemKeys[activeItemIndex]);
        UiManager.Instance.UpdateHotbar(items);
    }

    private void PreviousItem()
    {
        if (itemKeys.Count == 0) return;

        activeItemIndex = (activeItemIndex - 1 + itemKeys.Count) % itemKeys.Count;
        playerInteractions.SetActiveItem(itemKeys[activeItemIndex]);
        UiManager.Instance.UpdateHotbar(items);
    }

    public void SelectItem(int slotCount)
    {
        if (slotCount < 0 || slotCount >= itemKeys.Count) return;

        activeItemIndex = slotCount;
        playerInteractions.SetActiveItem(itemKeys[activeItemIndex]);
        UiManager.Instance.UpdateHotbar(items);
    }

    public void AddItem(Item item)
    {
        if (items.ContainsKey(item))
        {
            items[item]++;
        }
        else
        {
            items[item] = 1;
            itemKeys.Add(item);
        }
        UiManager.Instance.UpdateHotbar(items);
    }

    public Item RemoveItem(Item item)
    {
        if (items.ContainsKey(item))
        {
            items[item]--;
            if (items[item] <= 0)
            {
                items.Remove(item);
                itemKeys.Remove(item);
                playerInteractions.SetActiveItem(null);
                UiManager.Instance.UpdateHotbar(items);
            }
            else
            {
                UiManager.Instance.UpdateHotbar(items);
                return item;
            }
        }
        return null;
    }
}
