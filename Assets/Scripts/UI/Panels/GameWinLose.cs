using DG.Tweening;
using UnityEngine;

public class GameWinLose : MonoBehaviour
{
    [SerializeField] private GameObject gameWinPanel;
    [SerializeField] private GameObject gameLosePanel;
    [SerializeField] private GameObject disableGamePanel;
    [SerializeField] private PausePanel panel;

    private bool isPauseGame = false;

    private void Start()
    {
        disableGamePanel.SetActive(false);
        GameManager.Instance.OnGameCondition += PanelPopUp;
    }
    
    private void OnDestroy()
    {
        GameManager.Instance.OnGameCondition -= PanelPopUp;
        
    }
    public void PanelPopUp(bool gameCondition)
    {
        Debug.Log("Game Condition: " + gameCondition);
        GameObject gamePanel = gameCondition ? gameWinPanel : gameLosePanel;

        gamePanel.transform.localScale = Vector3.zero;

        if (panel.pauseGamePanel.activeSelf)
        {
            panel.PanelClose();
        }

        gamePanel.SetActive(true);
        disableGamePanel.SetActive(true);

        gamePanel.transform
            .DOScale(1f, 0.4f)
            .SetEase(Ease.OutBack);

        if (gameCondition)
            AudioManager.Instance.PlaySound("Win");
        else
            AudioManager.Instance.PlaySound("Lose");
    }

    public void PausePanelPopUp()
    {
        AudioManager.Instance.PlaySound("ButtonClick");

        if (gameWinPanel.activeSelf || gameLosePanel.activeSelf)
            return;

        if (isPauseGame)
        {
            isPauseGame = false;
            panel.PanelClose();
        }
        else
        {
            isPauseGame = true;
            panel.PanelPopUp();
        }
    }
}