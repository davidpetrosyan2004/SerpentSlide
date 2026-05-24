using DG.Tweening;
using Mono.Cecil;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SnakeControler : MonoBehaviour, ISlidable
{
    [Header("References")]
    [SerializeField] private GridTiles gridTiles;
    [SerializeField] private Snake snake;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private int spacing;

    private Transform slideObject;

    private (Vector3, Vector3Int)? start;
    private (Vector3, Vector3Int)? target;

    private List<Vector3> positionsHistory = new();
    private List<Vector3Int> currentPath = new();
    private int pathIndex = 0;
    private bool isMoving = false;

    public void OnSlideStart(Collider targetCollider, Vector3 worldPosition)
    {
        slideObject = targetCollider.transform;
        positionsHistory.Clear();
        if (targetCollider.CompareTag("Tail"))
        {
            snake.BodyParts.Remove(snake.TailPrefab);
            if (!snake.BodyParts.Contains(snake.HeadPrefab))
            {
                snake.BodyParts.Reverse();
                snake.BodyParts.Add(snake.HeadPrefab);
            }
        }
        else
        {
            snake.BodyParts.Remove(snake.HeadPrefab);
            if (!snake.BodyParts.Contains(snake.TailPrefab))
            {
                snake.BodyParts.Reverse();
                snake.BodyParts.Add(snake.TailPrefab);
            }

        }
    }

    public void OnSlide(Vector3 worldPosition, Vector3 delta)
    {

        start = GetTargetTileAndPosition(slideObject.position);
        target = GetTargetTileAndPosition(worldPosition);

        if (!start.HasValue || !target.HasValue) return;

        if (start.Value.Item2 == target.Value.Item2) return;

        if (currentPath.Count == 0)
        {
            var path = gridTiles.GetPath(
            start.Value.Item2,
            target.Value.Item2);
            if (path != null)
                currentPath = path;

            pathIndex = 0;
        }
    }

    public void OnSlideEnd(Vector3 worldPosition)
    {
        start = GetTargetTileAndPosition(slideObject.position);
        target = GetTargetTileAndPosition(worldPosition);

        if (!start.HasValue || !target.HasValue) return;

        if (!isMoving)
        {
            var path = gridTiles.GetPath(
            start.Value.Item2,
            target.Value.Item2);
            if (path != null)
                currentPath = path;

            pathIndex = 0;
        }
    }

    private void Update()
    {
        if (slideObject == null)
            return;

        if (currentPath == null || currentPath.Count <= 0)
            return;

        Vector3 targetPos =
            gridTiles.tilemap.GetCellCenterWorld(currentPath[pathIndex]);

        targetPos.y = slideObject.position.y;

        var lastPos = slideObject.position;

        slideObject.position = Vector3.MoveTowards(
            slideObject.position,
            targetPos,
            moveSpeed * Time.deltaTime
        );
        isMoving = true;
        if (Vector3.Distance(slideObject.position, targetPos) < 0.01f)
        {
            gridTiles.SetWalkablesOnBoard();

            slideObject.position = targetPos;
            positionsHistory.Insert(0, lastPos); // Add current position to history

            for (var i = 0; i < snake.BodyParts.Count - 1; i++)
            {
                var bodyPart = snake.BodyParts[i];
                positionsHistory.Insert(i + 1, bodyPart.position);
                if (positionsHistory.Count > snake.BodyParts.Count)
                {
                    positionsHistory.RemoveAt(positionsHistory.Count - 1);
                }
            }
            if (positionsHistory.Count > snake.BodyParts.Count)
            {
                positionsHistory.RemoveAt(positionsHistory.Count - 1);
            }
            isMoving = false;
            pathIndex++;
        }

        if (pathIndex >= currentPath.Count)
        {
            currentPath.Clear();
            pathIndex = 0;
        }

        if (currentPath.Count > 0 && positionsHistory.Count > 0)
        {
            //Debug.Log($"Current path count: {currentPath.Count}, Positions history count: {positionsHistory.Count}");
            MoveBodyParts();
        }
    }

    public (Vector3, Vector3Int)? GetTargetTileAndPosition(Vector3 worldPosition)
    {
        return gridTiles.GetTileWorldPosition(worldPosition);
    }

    private void MoveBodyParts()
    {
        //Debug.Log("Body Count: " + snake.BodyParts.Count);
        int index = 0;
        foreach (var bodyPart in snake.BodyParts)
        {
            Vector3 point = positionsHistory[Mathf.Min(index * spacing, positionsHistory.Count - 1)];
            bodyPart.transform.position= Vector3.MoveTowards(bodyPart.transform.position, point, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(bodyPart.transform.position, point) < 0.01f)
            {
                bodyPart.transform.position = point;
            }
            index++;
        }
    }
}