using UnityEngine;

public class BodyPart : MonoBehaviour
{
    [SerializeField] private ParticleSystem colorChangeEffect;
    [SerializeField] private MeshRenderer meshRenderer;
    public int index { get; set; }
    public Color color { get; set; }

    public void SetColor(Color newColor)
    {
        color = newColor;
        meshRenderer.material.color = color;

        var effect = Instantiate(colorChangeEffect, transform.position, Quaternion.identity, transform);

        var renderer = effect.GetComponent<ParticleSystemRenderer>();
        renderer.material.color = color;

        effect.Play();
    }
}
