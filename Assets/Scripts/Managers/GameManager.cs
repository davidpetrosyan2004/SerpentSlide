using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Action<bool> OnGameCondition;
    public Action OnFingerTap;
    public Action TimerStart;
    private int gateCount = 0;
    public bool isTimerStarted;
    public bool isTutorial = true;
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddGateCount()
    {
        gateCount++;
    }

    public void RemoveGateCount()
    {
        gateCount--;
        Debug.Log("Gate Count: " + gateCount);
        if (gateCount <= 0)
        {
            Debug.Log("Game Won!");
            OnGameCondition?.Invoke(true);
            var levelIndex = PlayerPrefs.GetInt("CurrentLevel", 1);
            if (SceneManager.GetActiveScene().buildIndex == levelIndex)
            {
                PlayerPrefs.SetInt("CurrentLevel", levelIndex + 1);
            }
        }
    }
}
