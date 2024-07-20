using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Items;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<Item> items;

    [SerializeField] private PlayerInteractions playerInteractions;

    private int activeItemIndex = 0;

    private void OnEnable()
    {
        InputManager.Instance.playerInputActions.Game.NextItem.performed += OnNextItemPerformed;
        InputManager.Instance.playerInputActions.Game.PreviousItem.performed += OnPreviousItemPerformed;

        playerInteractions.SetActiveItem(items[activeItemIndex]);
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
        activeItemIndex = (activeItemIndex + 1) % items.Count;
        playerInteractions.SetActiveItem(items[activeItemIndex]);
    }

    private void PreviousItem()
    {
        activeItemIndex = (activeItemIndex - 1 + items.Count) % items.Count;
        playerInteractions.SetActiveItem(items[activeItemIndex]);
    }

    public void AddItem(Item item)
    {
        items.Add(item);
    }
    
    public void RemoveItem(Item item)
    {
        items.Remove(item);
        playerInteractions.SetActiveItem(null);
    }
}