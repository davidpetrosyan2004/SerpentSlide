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

    private void Start()
    {
        iceCount.text = count.ToString();
        GameManager.Instance.OnSnakeDive += IceCountDecrease;
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnSnakeDive -= IceCountDecrease;
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
            Destroy(gameObject);
        }
    }

}
