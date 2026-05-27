using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneButtonBinder : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private Button button;
    [SerializeField] private ButtonType buttonType;

    enum ButtonType
    {
        LoadSceneButton,
        CurrentLevelButton,
        RefreshLevelButton,
        NextLevelButton
    }

    private void Start()
    {
        if (buttonType == ButtonType.CurrentLevelButton)
        {
            int levelIndex = PlayerPrefs.GetInt("CurrentLevel", 1);
            sceneName = $"Level {levelIndex}";
        }
        else if (buttonType == ButtonType.NextLevelButton)
        {
            int levelIndex = SceneManager.GetActiveScene().buildIndex + 1;
            sceneName = $"Level {levelIndex}";
        }
        else if (buttonType == ButtonType.RefreshLevelButton)
        {
            int levelIndex = SceneManager.GetActiveScene().buildIndex;
            sceneName = $"Level {levelIndex}";
        }

        button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        if (buttonType == ButtonType.NextLevelButton)
        {
            int levelIndex = SceneManager.GetActiveScene().buildIndex + 1;
            PlayerPrefs.SetInt("CurrentLevel", levelIndex);
            PlayerPrefs.Save();
        }

        transform.DOPunchScale(
            Vector3.one * 0.1f,
            0.3f,
            8,
            0.5f
        ).OnComplete(() =>
        {
            SceneManager.LoadScene(sceneName);
        });

        PlaySound();
    }

    public void PlaySound()
    {
        AudioManager.Instance.PlaySound("ButtonClick");
    }
}