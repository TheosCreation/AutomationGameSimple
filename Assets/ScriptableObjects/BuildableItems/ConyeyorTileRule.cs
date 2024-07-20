using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Custom Tiles/ConveyorTileRule")]
public class ConveyorTileRule : RuleTile<ConveyorTileRule.Neighbor>
{
    public Direction conveyorDirection;

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public class Neighbor : RuleTile.TilingRule.Neighbor
    {
        public const int SameDirection = 3;
        public const int DifferentDirection = 4;
    }

    public override bool RuleMatch(int neighbor, TileBase tile)
    {
        ConveyorTileRule conveyorTile = tile as ConveyorTileRule;
        switch (neighbor)
        {
            case Neighbor.This:
                return tile == null;
            case Neighbor.NotThis:
                return tile != null;
            case Neighbor.SameDirection:
                return conveyorTile != null && conveyorTile.conveyorDirection == this.conveyorDirection;
            case Neighbor.DifferentDirection:
                return conveyorTile != null && conveyorTile.conveyorDirection != this.conveyorDirection;
        }
        return base.RuleMatch(neighbor, tile);
    }
}
