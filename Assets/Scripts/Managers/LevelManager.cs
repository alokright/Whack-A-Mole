
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour,ISaveGameState
{
    [SerializeField]private Transform MoleParent;
    [SerializeField]private List<GameObject> HolesVisuals;

    [SerializeField] private TextMeshProUGUI TimerText;
    [SerializeField] private GameState gameState = GameState.DEFAULT;
    [SerializeField] private GameConfig gameConfig;
    [SerializeField] TextMeshProUGUI ScoreText;
    [SerializeField] TextMeshProUGUI LivesText;
    [SerializeField] GameSaveHandler GameSaveHandler;
    private int CurrentScore = 0;
    private int repeatCount;
    private List<int> HoleSequence = null;
    private int HoleIndex = 0;
    private float StartTimeDuration = 0;
    private bool ShowGameTimer = false;

    private const string START_TIMER_TEXT_FORMAT = "Game starting in {0}...";
    private const string RESUME_TIMER_TEXT_FORMAT = "Game resuming in {0}...";
    private const string HUD_TEXT_FORMAT = "{0}/{1}";
    private int MaxLives;
    private int ScoreToWin;
    private int LivesConsumed = 0;
   
    private int CurrentLevelId;

    private bool HasWatchedAd = false;
    private Coroutine MoleSpawnRoutine = null;
    private float minSpawnWait = 0f;
    private float maxSpawnWait;
    private LevelData currentLevel;

    private void OnEnable()
    {
        EventManager.GameActionEvents.OnMoleKilled += OnMoleKilled;
        EventManager.GameActionEvents.OnMoleMissed += OnMoleMissed;
    }


    private void OnDisable()
    {
        EventManager.GameActionEvents.OnMoleKilled -= OnMoleKilled;
        EventManager.GameActionEvents.OnMoleMissed -= OnMoleMissed;
    }
    
    public void LoadLevel(LevelData level)
    {
        CurrentScore = 0;
        HoleIndex = 0;
        LivesConsumed = 0;

        InitializeLevel(level);
    }
    
    public void ReloadLevel(LevelData level)
    {
        InitializeLevel(level);
    }

    private void InitializeLevel(LevelData level)
    {
        repeatCount = level.MaxScore + level.MaxLives;
        ScoreToWin = level.MaxScore;
        HoleSequence = GenerateNonAdjacentSequence(level.NumberOfHoles, level.MaxScore + level.MaxLives + 1);
        for (int i = 0; i < HolesVisuals.Count; i++)
            HolesVisuals[i].SetActive(i < level.NumberOfHoles);
        MaxLives = level.MaxLives;
        gameState = GameState.RUNNING;
        LivesText.text = string.Format(HUD_TEXT_FORMAT, MaxLives - LivesConsumed, MaxLives);
        ScoreText.text = string.Format(HUD_TEXT_FORMAT, CurrentScore, ScoreToWin);
        CurrentLevelId = level.Index;
        currentLevel = level;
    }

    public void StartGame(DateTime startTime)
    {
        StartTimeDuration = 3;
        ShowGameTimer = true;
        TimerText.gameObject.SetActive(true);
        gameState = GameState.DEFAULT;
        SetupMoleSpawn();
    }
   
    private void SpawnMole()
    {
        GameObject moleObject = ObjectPoolManager.Instance.GetObject(currentLevel.PrefabType);
        moleObject.transform.parent = MoleParent;
        moleObject.transform.localPosition = gameConfig.MolePositions[HoleSequence[HoleIndex++]];
        Mole moleScript = moleObject.GetComponentInChildren<Mole>();

        if (moleScript != null)
        {
            moleScript.ShowMole(gameConfig, currentLevel.MoleMovementDuration, currentLevel.MoleAliveDuration);
        }
        Debug.Log("currentLevel.MoleMovementDuration"+ currentLevel.MoleMovementDuration + " currentLevel.MoleAliveDuration "+ currentLevel.MoleAliveDuration);
        SetupMoleSpawn();

    }

    private void SetupMoleSpawn()
    {
       
        SpawnDelay = UnityEngine.Random.Range(currentLevel.MinWaitTimeToSpawn, currentLevel.MaxWaitTimeToSpawn);
        Debug.Log("Duration" + currentLevel.MinWaitTimeToSpawn + " asd " + currentLevel.MaxWaitTimeToSpawn+"  fer"+ SpawnDelay);
    }
    //Update Scores
    private void OnMoleKilled()
    {
        CurrentScore++;
        ScoreText.text = string.Format(HUD_TEXT_FORMAT, CurrentScore,ScoreToWin);
        Debug.Log("Mole Killed"+CurrentScore);
        if(CurrentScore >= ScoreToWin)
        {
            EventManager.GameStateEvents.LevelFinished(CurrentLevelId);
            gameState = GameState.GAME_OVER;
        }
    }

    
    //Update Lives
    private void OnMoleMissed()
    {
        LivesConsumed++;
        LivesText.text = string.Format(HUD_TEXT_FORMAT, MaxLives - LivesConsumed, MaxLives);

        if (LivesConsumed >= MaxLives)
        {
            EventManager.GameStateEvents.GameOver();
            gameState = GameState.PAUSED;
        }
        
    }

    IEnumerator DelayedShowMole(Mole mole, float delay)
    {
        yield return new WaitForSeconds(delay);
        mole.ShowMole(gameConfig, currentLevel.MoleMovementDuration, currentLevel.MoleAliveDuration);
    }
    IEnumerator RepeatedlyShowMoles()
    {
        for (int i = 0; i < repeatCount; i++)
        {
            GetAndShowMole();
            while (gameState == GameState.PAUSED)
                yield return null;
            yield return new WaitForSeconds(UnityEngine.Random.Range(1.1f, 2f)); // Wait before showing the next mole
        }
    }
    
    void GetAndShowMole()
    {

        GameObject moleObject = ObjectPoolManager.Instance.GetObject(currentLevel.PrefabType);
        moleObject.transform.parent = MoleParent;
        Debug.Log("game MolePositions" + gameConfig.MolePositions.Count);
        moleObject.transform.localPosition = gameConfig.MolePositions[HoleSequence[HoleIndex++]];
        Mole moleScript = moleObject.GetComponentInChildren<Mole>();

        if (moleScript != null)
        {
            float delay = UnityEngine.Random.Range(0.5f, 1f);
            MoleSpawnRoutine = StartCoroutine(DelayedShowMole(moleScript, delay));
        }
    }

    private List<int> GenerateNonAdjacentSequence(int range, int sequenceLength)
    {
        List<int> sequence = new List<int>();
        int lastNumber = -1; // Start with a number outside the range

        for (int i = 0; i < sequenceLength; i++)
        {
            int nextNumber;

            do
            {
                nextNumber = UnityEngine.Random.Range(0, range);
            }
            while (nextNumber == lastNumber);

            sequence.Add(nextNumber);
            lastNumber = nextNumber;
        }

        return sequence;
    }

    public void ResumeGame()
    {
        gameState = GameState.PAUSED;
        StartTimeDuration = 3;
        ShowGameTimer = true;
        TimerText.gameObject.SetActive(true);
        MaxLives +=1 ;
        SetupMoleSpawn();
    }

    float SpawnDelay = 0;
    private void Update()
    {
        if (ShowGameTimer)
        {
            StartTimeDuration -= Time.deltaTime;
            if (StartTimeDuration > 0)
                TimerText.text = string.Format(gameState == GameState.PAUSED ? RESUME_TIMER_TEXT_FORMAT : START_TIMER_TEXT_FORMAT, Math.Ceiling(StartTimeDuration));
            else
            {
                ShowGameTimer = false;
                gameState = GameState.RUNNING;
                TimerText.gameObject.SetActive(false);
               // StartCoroutine("RepeatedlyShowMoles");
            }
        }

        if(gameState == GameState.RUNNING)
        {
            SpawnDelay -= Time.deltaTime;
            if(SpawnDelay <= 0)
            {
                SpawnMole();
            }
        }

    }
   
    public Dictionary<string, object> SaveGameData(Dictionary<string, object> data)
    {
        if (gameState != GameState.RUNNING)
            return data;
        data[Constants.LEVEL_ID_KEY] = CurrentLevelId;
        data[Constants.LEVEL_SCORE_KEY] = CurrentScore;
        data[Constants.LEVEL_LIVES_CONSUMED_KEY] = LivesConsumed;
        data[Constants.LEVEL_AD_WATCHED_KEY] = false;
        return data;
    }
    public void SetGameResumeData(Dictionary<string, object> data)
    {
        CurrentLevelId = int.Parse(data[Constants.LEVEL_ID_KEY].ToString());
        CurrentScore = int.Parse(data[Constants.LEVEL_SCORE_KEY].ToString()) ;
        LivesConsumed = int.Parse(data[Constants.LEVEL_LIVES_CONSUMED_KEY].ToString());
        HasWatchedAd = bool.Parse(data[Constants.LEVEL_AD_WATCHED_KEY].ToString());
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause && gameState == GameState.RUNNING)
        {
            GameSaveHandler.SaveData();
        }

    }

    public enum GameState
    {
        RUNNING,
        PAUSED,
        GAME_OVER,
        DEFAULT
    }
    
    #region Unit Testing

    [SerializeField] private bool isTesting = false;
    [SerializeField] LevelData testLevel;
    private void LateUpdate()
    {
        if (isTesting)
        {
            isTesting = false;
            LoadLevel(testLevel);
            StartGame(DateTime.Now.Add(TimeSpan.FromSeconds(3)));
        }
    }
    #endregion
}
