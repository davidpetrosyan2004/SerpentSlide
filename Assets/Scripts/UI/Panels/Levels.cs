using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelButton : MonoBehaviour
{
    public List<Button> levelButtons = new();
    void Start()
    {
        int levelIndex = PlayerPrefs.GetInt("CurrentLevel", 1);
        for (int i = 0; i < levelButtons.Count; i++)
        {
            if (i < levelIndex)
            {
                levelButtons[i].interactable = true;
            }
            else
            {
                levelButtons[i].interactable = false;
            }
        }
    }
}
