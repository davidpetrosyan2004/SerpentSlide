using UnityEngine;
using UnityEngine.Splines;

public class SplinePath : MonoBehaviour
{
    [SerializeField] private SplineContainer spline;
    [SerializeField] private float speed = 0.2f;


    private float t;

    private void Update()
    {
        t += speed * Time.deltaTime;

        Vector3 position = spline.EvaluatePosition(t);
        Vector3 tangent = spline.EvaluateTangent(t);

        transform.position = position;

        if (tangent != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(tangent) * Quaternion.Euler(0, 90, 0);
    }
}
