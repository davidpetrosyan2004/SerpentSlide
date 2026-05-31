using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Action<bool> OnGameCondition;
    public Action OnFingerTap;
    public Action OnSnakeDive;
    public Action TimerStart;
    private int gateCount = 0;
    public bool isTimerStarted;
    public bool isTutorial = false;
    public static GameManager Instance;
    [SerializeField] private GameObject infoPanel=null;

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
    private void Start()
    {
        if (infoPanel != null)
        {
            infoPanel.SetActive(true);
            infoPanel.transform.DOPunchScale(Vector3.one, 2f);
        }
    }

    public void OnResumeButtonClick()
    {
        infoPanel.SetActive(false);
    }

    public void AddGateCount()
    {
        gateCount++;
    }

    public void RemoveGateCount()
    {
        gateCount--;
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
