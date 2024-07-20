using UnityEngine;
using UnityEngine.Tilemaps;

namespace Items
{
    public class Item : ScriptableObject
    {
        [field: SerializeField]
        public string Name { get; private set; }

        [field: SerializeField]
        public TileBase Tile { get; private set; }

        [field: SerializeField]
        public Vector3 TileOffset { get; private set; }

        [field: SerializeField]
        public Sprite PreviewSprite { get; private set; }

        [field: SerializeField]
        public GameObject GameObject { get; private set; }

        [field: SerializeField]
        public bool CanPreview { get; private set; }
    }
}