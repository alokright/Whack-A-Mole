using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int CurrentLives= 0;
    public int MaxLives = 5;

   
    void Start()
    {
        
    }
    private void OnEnable()
    {
        EventManager.AdEvents.OnVideoAdRewardReceived += RewardVideoAds;
    }

    private void OnDisable()
    {
        EventManager.AdEvents.OnVideoAdRewardReceived -= RewardVideoAds;
    }
    private void RewardVideoAds()
    {
        if (CurrentLives + 1 < MaxLives)
            CurrentLives++;
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
