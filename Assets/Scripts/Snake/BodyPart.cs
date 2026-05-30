using NUnit.Framework.Constraints;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    [SerializeField] private ParticleSystem colorChangeEffect;
    [SerializeField] private GameObject straight;
    [SerializeField] private GameObject cornerUpRight;
    [SerializeField] private GameObject cornerUpLeft;
    [SerializeField] private GameObject cornerDownLeft;
    [SerializeField] private GameObject cornerDownRight;
    [SerializeField] private SkinnedMeshRenderer straightMesh;
    [SerializeField] private MeshRenderer cornerUpRightMesh;
    [SerializeField] private MeshRenderer cornerUpLeftMesh;
    [SerializeField] private MeshRenderer cornerDownLeftMesh;
    [SerializeField] private MeshRenderer cornerDownRightMesh;


    public int index { get; set; }
    public Texture texture { get; set; }

    public void SetColor(Texture newTexture)
    {
        texture = newTexture;
        straightMesh.material.SetTexture("_BaseMap", texture);
        cornerUpRightMesh.material.SetTexture("_BaseMap", texture);
        cornerUpLeftMesh.material.SetTexture("_BaseMap", texture);
        cornerDownLeftMesh.material.SetTexture("_BaseMap", texture);
        cornerDownRightMesh.material.SetTexture("_BaseMap", texture);

        var effect = Instantiate(colorChangeEffect, transform.position, Quaternion.identity, transform);

        var renderer = effect.GetComponent<ParticleSystemRenderer>();
        renderer.material.SetTexture("_MainTex", texture);

        effect.Play();
    }

    public void SetGraphic(string name)
    {
        switch (name)
        {
            case "straight":
                straight.SetActive(true);
                cornerUpRight.SetActive(false);
                cornerUpLeft.SetActive(false);
                cornerDownRight.SetActive(false);
                cornerDownLeft.SetActive(false);
                break;

            case "UR corner":
                straight.SetActive(false);
                cornerUpRight.SetActive(true);
                cornerUpLeft.SetActive(false);
                cornerDownRight.SetActive(false);
                cornerDownLeft.SetActive(false);
                break;
            case "UL corner":
                straight.SetActive(false);
                cornerUpRight.SetActive(false);
                cornerUpLeft.SetActive(true);
                cornerDownRight.SetActive(false);
                cornerDownLeft.SetActive(false);
                break;
            case "DR corner":
                straight.SetActive(false);
                cornerUpRight.SetActive(false);
                cornerUpLeft.SetActive(false);
                cornerDownRight.SetActive(true);
                cornerDownLeft.SetActive(false);
                break;
            case "DL corner":
                straight.SetActive(false);
                cornerUpRight.SetActive(false);
                cornerUpLeft.SetActive(false);
                cornerDownRight.SetActive(false);
                cornerDownLeft.SetActive(true);
                break;
        }
    }
}
