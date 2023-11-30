using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LiveManager : MonoBehaviour
{
    private const string TIMER_TEXT_FORMAT= "Next in {0:00}:{1:00}";
    private const string LIVE_TEXT_FORMAT = "Lives : <color=#00FF00>{0}</color><color=#FFFFFF>/{1}</color>";
    public float TimeToGenerateLife = 300f; 
    public int MaxLives = 5; 
    private int CurrentLives = 0;
    private DateTime LifeGenStartTime;

    private float Timer; 
    private bool IsTimerRunning = false; 

    [SerializeField]private TextMeshProUGUI TimerText;
    [SerializeField]private TextMeshProUGUI LiveCounter;

    public void Initialize(float timeToGenerateLife, int maxLives, int currentLives, DateTime lifeGenStartTime)
    {
        this.TimeToGenerateLife = timeToGenerateLife;
        this.MaxLives = maxLives;
        this.CurrentLives = currentLives;
        this.LifeGenStartTime = lifeGenStartTime;

        Timer = (float)(LifeGenStartTime.AddSeconds(TimeToGenerateLife) - DateTime.Now).TotalSeconds;
        Debug.Log("Timer"+Timer);
        while (Timer < 0)
        {
            Timer = Timer + TimeToGenerateLife;
            CurrentLives++;
            lifeGenStartTime = LifeGenStartTime.AddSeconds(TimeToGenerateLife);
        }
        if (CurrentLives > MaxLives)
        {
            CurrentLives = MaxLives;
            PlayerDataManager.Instance.SetLiveGenerationStartTime(lifeGenStartTime.Ticks);
        }
        
        UpdateUI();
       
        if (currentLives < maxLives)
        {
            StartTimer();
        }

        LiveCounter.text = string.Format(LIVE_TEXT_FORMAT, CurrentLives, MaxLives);
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
        if (CurrentLives > 1)
            CurrentLives--;

        if (!IsTimerRunning)
        {
            LifeGenStartTime = DateTime.Now;
            PlayerDataManager.Instance.SetLiveGenerationStartTime(DateTime.Now.Ticks);
            StartTimer();
        }
        PlayerDataManager.Instance.SetLives(CurrentLives);
        LiveCounter.text = string.Format(LIVE_TEXT_FORMAT,CurrentLives,MaxLives);
    }
    private void Update()
    {
        if (IsTimerRunning)
        {
            Timer = (float)(LifeGenStartTime.AddSeconds(TimeToGenerateLife) - DateTime.Now).TotalSeconds;

            UpdateUI();

            if (Timer <= 0f)
            {
                GameEventManager.LifeReplenished();
                CurrentLives++;
                if (CurrentLives > MaxLives)
                    CurrentLives = MaxLives;
                IsTimerRunning = false;
                LifeGenStartTime = DateTime.MinValue;
                UpdateUI(); 
                if (CurrentLives < MaxLives)
                {
                    StartTimer();
                    LifeGenStartTime = DateTime.Now;
                    PlayerDataManager.Instance.SetLiveGenerationStartTime(DateTime.Now.Ticks);
                }
                LiveCounter.text = string.Format(LIVE_TEXT_FORMAT, CurrentLives, MaxLives);
                PlayerDataManager.Instance.SetLives(CurrentLives);
            }
        }
    }
    //CFX_Hit_C White(Clone)
    private void StartTimer()
    {
        if (IsTimerRunning)
            return;
        Timer = TimeToGenerateLife;
        IsTimerRunning = true;
        LiveCounter.text = string.Format(LIVE_TEXT_FORMAT, CurrentLives, MaxLives);
    }

    private void UpdateUI()
    {
        if (CurrentLives < MaxLives)
        {
            TimerText.text = FormatTime(Timer);
            TimerText.gameObject.SetActive(true);
        }
        else
        {
            TimerText.gameObject.SetActive(false);
        }
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        return string.Format(TIMER_TEXT_FORMAT, minutes, seconds);
    }

    #region Unit Testing
#if UNITY_EDITOR
    [SerializeField] bool IsTesting = false;

    public int CurrentLives1 { get => CurrentLives; set => CurrentLives = value; }

    private void LateUpdate()
    {
        if (IsTesting)
        {
            IsTesting = false;
            StartTimer();
        }
    }
#endif
    #endregion
}
