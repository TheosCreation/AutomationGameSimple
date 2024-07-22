using UnityEngine;
using GameInput;
using UnityEngine.InputSystem;
using Items;
using BuildingSystem;
using ResourceSystem;

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
    private ResourceLayer resourceLayer;

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


    private float useTimer = 0;
    private float dropTimer = 0;
    public float dropTime = 0.2f;

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

        if (useTimer > 0)
        {
            useTimer -= Time.deltaTime;
        }
        
        if (dropTimer > 0)
        {
            dropTimer -= Time.deltaTime;
        }

        if (!isSelecting) return;
        
        // We were sucessfully able to build we return
        if (TryBuild()) return;

        if (useTimer <= 0)
        {
            if (TryUseItem()) return;
        }

        if(dropTimer <= 0)
        {
            if (TryDropItem()) return;
        }
    }

    bool TryBuild()
    {
        if (gridMouseCellPos == gridCellPos) return false;

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
            Debug.Log("Used item");
            useTimer = useableItem.UseCooldown;

            //play swing animation

            if (!IsMouseWithinRange()) return false;

            if (useableItem.ItemUse == ItemFunctionlity.Break)
            {
                if(!constructionLayer.IsEmpty(mousePos))
                {
                    constructionLayer.DestroyBuild(mousePos);
                    return true;
                }
                
                if(!resourceLayer.IsEmpty(mousePos))
                {
                    resourceLayer.MineResource(player, mousePos);
                    return true;
                }
            }
            else if(useableItem.ItemUse == ItemFunctionlity.CraftingItem)
            {
                if(useableItem.GameObject == null)
                {
                    Debug.LogError("Crafting Item doesn't have gameobject set");
                    return false;
                }

                DropHeldItem();
            }
            else if(useableItem.ItemUse == ItemFunctionlity.Consume)
            {
                if(resourceLayer.CollectResource(player, mousePos))
                {
                    playerInventory.RemoveItem(ActiveItem);
                    return true;
                }
            }
            return true;
        }

        return false;
    }
    
    bool TryDropItem()
    {
        if (gridMouseCellPos == gridCellPos) return false;

        if (ActiveItem is Resource resourceItem)
        {
            if (!IsMouseWithinRange()) return false;

            if(resourceItem.GameObject == null)
            {
                Debug.LogError("Crafting Item doesn't have gameobject set");
                return false;
            }

            DropHeldItem();
            dropTimer = dropTime;
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

    public void DropHeldItem()
    {
        GameObject itemObject = Instantiate(
            ActiveItem.GameObject,
            grid.LocalToWorld(gridMouseCellPos) + new Vector3(0.5f, 0.5f, 0),
            Quaternion.identity,
            parentToAttachItems.transform
            );

        ActiveItem = playerInventory.RemoveItem(ActiveItem);
    }

    public void SetActiveItem(Item item)
    {
        ActiveItem = item;
        UiManager.Instance.SetSelectedItem(item);
    }
}