using TMPro;
using UnityEngine;
using DG.Tweening;

public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text leftDigit;
    [SerializeField] private TMP_Text rightDigit;

    [SerializeField] private int m_Time = 0;
    [SerializeField] private int s_Time = 0;

    private float time;
    private float delay;
    private bool running = false;
    private bool isEnded = false;
    private bool isCalledAudio = false;

    private int lastLeft = -1;
    private int lastRight = -1;

    private void OnEnable()
    {
        GameManager.Instance.TimerStart += StartTimer;
        GameManager.Instance.OnGameCondition += StopTimer;
    }

    private void OnDisable()
    {
        GameManager.Instance.TimerStart -= StartTimer;
    }

    private void Start()
    {
        time = m_Time * 60 + s_Time;
        leftDigit.text = m_Time.ToString() + ": ";
        if (s_Time / 10 != 0)
        {
            rightDigit.text = (s_Time-1).ToString();
        }
        else
        {
            rightDigit.text = "0"+(s_Time - 1).ToString();
        }
    }

    private void Update()
    {
        if (isEnded) return;
        if (!running) return;

        time -= Time.deltaTime;
        delay += Time.deltaTime;

        int total = Mathf.Max(0, Mathf.FloorToInt(time));

        int minutes = total / 60;
        int seconds = total % 60;

        int left = seconds / 10;
        int right = seconds % 10;

        if (delay >= 1f)
        {
            delay = 0;

            leftDigit.text = minutes.ToString() + ":" + left.ToString();
            rightDigit.text = right.ToString();

            if (left != lastLeft)
            {
                Punch(leftDigit.transform);
                lastLeft = left;
            }

            if (right != lastRight)
            {
                Punch(rightDigit.transform);
                lastRight = right;
            }
        }

        if (total <= 30)
        {
            if (!isCalledAudio)
            {
                isCalledAudio = true;
                AudioManager.Instance.PlaySound("ClockTick");
            }
            leftDigit.color = Color.red;
            rightDigit.color = Color.red;
        }

        if (time <= 0)
        {
            isEnded = true;
            GameManager.Instance.OnGameCondition?.Invoke(false);
            AudioManager.Instance.StopSound("ClockTick");
        }
    }

    private void Punch(Transform t)
    {
        t.DOKill();
        t.localScale = Vector3.one;

        t.DOPunchScale(Vector3.one * 0.35f, 0.25f, 10, 1f);
    }

    public void StartTimer()
    {
        running = true;
    }

    public void StopTimer(bool isGame)
    {
        isEnded = true;
    }
}