using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour,ISaveGameState
{
    [SerializeField] GameObject MainMenuParent;
    [SerializeField] LevelManager LevelManager;
    [SerializeField] GameEndUI GameEndPopup;
    [SerializeField] LevelDataProvider levelDataProvder;
    [SerializeField] GameSaveHandler SaveHandler;
    [SerializeField] LiveManager LifeManager;
    [SerializeField] GameConfig gameConfig;
    private LevelData currentLevel;
    private void Awake()
    {
        Application.targetFrameRate = 60;
        gameConfig = Resources.Load<GameConfig>("GameConfig");
        
    }
    void Start()
    {
        AdManager.Initialize();
        if (SaveHandler.ShouldResumeGame())
        {
            SaveHandler.ShowResumePopup();
        }
        else
        {
            //Show Main Menu
            ShowMainMenu();
        }
        LifeManager.Initialize(gameConfig.LifeGenerationDuration, PlayerDataManager.Instance.GetLivesSlotSize(), PlayerDataManager.Instance.GetPlayerLives(),PlayerDataManager.Instance.GetLiveGenerationStartTime());
    }

    private void OnEnable()
    {
        EventManager.PlayerEvents.OnLevelSelected += OnLevelSelected;
        EventManager.PlayerEvents.OnShowMainMenu += ShowMainMenu;
        EventManager.PlayerEvents.OnStartNextLevel += StartNextLevel;

        EventManager.GameStateEvents.OnGameOver += GameOver;
        EventManager.GameStateEvents.OnLevelFinished += LevelFinished;
        EventManager.GameStateEvents.OnResumeGame += ResumeGame;

        EventManager.AdEvents.OnWatchAdToContinue += WatchAdToContinue;

    }

    private void ResumeGame()
    {
        MainMenuParent.SetActive(false);
        SaveHandler.SetGameResumeData();
        LevelManager.ReloadLevel(currentLevel);
        LevelManager.ResumeGame();
    }

    private void OnDisable()
    {
        EventManager.PlayerEvents.OnLevelSelected -= OnLevelSelected;
        EventManager.PlayerEvents.OnShowMainMenu -= ShowMainMenu;
        EventManager.PlayerEvents.OnStartNextLevel -= StartNextLevel;

        EventManager.GameStateEvents.OnGameOver -= GameOver;
        EventManager.GameStateEvents.OnLevelFinished -= LevelFinished;
        EventManager.GameStateEvents.OnResumeGame -= ResumeGame;

        EventManager.AdEvents.OnWatchAdToContinue -= WatchAdToContinue;
        
    }

    private void OnLevelSelected(LevelData obj)
    {
        HideMainMenu();
        currentLevel = obj;
        StartGame(obj);
    }

    private void ShowMainMenu()
    {
        MainMenuParent.SetActive(true);
    }

    private void HideMainMenu()
    {
        MainMenuParent.SetActive(false);
    }
    //Start Level
    private void StartGame(LevelData obj)
    {
        LevelManager.LoadLevel(obj);
        LevelManager.StartGame(DateTime.Now.Add(TimeSpan.FromSeconds(3)));

    }

    private void LevelFinished(int levelId)
    {
        GameEndPopup.ShowPopup(true);
        PlayerDataManager.Instance.UpdateLevelCompletion(currentLevel.Index);
        PlayerDataManager.Instance.ClearSavedGameData();
    }

    private void GameOver()
    {
        GameEndPopup.ShowPopup(false);
    }

    private void WatchAdToContinue()
    {
        AdManager.Instance.ShowAds();
        EventManager.AdEvents.OnVideoAdRewardReceived += AdRewardReceived;
    }

    private void AdRewardReceived()
    {
        LevelManager.ResumeGame();
        GameEndPopup.HidePopup();
        EventManager.AdEvents.OnVideoAdRewardReceived -= AdRewardReceived;
    }

    private void StartNextLevel()
    {
        LevelData nextLevel= levelDataProvder.GetNextLevel(currentLevel);
        LevelManager.LoadLevel(nextLevel);
        currentLevel = nextLevel;
        LevelManager.StartGame(DateTime.Now.Add(TimeSpan.FromSeconds(3)));
    }

    public Dictionary<string, object> SaveGameData(Dictionary<string, object> data)
    {
        return data;
    }

    public void SetGameResumeData(Dictionary<string, object> data)
    {
        int id = int.Parse(data[Constants.LEVEL_ID_KEY].ToString());
        currentLevel =  levelDataProvder.GetLevel(id);
    }

}
