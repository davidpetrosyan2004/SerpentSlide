using UnityEngine;
using UnityEngine.UI;

public class SettingsButtons : MonoBehaviour
{
    [SerializeField] private Button settingButton;
    [SerializeField] private Sprite muteSprite;
    private Sprite initSprite;

    private bool isSoundButtonPressed = true;
    private bool isMusicButtonPressed = true;
    private bool isHapticsButtonPressed = true;

    private void Start()
    {
        initSprite = settingButton.GetComponent<Image>().sprite;
    }
    public void ClickSoundButton()
    {
        AudioManager.Instance.PlaySound("ButtonClick");
        if (isSoundButtonPressed)
        {
            AudioManager.Instance.AudioMute();
            isSoundButtonPressed = false;
            settingButton.image.sprite = muteSprite;
        }
        else
        {
            AudioManager.Instance.AudioOn();
            isSoundButtonPressed = true;
            settingButton.image.sprite = initSprite;
        }
    }
    public void ClickMusicButton()
    {
        AudioManager.Instance.PlaySound("ButtonClick");
        if (isMusicButtonPressed)
        {
            isMusicButtonPressed = false;
            settingButton.image.sprite = muteSprite;
        }
        else
        {
            isMusicButtonPressed = true;
            settingButton.image.sprite = initSprite;
        }
    }
    public void ClickHapticsButton()
    {
        AudioManager.Instance.PlaySound("ButtonClick");
        if (isHapticsButtonPressed)
        {
            isHapticsButtonPressed = false;
            settingButton.image.sprite = muteSprite;
            AudioManager.Instance.isHaptics = false;
        }
        else
        {
            isHapticsButtonPressed = true;
            settingButton.image.sprite = initSprite;
            AudioManager.Instance.isHaptics = true;
        }
    }


}
