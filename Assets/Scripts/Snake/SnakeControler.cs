//using System.Collections.Generic;
//using UnityEngine;

//public class SnakeControler : MonoBehaviour, ISlidable
//{
//    [Header("References")]
//    [SerializeField] private GridTiles gridTiles;
//    [SerializeField] private float moveSpeed = 3f;
//    [SerializeField] private Snake snake;

//    private (Vector3, Vector3Int)? target;

//    private List<Vector3Int> currentPath = new();

//    private int pathIndex = 0;

//    private bool isMoving = false;

//    private Vector3Int lastTargetTile;
//    private Vector3Int currentHeadTile;

//    public void OnSlideStart(Vector3 worldPosition)
//    {
//        var headTile = GetTargetTileAndPosition(worldPosition);

//        if (headTile.HasValue)
//        {
//            currentHeadTile = headTile.Value.Item2;
//        }
//    }
//    public void OnSlideEnd(Vector3 worldPosition) { }

//    public void OnSlide(Vector3 worldPosition, Vector3 delta)
//    {
//        target = GetTargetTileAndPosition(worldPosition);

//        if (!target.HasValue)
//            return;

//        // если цель не изменилась — ничего не делаем
//        if (target.Value.Item2 == lastTargetTile)
//        {
//            return;
//        }

//        lastTargetTile = target.Value.Item2;

//        List<Vector3Int> newPath =
//            gridTiles.GetPath(currentHeadTile, target.Value.Item2);

//        if (newPath != null && newPath.Count > 0)
//        {
//            currentPath = newPath;
//            pathIndex = 0;
//            isMoving = true;
//        }
//    }

//    private void Update()
//    {
//        if (!isMoving)
//            return;

//        if (currentPath == null || currentPath.Count == 0)
//            return;

//        if (pathIndex >= currentPath.Count)
//        {
//            isMoving = false;
//            return;
//        }

//        Vector3 targetPos =
//            gridTiles.tilemap.GetCellCenterWorld(currentPath[pathIndex]);

//        targetPos.y = 0.5f;

//        snake.HeadPrefab.position =
//            Vector3.MoveTowards(
//                snake.HeadPrefab.position,
//                targetPos,
//                moveSpeed * Time.deltaTime);

//        if (Vector3.Distance(snake.HeadPrefab.position, targetPos) < 0.01f)
//        {
//            currentHeadTile = currentPath[pathIndex];

//            pathIndex++;

//            if (pathIndex >= currentPath.Count)
//            {
//                isMoving = false;
//            }
//        }
//    }


//    public (Vector3, Vector3Int)? GetTargetTileAndPosition(Vector3 worldPosition)
//    {
//        return gridTiles.GetTileWorldPosition(worldPosition);
//    }
//}

using System.Collections.Generic;
using UnityEngine;

public class SnakeControler : MonoBehaviour, ISlidable
{
    [Header("References")]
    [SerializeField] private GridTiles gridTiles;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private Snake snake;

    private (Vector3, Vector3Int)? start;
    private (Vector3, Vector3Int)? target;

    private List<Vector3Int> currentPath = new();
    private int pathIndex = 0;
    private int pathIndex2 = 0;

    private bool isMoving = false;
    private bool isSlideOver = false;

    //private void Start()
    //{
    //    var t = GetTargetTileAndPosition(snake.HeadPrefab.position);
    //}

    public void OnSlideStart(Vector3 worldPosition)
    {
        start = GetTargetTileAndPosition(worldPosition);
        isSlideOver = false;
    }

    public void OnSlide(Vector3 worldPosition, Vector3 delta)
    {
        target = GetTargetTileAndPosition(worldPosition);

        if (!start.HasValue || !target.HasValue)
            return;

        // если path пустой -> создаем
        if (currentPath.Count == 0)
        {
            currentPath = gridTiles.GetPath(start.Value.Item2, target.Value.Item2);

            if (currentPath != null && currentPath.Count > 0)
            {
                pathIndex2 = 0;
            }
        }

    }
    public void OnSlideEnd(Vector3 worldPosition)
    {
        isSlideOver = true;
        target = GetTargetTileAndPosition(worldPosition);
        start = GetTargetTileAndPosition(snake.HeadPrefab.position);
        if (start.HasValue && target.HasValue)
        {
            var path = gridTiles.GetPath(start.Value.Item2, target.Value.Item2);
            if (path != null && path.Count > 0)
            {
                // convert tile indices to world centers
                currentPath.Clear();
                foreach (var cell in path)
                {
                    currentPath.Add(cell);
                }

                // start moving from current head position toward first waypoint
                pathIndex = 0;
                isMoving = true;
            }
            else
            {
                isMoving = false;
            }
        }

    }
    private void LateUpdate()
    {
        if (!isMoving || currentPath.Count == 0 || !isSlideOver)
        {

            // двигаемся по path
            if (currentPath != null && pathIndex2 < currentPath.Count)
            {
                Vector3 cellPos =
                    gridTiles.tilemap.GetCellCenterWorld(currentPath[pathIndex2]);

                cellPos.y = 0.5f;

                snake.HeadPrefab.position =
                    Vector3.MoveTowards(
                        snake.HeadPrefab.position,
                        cellPos,
                        moveSpeed * Time.deltaTime);

                // дошли до клетки
                if (Vector3.Distance(snake.HeadPrefab.position, cellPos) < 0.01f)
                {
                    start = (cellPos, currentPath[pathIndex2]);

                    pathIndex2++;

                    // path закончился
                    if (pathIndex2 >= currentPath.Count)
                    {
                        currentPath.Clear();
                        pathIndex2 = 0;
                    }
                }
            }
        }

        else
        {
            // If head is not at the current waypoint, move toward it
            Vector3 currentTarget = gridTiles.tilemap.GetCellCenterWorld(currentPath[pathIndex]);
            currentTarget.y = 0.5f; // Ensure target is at the correct height

            Transform head = snake.HeadPrefab;
            head.position = Vector3.MoveTowards(head.position, currentTarget, moveSpeed * Time.deltaTime);

            // If reached waypoint, advance
            if (Vector3.Distance(head.position, currentTarget) < 0.01f)
            {
                start = (currentTarget, currentPath[pathIndex]);
                pathIndex++;
                if (pathIndex >= currentPath.Count)
                {
                    isMoving = false;
                    //pathIndex = 0;
                    currentPath.Clear();
                }
            }
        }
    }


    public (Vector3, Vector3Int)? GetTargetTileAndPosition(Vector3 worldPosition)
    {
        return gridTiles.GetTileWorldPosition(worldPosition);
    }
}
