using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerDataProvider
{

     void Init();
     void Refresh();
     void Reset();

     int GetLives();
     void SetLives(int l);
     void SetLevelCompleted(int levelId);
     void SaveGameData(string data);
     string FetchSavedGameData();
     void ClearSavedGameData();
     bool IsGameSaved();
     DateTime GetLiveGenerationStartTime();
    void SetLiveGenerationStartTime(long l);
    int GetLivesSlotSize();
}
[Serializable]
public struct LevelState
{
    public bool IsLocked;
    public bool IsCompleted;
}