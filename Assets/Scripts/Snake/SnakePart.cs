using UnityEngine;

public class SnakePart : MonoBehaviour
{
    public SkinnedMeshRenderer partMesh;
    [SerializeField] private ParticleSystem colorChangeEffect;

    public Color color { get; set; }

    public void SetColor(Color newColor)
    {
        color = newColor;
        partMesh.material.color = newColor;

        var effect = Instantiate(colorChangeEffect, transform.position, Quaternion.identity, transform);

        var renderer = effect.GetComponent<ParticleSystemRenderer>();
        renderer.material.color = newColor;

        effect.Play();
    }
}
