using System;
using UnityEngine;
using UnityEngine.UI;

public class LiveManager : MonoBehaviour
{
    public float timeToGenerateLife = 300f; 
    public int maxLives = 5; 
    private int currentLives = 0;
    private DateTime lifeGenStartTime;

    private float timer; 
    private bool isTimerRunning = false; 

    public Text timerText; 

    public void Initialize(float timeToGenerateLife, int maxLives, int currentLives, DateTime lifeGenStartTime)
    {
        this.timeToGenerateLife = timeToGenerateLife;
        this.maxLives = maxLives;
        this.currentLives = currentLives;
        this.lifeGenStartTime = lifeGenStartTime;

        UpdateUI();

        if (currentLives < maxLives)
        {
            StartTimer();
        }
    }

    private void OnEnable()
    {
        GameEventManager.OnMoleMissed += OnLiveConsumed;
    }
    private void OnDisable()
    {
        GameEventManager.OnMoleMissed -= OnLiveConsumed;
    }
    public void OnLiveConsumed()
    {
        if (currentLives > 1)
            currentLives--;

        if (!isTimerRunning)
            StartTimer();
    }
    private void Update()
    {
        if (isTimerRunning)
        {
            timer = (float)(lifeGenStartTime.AddSeconds(timeToGenerateLife) - DateTime.Now).TotalSeconds;

            UpdateUI();

            if (timer <= 0f)
            {
                GameEventManager.LifeReplenished();
                currentLives++;
                isTimerRunning = false;
                lifeGenStartTime = DateTime.MinValue;
                UpdateUI(); 
                if (currentLives < maxLives)
                {
                    StartTimer(); 
                }
            }
        }
    }

     private void StartTimer()
    {
        if (isTimerRunning)
            return;
        lifeGenStartTime = DateTime.Now; 
        timer = timeToGenerateLife;
        isTimerRunning = true;
    }

    private void UpdateUI()
    {
        if (currentLives < maxLives)
        {
            timerText.text = FormatTime(timer);
            timerText.gameObject.SetActive(true);
        }
        else
        {
            timerText.gameObject.SetActive(false);
        }
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    
}
