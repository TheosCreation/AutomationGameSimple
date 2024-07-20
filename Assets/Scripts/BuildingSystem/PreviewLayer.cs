using UnityEngine;
using Items;

namespace BuildingSystem
{
    public class PreviewLayer : TilemapLayer
    {
        [SerializeField] private SpriteRenderer previewRenderer;

        public void ShowPreview(Item item, Vector3 worldCoords, Vector3 direction, bool isValid)
        {
            //snap to grid and draw
            var coords = _tilemap.WorldToCell(worldCoords);
            previewRenderer.enabled = true;
            previewRenderer.transform.position =
                _tilemap.CellToWorld(coords) + 
                _tilemap.cellSize / 2 +
                item.TileOffset;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            previewRenderer.transform.rotation = Quaternion.Euler(0, 0, angle);

            previewRenderer.sprite = item.PreviewSprite;
            previewRenderer.color = isValid ? new Color(0, 1, 0, 0.5f) : new Color(1, 0, 0, 0.5f);
        }

        public void ClearPreview()
        {
            previewRenderer.enabled = false;
        }
    }
}