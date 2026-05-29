using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridTiles : MonoBehaviour
{
    [SerializeField] public Tilemap tilemap;
    [SerializeField] private Transform enviroment;
    private Dictionary<Vector3Int, AStarPathfinding.Node> gridMap = new();

    public Color Tag { get; set; }


    private void Start()
    {
        Application.targetFrameRate = 60;

        InitBoardTilesNodes();
        SetWalkablesOnBoard();
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

    public void SetWalkablesOnBoard()
    {
        BoundsInt bounds = tilemap.cellBounds;

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);

                TileBase tile = tilemap.GetTile(pos);

                Vector3 worldPos = tilemap.GetCellCenterWorld(pos);
                RaycastHit hit;
                bool isBlocked = false;

                if (Physics.Raycast(
                    worldPos,
                    Vector3.up,
                    out hit,
                    20f
                ))
                {
                    isBlocked = true;
                    if (hit.collider.TryGetComponent<CellObject>(out var cellObject))
                    {
                        if (cellObject.color == Tag)
                        {
                            isBlocked = false;
                        }
                    }
                }
                bool walkable = !isBlocked && tile != null;
                //Debug.DrawRay(worldPos, Vector3.up * 20f, walkable ? Color.green : Color.red,2f);

                gridMap[pos].walkable = walkable;
            }
        }
    }

    public void InitBoardTilesNodes()
    {
        BoundsInt bounds = tilemap.cellBounds;
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                gridMap[pos] = new AStarPathfinding.Node(pos, false);
            }
        }
    }

}
