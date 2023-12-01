using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class GameSaveHandler : MonoBehaviour
{
    public List<MonoBehaviour> DataProviders;
    [SerializeField] Button ResumeButton;
    [SerializeField] Button MainMenuButton;

    [SerializeField] GameObject ResumePopupParent;
    
    void Start()
    {
        ResumeButton.onClick.AddListener(()=> {
            ResumePopupParent.SetActive(false);
            GameEventManager.ResumeGame();
        });
        MainMenuButton.onClick.AddListener(()=> {
            ResumePopupParent.SetActive(false);
            PlayerDataManager.Instance.ClearSavedGameData();
            GameEventManager.ShowMainMenu();
        });
       
    }
    private void OnEnable()
    {
        GameEventManager.OnLevelFinished += LevelFinished;
    }
    private void OnDisable()
    {
        GameEventManager.OnLevelFinished -= LevelFinished;
    }

    private void LevelFinished(int obj)
    {
        PlayerDataManager.Instance.ClearSavedGameData();
    }
#if UNITY_EDITOR

    private void OnApplicationQuit()
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
            foreach (var providers in DataProviders)
                data = ((ISaveGameState)providers).SaveGameData(data);

            PlayerDataManager.Instance.SaveGameData(JsonConvert.SerializeObject(data));
      
    }
#endif
    public void SaveData()
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
            foreach (var providers in DataProviders)
                data = ((ISaveGameState) providers).SaveGameData(data);

            PlayerDataManager.Instance.SaveGameData(JsonConvert.SerializeObject(data));
    }

    public bool ShouldResumeGame()
    {
        return PlayerDataManager.Instance.IsGameSaved();
    }

    public void SetGameResumeData()
    {
        Dictionary<string, object> data = PlayerDataManager.Instance.GetSavedGameData(); ;
        foreach (var providers in DataProviders)
        {
            var saveObject = providers as ISaveGameState;
            if (saveObject != null)
                saveObject.SetGameResumeData(data);
        }
    }
    public void ShowResumePopup()
    {
        ResumePopupParent.SetActive(true);

    }
}
