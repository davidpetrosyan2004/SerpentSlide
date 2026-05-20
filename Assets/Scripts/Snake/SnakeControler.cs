using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
public class SnakeControler : MonoBehaviour, ISlidable
{
    [Header("References")]
    [SerializeField] private GridTiles gridTiles; 

    [SerializeField] private float smoothSpeed;
    private Vector3 targetTilePosition;
    private Vector3Int targetTileIndex;
    private (Vector3, Vector3Int)? target;

    private bool isTapped;
    private bool isSlideOver;

    public void OnSlideStart(Vector3 worldPosition)
    {
        target = gridTiles.GetTileWorldPosition(worldPosition);
        if (target != null)
        {
            targetTilePosition = target.Value.Item1;
            targetTileIndex = target.Value.Item2;
        }
        isTapped = true;
        isSlideOver = false;
    }


    public void OnSlide(Vector3 worldPosition, Vector3 delta)
    {
        target = gridTiles.GetTileWorldPosition(worldPosition);
        if (target != null)
        {
            targetTilePosition = target.Value.Item1;
            targetTileIndex = target.Value.Item2;
        } 
    }

    public void OnSlideEnd(Vector3 worldPosition)
    {
        target = gridTiles.GetTileWorldPosition(worldPosition);
        if (target != null)
        {
            targetTilePosition = target.Value.Item1;
            targetTileIndex = target.Value.Item2;
        }
        isSlideOver = true;
    }
    private void Update()
    {
        if (isTapped)
        {
            transform.position = Vector3.Lerp(transform.position, targetTilePosition, Time.deltaTime * smoothSpeed);
            if (Vector3.Distance(transform.position, targetTilePosition) < 0.01f && isSlideOver)
            {
                transform.position = targetTilePosition;
                isTapped = false;
            }
        }
    }
}
