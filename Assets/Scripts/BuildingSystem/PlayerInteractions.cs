using UnityEngine;
using GameInput;
using UnityEngine.InputSystem;
using Items;
using BuildingSystem;

public enum Rotation
{
    Right,
    Up
}

public class PlayerInteractions : MonoBehaviour
{
    [field: SerializeField]
    public Item ActiveItem { get; private set; }

    [field: SerializeField]
    public Grid grid { get; private set; }

    [SerializeField] private float maxInteractDistance = 1.5f;

    public Transform parentToAttachItems;

    [SerializeField]
    private float maxBuildDistance = 3f;

    [SerializeField]
    private ConstructionLayer constructionLayer;

    [SerializeField]
    private PreviewLayer previewLayer;

    [SerializeField]
    private PlayerMouse playerMouse;

    [SerializeField]
    private PlayerController player;

    [SerializeField]
    private Inventory playerInventory;

    private bool isSelecting = false;

    private Vector2 mousePos;
    private Vector3Int gridMouseCellPos;
    private Vector3Int gridCellPos;

    private Vector3[] rotationDirections = { Vector3.up, Vector3.right, Vector3.down, Vector3.left };
    private int currentDirectionIndex = 1;
    private Vector3 currentRotationSelected;

    private void Start()
    {
        player = GetComponent<PlayerController>();
        playerMouse = GetComponent<PlayerMouse>();

        InputManager.Instance.playerInputActions.Game.Select.started += Select;
        InputManager.Instance.playerInputActions.Game.Select.canceled += EndSelect;
        InputManager.Instance.playerInputActions.Game.Rotate.started += Rotate;

        currentRotationSelected = rotationDirections[currentDirectionIndex];
    }

    private void Update()
    {
        
        mousePos = playerMouse.MouseInWorldPosition;
        gridMouseCellPos = grid.WorldToCell(mousePos);

        gridCellPos = grid.WorldToCell(transform.position);

        if(IsActiveItemPreviewAble() && gridMouseCellPos != gridCellPos)
        {
            previewLayer.ShowPreview(
                ActiveItem,
                mousePos,
                currentRotationSelected,
                constructionLayer.IsEmpty(mousePos) && gridMouseCellPos != gridCellPos && IsMouseWithinBuildRange()
                );
        }
        else
        {
            previewLayer.ClearPreview();
        }
        

        if (!isSelecting) return;
        if (gridMouseCellPos == gridCellPos) return;
        
        // We were sucessfully able to build we return
        if (TryBuild()) return;
        if (TryUseItem()) return;
    }

    bool TryBuild()
    {

        if (ActiveItem is BuildableItem buildableItem)
        {
            if (!constructionLayer.IsEmpty(mousePos) || !IsMouseWithinBuildRange()) return false;

            constructionLayer.Build(mousePos, buildableItem, currentRotationSelected);
            return true;
        }

        return false;
    }

    bool TryUseItem()
    {
        if (ActiveItem is UseableItem useableItem)
        {
            if (!IsMouseWithinRange()) return false;

            if (useableItem.ItemUse == ItemFunctionlity.Break)
            {
                if(constructionLayer.IsEmpty(mousePos)) return false;
                constructionLayer.DestroyBuild(mousePos);
            }
            else if(useableItem.ItemUse == ItemFunctionlity.CraftingItem)
            {
                if(useableItem.GameObject == null)
                {
                    Debug.LogError("Crafting Item doesn't have gameobject set");
                    return false;
                }
                GameObject itemObject = Instantiate(
                    useableItem.GameObject, 
                    grid.LocalToWorld(gridMouseCellPos) + useableItem.TileOffset,
                    Quaternion.identity,
                    parentToAttachItems.transform
                    );

                playerInventory.RemoveItem(ActiveItem);
                ActiveItem = null;
            }
            return true;
        }

        return false;
    }

    private void Rotate(InputAction.CallbackContext ctx)
    {
        currentDirectionIndex = (currentDirectionIndex + 1) % rotationDirections.Length;
        currentRotationSelected = rotationDirections[currentDirectionIndex];
    }

    private void Select(InputAction.CallbackContext ctx)
    {
        if(ActiveItem == null) return;
        if(constructionLayer == null) return;

        isSelecting = true;
    }
    
    private void EndSelect(InputAction.CallbackContext ctx)
    {
        isSelecting = false;
    }

    private bool IsMouseWithinRange()
    {
        return Vector3.Distance(gridMouseCellPos,
            gridCellPos) <= maxInteractDistance;
    }

    private bool IsActiveItemPreviewAble()
    {
        if(ActiveItem == null) return false;
        if(ActiveItem.CanPreview) return true;
        return false;
    }

    private bool IsMouseWithinBuildRange()
    {
        return Vector3.Distance(gridMouseCellPos,
            gridCellPos) <= maxBuildDistance;
    }

    public void SetActiveItem(Item item)
    {
        ActiveItem = item;
        UiManager.Instance.SetSelectedItem(item);
    }
}