using System.Collections.Generic;
using UnityEngine;

public class SnakeControler : MonoBehaviour, ISlidable
{
    [Header("References")]
    [SerializeField] private GridTiles gridTiles;
    [SerializeField] private Snake snake;
    [SerializeField] private float moveSpeed = 3f;

    private (Vector3, Vector3Int)? start;
    private (Vector3, Vector3Int)? target;

    private List<Vector3Int> currentPath = new();
    private int pathIndex = 0;
    private int pathIndex2 = 0;

    private bool isSlideOver = false;

    private List<Vector3Int> positionHistory = new();

    [SerializeField] private int spacing = 1;
    private int maxHistory => (snake.BodyParts.Count + 1) * spacing;
    private Transform slideObject;

    public void OnSlideStart(Collider targetCollider, Vector3 worldPosition)
    {
        Debug.Log("Slide Start");
        if (targetCollider.CompareTag("Head"))
        {
            Debug.Log("Head");
            slideObject = snake.HeadPrefab;
            snake.BodyParts.Add(snake.TailPrefab);
        }
        else if(targetCollider.CompareTag("Tail"))
        {
            Debug.Log("Tail");
            slideObject = snake.TailPrefab;
            snake.BodyParts.Add(snake.HeadPrefab);
        }
        start = GetTargetTileAndPosition(worldPosition);
        isSlideOver = false;
    }

    public void OnSlide(Vector3 worldPosition, Vector3 delta)
    {
        var t = GetTargetTileAndPosition(worldPosition);
        if (t != null) target = t;

        if (!start.HasValue || !target.HasValue) return;

        if (Vector3.Distance(slideObject.position, target.Value.Item1) < 0.01f)
            return;

        if (currentPath.Count == 0)
        {
            var path = gridTiles.GetPath(start.Value.Item2, target.Value.Item2); // GetPath
            if (path != null && path.Count > 0)
            {
                currentPath = path;
                pathIndex2 = 0;
            }
        }
    }

    public void OnSlideEnd(Vector3 worldPosition)
    {
        isSlideOver = true;

        var t = GetTargetTileAndPosition(worldPosition);
        if (t != null) target = t;
        start = GetTargetTileAndPosition(slideObject.position);

        if (!start.HasValue || !target.HasValue) return;

        if (Vector3.Distance(slideObject.position, target.Value.Item1) < 0.01f)
            return;


        var path = gridTiles.GetPath(start.Value.Item2, target.Value.Item2); // GetPath
        if (path != null && path.Count > 0)
        {
            currentPath.Clear();
            currentPath.AddRange(path);
            pathIndex = 0;
        }
    }

    private void Update()
    {
        // ✅ Записываем позицию головы КАЖДЫЙ КАДР
        if (positionHistory.Count > maxHistory)
            positionHistory.RemoveAt(positionHistory.Count - 1);


        if (currentPath.Count == 0 || !isSlideOver)
        {
            // Движение во время слайда
            if (pathIndex2 < currentPath.Count)
            {
                Vector3 cellPos = gridTiles.tilemap.GetCellCenterWorld(currentPath[pathIndex2]);
                cellPos.y = 0.5f;

                slideObject.position = Vector3.MoveTowards(
                    slideObject.position, cellPos, moveSpeed * Time.deltaTime);

                if (Vector3.Distance(slideObject.position, cellPos) < 0.01f)
                {
                    positionHistory.Insert(0, GetTargetTileAndPosition(slideObject.position).Value.Item2);

                    slideObject.position = cellPos;
                    start = (cellPos, currentPath[pathIndex2]);
                    pathIndex2++;

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
            // Движение после отпускания пальца
            Vector3 currentTarget = gridTiles.tilemap.GetCellCenterWorld(currentPath[pathIndex]);
            currentTarget.y = 0.5f;

            Transform head = slideObject;
            head.position = Vector3.MoveTowards(head.position, currentTarget, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(head.position, currentTarget) < 0.01f)
            {
                spacing = 1;
                positionHistory.Insert(0, GetTargetTileAndPosition(slideObject.position).Value.Item2);

                head.position = currentTarget;
                start = (currentTarget, currentPath[pathIndex]);
                pathIndex++;

                if (pathIndex >= currentPath.Count)
                {
                    currentPath.Clear();
                }
            }
        }
        if (currentPath.Count > 0 && positionHistory.Count > 0)
        {
            MoveBodyParts();
        }

    }

    public (Vector3, Vector3Int)? GetTargetTileAndPosition(Vector3 worldPosition)
    {
        return gridTiles.GetTileWorldPosition(worldPosition);
    }

    private void MoveBodyParts()
    {
        /// MoveBodyParts теперь просто читает позиции из истории и двигает части тела к ним с помощью DOTween.
        int index = 0;
        foreach (var bodyPart in snake.BodyParts)
        {
            var targetPos = gridTiles.tilemap.GetCellCenterWorld(positionHistory[Mathf.Min(index * spacing, positionHistory.Count - 1)]);
            // Implementation for moving body parts
            targetPos.y = 0.5f; // фиксируем высоту
            bodyPart.position = Vector3.MoveTowards(
                bodyPart.position,
                targetPos,
                moveSpeed * Time.deltaTime);
            if (Vector3.Distance(bodyPart.position, targetPos) < 0.01f)
            {
                bodyPart.position = targetPos;
            }
            index++;
        }
    }
}