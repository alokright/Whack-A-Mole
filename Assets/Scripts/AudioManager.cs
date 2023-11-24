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
        GameEventManager.OnMoleSpawned += MoleSpawned;
        GameEventManager.OnMoleMissed += MoleMissed;
        GameEventManager.OnMoleKilled += MoleKilled;
        GameEventManager.OnGameOver += GameOver;
        GameEventManager.OnLevelFinished += LevelFinished;
        GameEventManager.OnLifeReplenished += LifeReplenished;
    }

    private void OnDisable()
    {
        GameEventManager.OnMoleSpawned -= MoleSpawned;
        GameEventManager.OnMoleMissed -= MoleMissed;
        GameEventManager.OnMoleKilled -= MoleKilled;
        GameEventManager.OnGameOver -= GameOver;
        GameEventManager.OnLevelFinished -= LevelFinished;
        GameEventManager.OnLifeReplenished -= LifeReplenished;
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

    private void LevelFinished()
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
