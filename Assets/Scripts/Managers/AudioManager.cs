using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip MoleSpawnedAudio;
    public AudioClip MoleKilledAudio;
    public AudioClip MoleMissedAudio;
    public AudioClip LevelFinishedAudio;
    public AudioClip GameOverAudio;
    public AudioClip LifeReplinshedAudio;
    private void OnEnable()
    {
        EventManager.GameActionEvents.OnMoleSpawned += MoleSpawned;
        EventManager.GameActionEvents.OnMoleMissed += MoleMissed;
        EventManager.GameActionEvents.OnMoleKilled += MoleKilled;
        EventManager.GameStateEvents.OnGameOver += GameOver;
        EventManager.GameStateEvents.OnLevelFinished += LevelFinished;
        EventManager.GameStateEvents.OnLifeReplenished += LifeReplenished;
    }

    private void OnDisable()
    {
        EventManager.GameActionEvents.OnMoleSpawned -= MoleSpawned;
        EventManager.GameActionEvents.OnMoleMissed -= MoleMissed;
        EventManager.GameActionEvents.OnMoleKilled -= MoleKilled;
        EventManager.GameStateEvents.OnGameOver -= GameOver;
        EventManager.GameStateEvents.OnLevelFinished -= LevelFinished;
        EventManager.GameStateEvents.OnLifeReplenished -= LifeReplenished;
    }

    private void MoleSpawned()
    {
        PlayAudio(MoleSpawnedAudio);
    }

    private void MoleMissed()
    {
        PlayAudio(MoleMissedAudio);
    }

    private void MoleKilled()
    {
        PlayAudio(MoleKilledAudio);
    }

    private void GameOver()
    {
        PlayAudio(GameOverAudio);
    }

    private void LevelFinished(int levelId)
    {
        PlayAudio(LevelFinishedAudio);
    }

    private void LifeReplenished()
    {
        PlayAudio(LifeReplinshedAudio);
    }
   
    void PlayAudio(AudioClip clip)
    {
        if (audioSource != null )//&& !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
