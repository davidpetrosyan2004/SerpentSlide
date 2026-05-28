using DG.Tweening;
using UnityEngine;

public class PausePanel : MonoBehaviour
{
    public GameObject pauseGamePanel;
    private bool isPauseGame;
    public void PanelClose()
    {
        AudioManager.Instance.PlaySound("ButtonClick");
        isPauseGame = false;

        pauseGamePanel.transform.DOKill();
        pauseGamePanel.transform
                .DOScale(0f, 0.4f)
                .SetEase(Ease.OutBack).OnComplete(() => pauseGamePanel.SetActive(false));
    }
    public void PanelPopUp()
    {
        AudioManager.Instance.PlaySound("ButtonClick");
        isPauseGame = true;
        pauseGamePanel.SetActive(true);

        pauseGamePanel.transform.localScale = Vector3.zero;
        pauseGamePanel.transform.DOKill();

        pauseGamePanel.transform
            .DOScale(1f, 0.4f)
            .SetEase(Ease.OutBack);
    }

    public void PausePanelPopUp()
    {
        if (isPauseGame)
        {
            PanelClose();
        }
        else
        {
            PanelPopUp();
        }
    }
}
