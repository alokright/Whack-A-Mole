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

#if UNITY_EDITOR

    private void OnApplicationQuit()
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
            foreach (var providers in DataProviders)
                data = ((ISaveGameState)providers).SaveGameData(data);

            PlayerDataManager.Instance.SaveGameData(JsonConvert.SerializeObject(data));
      
    }
#endif
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            foreach (var providers in DataProviders)
                data = ((ISaveGameState) providers).SaveGameData(data);

            PlayerDataManager.Instance.SaveGameData(JsonConvert.SerializeObject(data));
        }
       
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