using UnityEngine;

namespace Items
{
    public enum ItemFunctionlity
    {
        Break,
        Attack,
        CraftingItem
    }

    [CreateAssetMenu(menuName = "Items/New Useable Item", fileName = "New Useable Item")]
    public class UseableItem : Item
    {
        [field: SerializeField]
        public ItemFunctionlity ItemUse { get; private set; }
    }
}