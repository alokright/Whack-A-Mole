using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerDataProvider
{

    public void Init();
    public void Refresh();
    public void Reset();

    public int GetLives();
    public void SetLevelCompleted(int levelId);
    public void SaveGameData(string data);
    public string FetchSavedGameData();
    public void ClearSavedGameData();
    public bool IsGameSaved();
}
[Serializable]
public struct LevelState
{
    public bool IsLocked;
    public bool IsCompleted;
}