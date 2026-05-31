using UnityEngine;
using UnityEngine.UI;

public class SettingsButtons : MonoBehaviour
{
    [SerializeField] private Button settingButton;
    [SerializeField] private Sprite muteSprite;
    private Sprite initSprite;

    private bool isSoundButtonPressed = true;
    private bool isHapticsButtonPressed = true;

    private void Start()
    {
        initSprite = settingButton.GetComponent<Image>().sprite;
            Debug.Log(initSprite.name);
        if (initSprite.name == "music2_0")
        {
            var isMute = PlayerPrefs.GetInt("Sound");
            if (isMute == 0)
            {
                settingButton.image.sprite = initSprite;
                isSoundButtonPressed = true;
            }
            else
            {
                settingButton.image.sprite = muteSprite;
                isSoundButtonPressed = false;
            }
        }
        else if (initSprite.name == "vinbration_0")
        {
            var isMute = PlayerPrefs.GetInt("Haptics");
            if (isMute == 0)
            {
                settingButton.image.sprite = initSprite;
                isHapticsButtonPressed = true;
            }
            else
            {
                settingButton.image.sprite = muteSprite;
                isHapticsButtonPressed = false;
            }
        }
    }
    public void ClickSoundButton()
    {
        AudioManager.Instance.PlaySound("ButtonClick");
        if (isSoundButtonPressed)
        {
            AudioManager.Instance.AudioMute();
            isSoundButtonPressed = false;
            settingButton.image.sprite = muteSprite;
            PlayerPrefs.SetInt("Sound", 1);
        }
        else
        {
            AudioManager.Instance.AudioOn();
            isSoundButtonPressed = true;
            settingButton.image.sprite = initSprite;
            PlayerPrefs.SetInt("Sound", 0);
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
            PlayerPrefs.SetInt("Haptics", 1);
        }
        else
        {
            isHapticsButtonPressed = true;
            settingButton.image.sprite = initSprite;
            AudioManager.Instance.isHaptics = true;
            PlayerPrefs.SetInt("Haptics", 0);
        }
    }


}
