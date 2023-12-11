using System;

public static class EventManager
{
   public class GameActionEvents
    {
        public static event Action OnMoleKilled;
        public static event Action OnMoleMissed;
        public static event Action OnMoleSpawned;

        public static void MoleKilled()
        {
            OnMoleKilled?.Invoke();
        }

        public static void MoleMissed()
        {
            OnMoleMissed?.Invoke();
        }

        public static void MoleSpawned()
        {
            OnMoleSpawned?.Invoke();
        }
    }
    public class GameStateEvents
    {
        public static event Action OnLifeReplenished;
        public static event Action OnGameOver;
        public static event Action<int> OnLevelFinished;
        public static event Action OnGamePaused;
        public static event Action OnResumeGame;


        public static void LevelFinished(int levelId)
        {
            OnLevelFinished?.Invoke(levelId);
        }

        public static void LifeReplenished()
        {
            OnLifeReplenished?.Invoke();
        }

        public static void GameOver()
        {
            OnGameOver?.Invoke();
        }

        public static void GamePaused()
        {
            OnGamePaused?.Invoke();
        }

        public static void ResumeGame()
        {
            OnResumeGame?.Invoke();
        }
    }
    
    public class PlayerEvents
    {
        public static event Action OnShowMainMenu;
        public static event Action OnStartNextLevel;
        public static event Action<LevelData> OnLevelSelected;

        public static void LevelSelected(LevelData level)
        {
            OnLevelSelected?.Invoke(level);
        }

        public static void ShowMainMenu()
        {
            OnShowMainMenu?.Invoke();
        }

        public static void StartNextLevel()
        {
            OnStartNextLevel?.Invoke();
        }

    }
    
    public class AdEvents
    {
        public static event Action OnWatchAdToContinue;
        public static event Action OnVideoAdRewardReceived;

        public static void WatchAdToContinue()
        {
            OnWatchAdToContinue?.Invoke();
        }

        public static void VideoAdRewardRecieved()
        {
            OnVideoAdRewardReceived?.Invoke();
        }
    }








}
