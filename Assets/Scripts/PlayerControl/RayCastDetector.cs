using UnityEngine;
using UnityEngine.Assertions;

public class RayCastDetector : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _raycastDistance = 1000f;
    [SerializeField] private float _groundHeight = 0f;
    [SerializeField] private LayerMask _raycastMask = Physics.DefaultRaycastLayers;
    [SerializeField] private Vector2 _gizmoPlaneSize = new Vector2(10f, 10f);
    [SerializeField] private Color _planeGizmoColor = new Color(0f, 0.75f, 1f, 0.2f);
    [SerializeField] private Color _raycastHitColor = Color.green;
    [SerializeField] private Color _raycastMissColor = Color.red;
    [SerializeField] private float _raycastDebugDuration = 0.5f;


    private bool _hasRaycastDebug;
    private Vector3 _raycastDebugStart;
    private Vector3 _raycastDebugEnd;
    private Color _raycastDebugColor;
    private float _raycastDebugEndTime;

    private void Awake()
    {
        _camera = FindAnyObjectByType<Camera>();
        Assert.IsNotNull(_camera, "Camera reference is not set on InputRaycastHelper.");
    }

    public bool TryRaycast(Vector2 screenPosition, out RaycastHit hit)
    {
        Ray ray = _camera.ScreenPointToRay(screenPosition);
        bool hasHit = Physics.Raycast(ray, out hit, _raycastDistance, _raycastMask);
        DrawRaycastDebug(ray, hasHit, hit);

        return hasHit;
    }

    public bool TryGetRaycastTarget<T>(Vector2 screenPosition, out T target, out RaycastHit hit)
    {
        target = default;

        if (!TryRaycast(screenPosition, out hit))
        {
            return false;
        }
        //if (!hit.collider.enabled) return false;
        target = hit.collider.GetComponentInParent<T>();
        return target != null;
    }

    public Vector3 GetWorldPositionOnInputPlane(Vector2 screenPosition)
    {
        Ray ray = _camera.ScreenPointToRay(screenPosition);
        Plane inputPlane = new Plane(Vector3.up, new Vector3(0f, _groundHeight, 0f));

        if (inputPlane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }

        return Vector3.zero;
    }

    private void DrawRaycastDebug(Ray ray, bool hasHit, RaycastHit hit)
    {
        Vector3 endPosition = hasHit ? hit.point : ray.GetPoint(_raycastDistance);
        Color color = hasHit ? _raycastHitColor : _raycastMissColor;

        _hasRaycastDebug = true;
        _raycastDebugStart = ray.origin;
        _raycastDebugEnd = endPosition;
        _raycastDebugColor = color;
        _raycastDebugEndTime = Time.realtimeSinceStartup + _raycastDebugDuration;

        Debug.DrawLine(ray.origin, endPosition, color, _raycastDebugDuration);
    }

    public void ClearRaycastDebug()
    {
        _hasRaycastDebug = false;
    }

    private void OnDrawGizmos()
    {
        Vector3 center = new Vector3(0f, _groundHeight, 0f);
        Vector3 size = new Vector3(_gizmoPlaneSize.x, 0.01f, _gizmoPlaneSize.y);

        Gizmos.color = _planeGizmoColor;
        Gizmos.DrawCube(center, size);

        Gizmos.color = new Color(_planeGizmoColor.r, _planeGizmoColor.g, _planeGizmoColor.b, 1f);
        Gizmos.DrawWireCube(center, size);

        if (!_hasRaycastDebug)
        {
            return;
        }

        if (Application.isPlaying && Time.realtimeSinceStartup > _raycastDebugEndTime)
        {
            _hasRaycastDebug = false;
            return;
        }

        Gizmos.color = _raycastDebugColor;
        Gizmos.DrawLine(_raycastDebugStart, _raycastDebugEnd);
        Gizmos.DrawSphere(_raycastDebugEnd, 0.08f);

        Gizmos.color = Color.red;
    }
}
