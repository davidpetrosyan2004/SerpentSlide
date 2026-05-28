using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public Action<bool> OnGameCondition;
    public Action TimerStart;
    private int gateCount = 0;
    public bool isTimerStarted;
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
        }
    }
}
