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
    private LevelData currentLevel;
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
    }

    private void OnEnable()
    {
        GameEventManager.OnLevelSelected += OnLevelSelected;
        GameEventManager.OnGameOver += GameOver;
        GameEventManager.OnLevelFinished += LevelFinished;
        GameEventManager.OnShowMainMenu += ShowMainMenu;
        GameEventManager.OnStartNextLevel += StartNextLevel;
        GameEventManager.OnWatchAdToContinue += WatchAdToContinue;
        GameEventManager.OnResumeGame += ResumeGame;
        
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
        GameEventManager.OnLevelSelected -= OnLevelSelected;
        GameEventManager.OnGameOver -= GameOver;
        GameEventManager.OnLevelFinished -= LevelFinished;
        GameEventManager.OnShowMainMenu -= ShowMainMenu;
        GameEventManager.OnStartNextLevel -= StartNextLevel;
        GameEventManager.OnWatchAdToContinue -= WatchAdToContinue;
        GameEventManager.OnResumeGame -= ResumeGame;
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

    private void LevelFinished()
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
        GameEventManager.OnVideoAdRewardReceived += AdRewardReceived;
    }

    private void AdRewardReceived()
    {
        LevelManager.ResumeGame();
        GameEndPopup.HidePopup();
        GameEventManager.OnVideoAdRewardReceived -= AdRewardReceived;
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
        int id = int.Parse(data[ISaveGameState.LEVEL_ID_KEY].ToString());
        currentLevel =  levelDataProvder.GetLevel(id);
    }
}
