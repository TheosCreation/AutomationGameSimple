using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Items;

namespace BuildingSystem
{
    public class ConstructionLayer : TilemapLayer
    {
        private Dictionary<Vector3Int, Buildable> buildables = new();
        public Transform parentToAttachObjects;
        public void Build(Vector2 worldCoords, BuildableItem item, Vector3 direction)
        {
            GameObject itemObject = null;

            var coords = _tilemap.WorldToCell(worldCoords);
            float angle = 0;
            if (item.Tile != null)
            {
                // Calculate the angle based on the direction
                angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                // Create a rotation matrix
                Matrix4x4 rotationMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 0, angle));

                // Combine the rotation and translation matrices
                Matrix4x4 transformationMatrix = rotationMatrix * Matrix4x4.Translate(item.TileOffset);

                var tileChangeData = new TileChangeData(
                    coords,
                    item.Tile,
                    Color.white,
                    transformationMatrix
                );

                _tilemap.SetTile(tileChangeData, false);
            }

            if (item.GameObject != null)
            {
                itemObject = Instantiate(
                    item.GameObject,
                    _tilemap.CellToWorld(coords) + new Vector3(0.5f, 0.5f, 0f),
                    Quaternion.Euler(0, 0, angle),
                    parentToAttachObjects.transform
                    );

                TileInfo tileInfo = itemObject.GetComponent<TileInfo>();
                if (tileInfo != null)
                {
                    tileInfo.worldCoords = worldCoords;
                    tileInfo.direction = direction;
                }
            }

            var buildable = new Buildable(item, coords, _tilemap, itemObject);
            buildables.Add(coords, buildable);
        }

        public void DestroyBuild(Vector2 worldCoords)
        {
            var coords = _tilemap.WorldToCell(worldCoords);

            // Check if there is a buildable at the coordinates
            if (buildables.ContainsKey(coords))
            {
                buildables[coords].Destroy();

                // Remove the buildable from the dictionary
                buildables.Remove(coords);
            }
        }

        public bool IsEmpty(Vector3 worldCoords)
        {
            var coords = _tilemap.WorldToCell(worldCoords);
            return !buildables.ContainsKey(coords) && _tilemap.GetTile(coords) == null;
        }
    }
}