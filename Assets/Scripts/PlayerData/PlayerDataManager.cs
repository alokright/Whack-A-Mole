using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class PlayerDataManager 
{
    IPlayerDataProvider dataProvider = null;
    private PlayerDataManager()
    {
        dataProvider = new LocalPlayerDataProvider();
    }
    private static PlayerDataManager _instance;
    public static PlayerDataManager Instance
    {
        get
        {
            if (_instance == null)
                Init();
            return _instance;
        }
    }
 
    public static void Init()
    {
        _instance = new PlayerDataManager();
    }

    public int GetPlayerLives()
    {
        return dataProvider.GetLives();
    }

   public string GetCurrentLevelId()
    {
        return "1";
    }

    public int GetCurrentScore()
    {
        return 0;
    }

    public Dictionary<string, bool> GetLevelLockStatus()
    {
        Dictionary<string, bool> status = new Dictionary<string, bool>();
        return status;
    }

  
    public void UpdateLevelCompletion(int levelId)
    {
        dataProvider.SetLevelCompleted(levelId);
    }

    public void SaveGameData(string data)
    {
        dataProvider.SaveGameData(data);
    }

    public bool IsGameSaved()
    {
        return dataProvider.IsGameSaved();
    }

    public Dictionary<string, object> GetSavedGameData()
    {
        return JsonConvert.DeserializeObject<Dictionary<string,object>>( dataProvider.FetchSavedGameData());
    }

    public void ClearSavedGameData()
    {
        dataProvider.ClearSavedGameData();
    }
}
