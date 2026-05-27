using UnityEngine;
using UnityEngine.UI;

public class SettingsButtons : MonoBehaviour
{
    [SerializeField] private Button settingButton;
    private bool isSoundButtonPressed = true;
    private bool isMusicButtonPressed = true;
    private bool isHapticsButtonPressed = true;
    public void ClickSoundButton()
    {
        if (isSoundButtonPressed)
        {
            isSoundButtonPressed = false;
            Color color = settingButton.image.color;
            color.a = 0.4f;
            settingButton.image.color = color;
        }
        else
        {
            isSoundButtonPressed = true;
            Color color = settingButton.image.color;
            color.a = 1f;
            settingButton.image.color = color;
        }
    }
    public void ClickMusicButton()
    {
        if (isMusicButtonPressed)
        {
            isMusicButtonPressed = false;
            Color color = settingButton.image.color;
            color.a = 0.4f;
            settingButton.image.color = color;
        }
        else
        {
            isMusicButtonPressed = true;
            Color color = settingButton.image.color;
            color.a = 1f;
            settingButton.image.color = color;
        }
    }
    public void ClickHapticsButton()
    {
        if (isHapticsButtonPressed)
        {
            isHapticsButtonPressed = false;
            Color color = settingButton.image.color;
            color.a = 0.4f;
            settingButton.image.color = color;
            AudioManager.Instance.isHaptics = false;
        }
        else
        {
            isHapticsButtonPressed = true;
            Color color = settingButton.image.color;
            color.a = 1f;
            settingButton.image.color = color;
            AudioManager.Instance.isHaptics = true;
        }
    }


}
