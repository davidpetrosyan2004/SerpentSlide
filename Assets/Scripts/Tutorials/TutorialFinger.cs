using UnityEngine;

public class TutorialFinger : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.OnFingerTap += OnTutorialOver;
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnFingerTap -= OnTutorialOver;
    }

    public void OnTutorialOver()
    {
        GameManager.Instance.isTutorial = false;
        Debug.Log("falsed");
        Destroy(gameObject);
        Debug.Log("Destroyed");
    }
}
