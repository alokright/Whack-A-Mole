using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]Transform LevelsGrid;
    [SerializeField] List<LevelData> AllLevels;
    [SerializeField] GameObject LevelUIPrefab;
    Dictionary<string, bool> LevelLockStatus = null;
    
    private void Start()
    {
        FetchLevelDetails();
        PopulateLevels(AllLevels, LevelLockStatus);
    }

    private void FetchLevelDetails()
    {
        //Fetch levels
        AllLevels.Sort();
        //Fetch LockStatus
        LevelLockStatus =  PlayerDataManager.Instance.GetLevelLockStatus();

    }
    private void PopulateLevels(List<LevelData> levels, Dictionary<string, bool> levelLockStatus)
    {
        GameObject level = null;
        for(int i = 0; i < levels.Count; i++)
        {
            level = GameObject.Instantiate(LevelUIPrefab, LevelsGrid, false);
            level.GetComponent<LevelUI>().SetDetails(AllLevels[i],false);
           
        }
         
    }
    public void ShowAds()
    {
        AdManager.Instance.ShowAds();
    }
}
