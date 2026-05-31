using TMPro;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using DG.Tweening;
public class ChainIce : MonoBehaviour
{
    [SerializeField] private List<GameObject> ices;
    [SerializeField] private TextMeshPro iceCount;
    [SerializeField] private ParticleSystem effectIce;
    [SerializeField] private int count;

    private void OnEnable()
    {
        GameManager.Instance.OnSnakeDive += IceCountDecrease;
    }
    private void OnDisable()
    {
        GameManager.Instance.OnSnakeDive -= IceCountDecrease;
    }

    private void Start()
    {
        iceCount.text = count.ToString();
    }

    public void IceCountDecrease()
    {
        Debug.Log("Splash");
        count--;
        iceCount.text = count.ToString();
        foreach(var ice in ices)
        {
            var effect = Instantiate(effectIce, ice.transform.position, Quaternion.identity);
            effect.Play();
        }
        AudioManager.Instance.PlaySound("IceBreak");
        if (count <= 0)
        {
            IceDisapear();
        }
    }

    public void IceDisapear()
    {
        foreach(var ice in ices)
        {
            ice.SetActive(false);
        }
    }
}
