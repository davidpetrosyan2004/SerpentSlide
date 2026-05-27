using UnityEngine;

public class SlideControl : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInputController;
    [SerializeField] private RayCastDetector _inputRaycastDetector;

    private ISlidable _slidable;
    private Vector3 _lastWorldPosition;

    private void Awake()
    {
        _playerInputController = FindAnyObjectByType<PlayerInput>();
        _inputRaycastDetector = FindAnyObjectByType<RayCastDetector>();
    }

    private void OnEnable()
    {
        _playerInputController.FingerTapEvent += OnFingerTap;
        _playerInputController.FingerMoveEvent += OnFingerMove;
        _playerInputController.FingerReleaseEvent += OnFingerRelease;
    }
    private void OnDisable()
    {
        _playerInputController.FingerTapEvent -= OnFingerTap;
        _playerInputController.FingerMoveEvent -= OnFingerMove;
        _playerInputController.FingerReleaseEvent -= OnFingerRelease;
    }

    private void OnFingerTap(Vector2 screenPosition)
    {
        _slidable = null;
        Collider targetCollider = null;
        RaycastHit _ = default;
        if (_inputRaycastDetector.TryGetRaycastTarget(screenPosition, out ISlidable slidable, out _))
        {
            _slidable = slidable;
            targetCollider = _.collider;
            _lastWorldPosition = _inputRaycastDetector.GetWorldPositionOnInputPlane(screenPosition);
            _slidable.OnSlideStart(_.collider,_lastWorldPosition);
        }
    }
    private void OnFingerMove(Vector2 screenPosition)
    {
        if (_slidable == null)
        {
            return;
        }

        Vector3 worldPosition = _inputRaycastDetector.GetWorldPositionOnInputPlane(screenPosition);
        Vector3 delta = worldPosition - _lastWorldPosition;

        _slidable.OnSlide(worldPosition, delta);
        _lastWorldPosition = worldPosition;
    }
    private void OnFingerRelease(Vector2 screenPosition)
    {
        if (_slidable == null)
        {
            return;
        }

        Vector3 worldPosition = _inputRaycastDetector.GetWorldPositionOnInputPlane(screenPosition);

        _slidable.OnSlideEnd(worldPosition);
        _slidable = null;
    }

}
    