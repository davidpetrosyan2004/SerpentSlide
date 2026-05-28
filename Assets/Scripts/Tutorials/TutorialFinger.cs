using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialFinger : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        Destroy(gameObject);
    }
}
