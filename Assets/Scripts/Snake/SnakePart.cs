using UnityEngine;

public class SnakePart : MonoBehaviour
{
    public SkinnedMeshRenderer partMesh;
    [SerializeField] private ParticleSystem colorChangeEffect;

    public Texture texture { get; set; }
    public Color color { get; set; }
    //private void Awake()
    //{
    //    partMesh.material.renderQueue = 3001;
    //}
    public void SetColor(Texture newTexture)
    {
        texture = newTexture;
        partMesh.material.SetTexture("_BaseMap", newTexture);

        var effect = Instantiate(colorChangeEffect, transform.position, Quaternion.identity, transform);

        var renderer = effect.GetComponent<ParticleSystemRenderer>();
        renderer.material.SetTexture("_MainTex", newTexture);

        effect.Play();
    }
}
