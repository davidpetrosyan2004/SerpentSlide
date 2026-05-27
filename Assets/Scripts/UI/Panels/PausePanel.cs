using DG.Tweening;
using UnityEngine;

public class PausePanel : MonoBehaviour
{
    public GameObject pauseGamePanel;
    private bool isPauseGame;
    public void PanelClose()
    {
        pauseGamePanel.transform.DOKill();
        pauseGamePanel.transform
                .DOScale(0f, 0.4f)
                .SetEase(Ease.OutBack).OnComplete(() => pauseGamePanel.SetActive(false));
    }
    public void PanelPopUp()
    {
        pauseGamePanel.SetActive(true);

        pauseGamePanel.transform.localScale = Vector3.zero;
        pauseGamePanel.transform.DOKill();

        pauseGamePanel.transform
            .DOScale(1f, 0.4f)
            .SetEase(Ease.OutBack);
    }

    public void PausePanelPopUp()
    {
        AudioManager.Instance.PlaySound("ButtonClick");
        if (isPauseGame)
        {
            isPauseGame = false;
            PanelClose();
        }
        else
        {
            isPauseGame = true;
            PanelPopUp();
        }
    }
}
