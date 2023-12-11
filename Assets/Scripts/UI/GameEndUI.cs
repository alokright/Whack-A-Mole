using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameEndUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI HeaderText;
    [SerializeField] Button NextButton;
    [SerializeField] Button MainMenuButton;
    [SerializeField] Button WatchAdButton;

    private void Start()
    {
        NextButton.onClick.AddListener(ShowNextLevel);
        MainMenuButton.onClick.AddListener(ShowMainMenu);
        WatchAdButton.onClick.AddListener(WatchAdToContinue);
    }

    
    public void ShowPopup(bool playerWon)
    {
        gameObject.SetActive(true);
        if (playerWon)
        {
            HeaderText.text = "You Won!";
            NextButton.gameObject.SetActive(true);
            MainMenuButton.gameObject.SetActive(true);
            WatchAdButton.gameObject.SetActive(false);
        }
        else
        {
            HeaderText.text = "You Lost!";
            NextButton.gameObject.SetActive(false);
            MainMenuButton.gameObject.SetActive(true);
            WatchAdButton.gameObject.SetActive(true);
        }
    }


    private void ShowMainMenu()
    {
        gameObject.SetActive(false);
        EventManager.PlayerEvents.ShowMainMenu();
    }

    private void ShowNextLevel()
    {
        gameObject.SetActive(false);
        EventManager.PlayerEvents.StartNextLevel();
    }

    private void WatchAdToContinue()
    {
        EventManager.AdEvents.WatchAdToContinue();
       
    }

    public void HidePopup()
    {
        gameObject.SetActive(false);
    }
}
