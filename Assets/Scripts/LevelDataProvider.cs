using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDataProvider : MonoBehaviour
{
    [SerializeField] List<LevelData> AllLevels;

    private void Start()
    {
        AllLevels.Sort();
    }
    public List<LevelData> GetAllLevels()
    {
        return AllLevels;
    }

    public LevelData GetNextLevel(LevelData level)
    {
        int index = 0;
        List<LevelData> levels = GetAllLevels();
        while (index < levels.Count && !string.Equals(levels[index].Index, level.Index)) { index++; }
        if (index >= levels.Count)
            return null;
        return levels[index+1];
       
    }

    public LevelData GetLevel(int id)
    {
        List<LevelData> levels = GetAllLevels();

        for (int i = 0; i < levels.Count; i++)
            if (levels[i].Index == id)
                return levels[i];
        return null;
    }
}
