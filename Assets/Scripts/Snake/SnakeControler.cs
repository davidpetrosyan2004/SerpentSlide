using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class SnakeControler : MonoBehaviour, ISlidable
{
    [Header("References")]
    [SerializeField] private GridTiles gridTiles;
    [SerializeField] public Snake snake;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private int spacing;

    private Transform slideObject;

    private (Vector3, Vector3Int)? start;
    private (Vector3, Vector3Int)? target;

    private List<Vector3> positionsHistory = new();
    private List<Vector3Int> currentPath = new();
    private int pathIndex = 0;

    private bool isReached = false;
    private bool isMoving = false;
    private bool isKeyMoving = false;
    public bool isDiving { get; set; }

    public Vector3[] DivePositions { get; set; }
    public Gate gate { get; set; }
    private int diveIndex;
    public bool isLinked { get; set; } = false;

    public void OnSlideStart(Collider targetCollider, Vector3 worldPosition)
    {
        if (GameManager.Instance.isTutorial)
        {
            GameManager.Instance.isTutorial = false;
            GameManager.Instance.OnFingerTap?.Invoke();
        }
        if (!GameManager.Instance.isTimerStarted)
        {
            GameManager.Instance.isTimerStarted = true;
            GameManager.Instance.TimerStart?.Invoke();
        }
        if (isDiving || snake.isLocked) return;
        if (!isLinked)
            gridTiles.Tag = targetCollider.GetComponent<SnakePart>().color;
        Debug.Log(gridTiles.Tag);
        gridTiles.SetWalkablesOnBoard();

        positionsHistory.Clear();
        if (targetCollider.CompareTag("Tail"))
        {
            slideObject = targetCollider.transform;
            if (snake.linkedSnake != null)
            {
                Debug.Log("Notifying linked snake about tail slide start");
                snake.linkedSnake.GetComponent<SnakeControler>().OnSlideStart(snake.linkedSnake.GetComponent<Snake>().TailPrefab.GetComponent<Collider>(), worldPosition);
            }

            snake.GetComponent<Snake>().isReversed = true;
            snake.BodyParts.Remove(snake.TailPrefab);
            if (!snake.BodyParts.Contains(snake.HeadPrefab))
            {
                snake.BodyParts.Reverse();
                snake.BodyParts.Add(snake.HeadPrefab);
            }
        }
        else
        {
            slideObject = targetCollider.transform.parent;
            if (snake.linkedSnake != null)
            {
                snake.linkedSnake.GetComponent<SnakeControler>().OnSlideStart(snake.linkedSnake.GetComponent<Snake>().HeadPrefab.transform.GetChild(0).GetComponent<Collider>(), worldPosition);
            }
            snake.GetComponent<Snake>().isReversed = false;
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
        if (slideObject == null)
            return;
        if (snake.linkedSnake != null)
        {
            snake.linkedSnake.GetComponent<SnakeControler>().OnSlide(worldPosition, delta);
        }
        
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
        if (slideObject == null)
            return;

        if (snake.linkedSnake != null)
        {
            snake.linkedSnake.GetComponent<SnakeControler>().OnSlideEnd(worldPosition);
        }

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
        if (snake.isLocked) return;
        if (isDiving)
        {
            if (snake.isKey && !isKeyMoving)
            {
                isKeyMoving = true;
                snake.keyImagePrefab.transform.SetParent(null);
                snake.keyImagePrefab.transform.DOMove(snake.lockSnake.lockImagePrefab.transform.position, 1f).SetEase(Ease.InOutCirc).OnComplete(
                    () =>
                    {
                        snake.lockSnake.isLocked = false;
                        snake.isKey = false;
                        Destroy(snake.keyImagePrefab);
                        Destroy(snake.lockSnake.lockImagePrefab);
                    }
                    );
            }
            if (diveIndex >= DivePositions.Length)
            {
                isDiving = false;
                GameManager.Instance.RemoveGateCount();
                Destroy(gate.gameObject);
                Destroy(gameObject);
                return;
            }

            Vector3 target = DivePositions[diveIndex];

            Vector3 prevPos = slideObject.position;

            slideObject.position = Vector3.MoveTowards(
                slideObject.position,
                target,
                moveSpeed * Time.deltaTime
            );


            if (Vector3.Distance(slideObject.position, target) < 0.01f)
            {
                slideObject.position = target;

                AudioManager.Instance.PlaySound("Dive");
                gate.OnBodyPartDiveEffect();
                AudioManager.Instance.Vibrate();

                UpdatePositionsHistory(prevPos);

                diveIndex++;
            }

            MoveBodyParts();
            return;
        }

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

        Vector3 direction = targetPos - slideObject.position;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation =
    Quaternion.LookRotation(direction) *
    Quaternion.Euler(0, 90, 0);

            slideObject.rotation = targetRotation;
        }

        isMoving = true;

        if (Vector3.Distance(slideObject.position, targetPos) < 0.01f)
        {
            if (!isReached)
            {
                isReached = true;
                OnReachedTarget();
            }

            slideObject.position = targetPos;
            UpdatePositionsHistory(lastPos);

            isMoving = false;
            pathIndex++;
        }
        else
        {
            isReached = false;
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
            Vector3 direction = point - slideObject.position;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation =
        Quaternion.LookRotation(direction) *
        Quaternion.Euler(0, 90, 0);

                bodyPart.rotation = targetRotation;
            }
            if (Vector3.Distance(bodyPart.transform.position, point) < 0.01f)
            {
                bodyPart.transform.position = point;
            }
            index++;
        }
    }

    public void OnReachedTarget()
    {
        if (slideObject == null) return;
        gridTiles.SetWalkablesOnBoard();
        AudioManager.Instance.PlaySound("Move");
        AudioManager.Instance.Vibrate();
    }

    public void UpdatePositionsHistory(Vector3 lastPos)
    {
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
    }

    public bool IsSnakeFilled()
    {
        return snake.coloredBodyPartsCount == 0;
    }
}