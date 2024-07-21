using Items;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ResourceSystem
{
    [Serializable]
    public class PlaceableResource
    {
        [field: SerializeField]
        public Tilemap ParentTilemap { get; private set; }

        [field: SerializeField]
        public Resource PlaceableType { get; private set; }

        [field: SerializeField]
        public Vector3Int Coordinates { get; private set; }

        public PlaceableResource(Resource type, Vector3Int coords, Tilemap tilemap, GameObject gameObject = null)
        {
            ParentTilemap = tilemap;
            PlaceableType = type;
            Coordinates = coords;
        }

        public void Destroy()
        {
            ParentTilemap.SetTile(Coordinates, null);
        }
    }
}