using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInput : MonoBehaviour
{
    public Action<Vector2> FingerTapEvent;
    public Action<Vector2> FingerMoveEvent;
    public Action<Vector2> FingerReleaseEvent;

    public bool IsInputEnabled = true;
    [SerializeField] private bool _isMultitouchEnabled = false;

    private void Awake()
    {
        Input.multiTouchEnabled = _isMultitouchEnabled;
    }

    private void Update()
    {
        if (!IsInputEnabled)
        {
            return;
        }

        if (_isMultitouchEnabled && Input.touchCount > 0)
        {
            HandleTouches();
            return;
        }

        HandleMouse();
    }
    private void HandleMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (IsPointerOverUi())
            {
                return;
            }

            OnTap(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0))
        {
            OnMove(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            OnRelease(Input.mousePosition);
        }
    }

    private void HandleTouches()
    {
        Touch touch = Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                if (IsPointerOverUi(touch.fingerId))
                {
                    break;
                }

                OnTap(touch.position);
                break;
            case TouchPhase.Moved:
            case TouchPhase.Stationary:
                OnMove(touch.position);
                break;
            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                OnRelease(touch.position);
                break;
        }
    }
    private void OnTap(Vector2 position)
    {
        FingerTapEvent?.Invoke(position);
    }

    private void OnMove(Vector2 position)
    {
        FingerMoveEvent?.Invoke(position);
    }

    private void OnRelease(Vector2 position)
    {
        FingerReleaseEvent?.Invoke(position);
    }
    private bool IsPointerOverUi(int pointerId)
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(pointerId);
    }

    private bool IsPointerOverUi()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }
}
