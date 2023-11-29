using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
/// <summary>
/// Local PlayerPref based data storage
/// </summary>
public class LocalPlayerDataProvider : IPlayerDataProvider
{
    
    private int lives;

    //Constants
    private const string LIVES_KEY = "p_l";
    private const string LEVEL_STATE_KEY = "l_s_k";
    private const string SAVED_GAME_DATA_KEY = "s_g_d";

    private const string LIVE_GEN_START_TIME = "l_gen_s_k";
    private const string LIVES_SLOT_SIZE_KEY = "l_slot_s_k";

    public void Init()
    {
        lives = PlayerPrefs.GetInt(LIVES_KEY);
    }

    public void Refresh()
    {
        Init();
    }

    public void Reset()
    {
        
    }

    public int GetLives()
    {
        if (PlayerPrefs.HasKey(LIVES_KEY))
            return PlayerPrefs.GetInt(LIVES_KEY);
        return 6;
    }

    public void SetLives(int lives)
    {
        PlayerPrefs.SetInt(LIVES_KEY,lives);
        PlayerPrefs.Save();
    }
    public Dictionary<int, LevelState> GetLevelStates()
    {
        
        if (!PlayerPrefs.HasKey(LEVEL_STATE_KEY))
        {
            return new Dictionary<int, LevelState>();
        }

        string dataString = PlayerPrefs.GetString(LEVEL_STATE_KEY);
        Dictionary<int, LevelState> states = JsonConvert.DeserializeObject<Dictionary<int, LevelState>>(dataString);

        Debug.Log("Level Status "+dataString);
        return states;
    }
    public void SetLevelCompleted(int levelId)
    {
        string levelStates = null;
        Dictionary<int, LevelState> levelStateMap = GetLevelStates();
        LevelState data;
        if (levelStateMap.ContainsKey(levelId))
        {
            data = levelStateMap[levelId];
        }
        else
        {
            data = new LevelState();
            data.IsCompleted = true;
            data.IsLocked = false;
        }
        levelStateMap[levelId] = data;
        levelStates = JsonConvert.SerializeObject(levelStateMap);
        PlayerPrefs.SetString(LEVEL_STATE_KEY, levelStates);
        PlayerPrefs.Save();
    }

    public void SaveGameData(string data)
    {
        PlayerPrefs.SetString(SAVED_GAME_DATA_KEY,data);
        PlayerPrefs.Save();
    }

    public string FetchSavedGameData()
    {
        return PlayerPrefs.GetString(SAVED_GAME_DATA_KEY);
    }

    public bool IsGameSaved()
    {
        return PlayerPrefs.HasKey(SAVED_GAME_DATA_KEY);
    }
    public void ClearSavedGameData()
    {
        PlayerPrefs.DeleteKey(SAVED_GAME_DATA_KEY);
        PlayerPrefs.Save();
    }

    public DateTime GetLiveGenerationStartTime()
    {
        if(PlayerPrefs.HasKey(LIVE_GEN_START_TIME))
        {
            return new DateTime(long.Parse(PlayerPrefs.GetString(LIVE_GEN_START_TIME)));
        }
        return DateTime.Now;
    }

    public void SetLiveGenerationStartTime(long ticks)
    {
         PlayerPrefs.SetString(LIVE_GEN_START_TIME,ticks.ToString());
        PlayerPrefs.Save();
    }

    public int GetLivesSlotSize()
    {
        if (PlayerPrefs.HasKey(LIVES_SLOT_SIZE_KEY))
        {
            return PlayerPrefs.GetInt(LIVES_SLOT_SIZE_KEY);
        }
        return 6;
    }
}
