using UnityEngine;

public interface ISlidable
{
    void OnSlideStart(Vector3 worldPosition);
    void OnSlide(Vector3 worldPosition, Vector3 delta);
    void OnSlideEnd(Vector3 worldPosition);
}