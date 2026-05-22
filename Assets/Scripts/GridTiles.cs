using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class GridTiles : MonoBehaviour
{
    [SerializeField] public Tilemap tilemap;
    private Dictionary<Vector3Int, AStarPathfinding.Node> gridMap = new();

    private void Start()
    {
        BoundsInt bounds = tilemap.cellBounds;
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                TileBase tile = tilemap.GetTile(pos);
                bool walkable = tile != null;
                var node = new AStarPathfinding.Node(pos, walkable);
                gridMap[pos] = node;
            }
        }
    }

    public List<Vector3Int> GetPath(Vector3Int start, Vector3Int target)
    {
        return AStarPathfinding.FindPath(gridMap, start, target);
    }

    public (Vector3, Vector3Int)? GetTileWorldPosition(Vector3 worldPos)
    {
        Vector3Int cellIndex = tilemap.WorldToCell(worldPos);
        if (tilemap.HasTile(cellIndex))
        {
            Vector3 cellPosition = tilemap.GetCellCenterWorld(cellIndex);
            cellPosition.y = 0.5f;
            return (cellPosition, cellIndex);
        }
        return null;
    }
}
