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
    public Color color { get; set; }

    public void SetColor(Color newColor)
    {
        color = newColor;
        straightMesh.material.color = color;
        cornerUpRightMesh.material.color = color;
        cornerUpLeftMesh.material.color = color;
        cornerDownLeftMesh.material.color = color;
        cornerDownRightMesh.material.color = color;

        var effect = Instantiate(colorChangeEffect, transform.position, Quaternion.identity, transform);

        var renderer = effect.GetComponent<ParticleSystemRenderer>();
        renderer.material.color = color;

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
