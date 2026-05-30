using UnityEngine;

public class TutorialFinger : MonoBehaviour
{
    private void Awake()
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
        Destroy(gameObject);
    }
}
