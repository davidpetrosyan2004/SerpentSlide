using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridTiles : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;

    public (Vector3,Vector3Int)? GetTileWorldPosition(Vector3 screenPos)
    {
        Vector3Int cellIndex = tilemap.WorldToCell(screenPos);
        if (tilemap.HasTile(cellIndex))
        {
            Vector3 cellPosition = tilemap.GetCellCenterWorld(cellIndex);

            cellPosition.y = 0.5f;
            return (cellPosition, cellIndex);
        }
        return null;
    }
}
