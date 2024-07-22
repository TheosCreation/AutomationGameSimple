using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Items;

namespace ResourceSystem
{
    public class ResourceLayer : TilemapLayer
    {
        private Dictionary<Vector3Int, PlaceableResource> resources = new();
        public Transform parentToAttachObjects;
        public void PlaceResource(Vector2 worldCoords, Resource item, Vector3 direction)
        {
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

            var resource = new PlaceableResource(item, coords, _tilemap);
            resources.Add(coords, resource);
        }

        public void MineResource(PlayerController playerToGive, Vector3 worldCoords)
        {
            if(IsMineable(worldCoords))
            {
                var coords = _tilemap.WorldToCell(worldCoords);
                Item itemToGive = resources[coords].PlaceableType;
                playerToGive.inventory.AddItem(itemToGive);
            }
        }

        public bool CollectResource(PlayerController playerToGive, Vector3 worldCoords)
        {
            if(IsCollectable(worldCoords))
            {
                var coords = _tilemap.WorldToCell(worldCoords);
                Item itemToGive = resources[coords].PlaceableType;
                playerToGive.inventory.AddItem(itemToGive);
                return true;
            }
            return false;
        }

        public void DestroyResource(Vector2 worldCoords)
        {
            var coords = _tilemap.WorldToCell(worldCoords);

            // Check if there is a resource at the coordinates
            if (resources.ContainsKey(coords))
            {
                resources[coords].Destroy();

                // Remove the resource from the dictionary
                resources.Remove(coords);
            }
        }
        public bool IsMineable(Vector3 worldCoords)
        {
            Resource resource = GetResourceAtWorldPosition(worldCoords);
            if(resource == null) return false;

            if (resource.Name == "Stone") return true;
            if (resource.Name == "Wood") return true;
            return false;
        }
        
        public bool IsCollectable(Vector3 worldCoords)
        {
            Resource resource = GetResourceAtWorldPosition(worldCoords);
            if(resource == null) return false;

            if (resource.Name == "Water") return true;
            return false;
        }

        public Resource GetResourceAtWorldPosition(Vector3 worldCoords)
        {
            if (IsEmpty(worldCoords)) return null;

            var coords = _tilemap.WorldToCell(worldCoords);
            return resources[coords].PlaceableType;
        }
        public bool IsEmpty(Vector3 worldCoords)
        {
            var coords = _tilemap.WorldToCell(worldCoords);
            return !resources.ContainsKey(coords) && _tilemap.GetTile(coords) == null;
        }
    }
}