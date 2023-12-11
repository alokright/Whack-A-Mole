using System.Collections.Generic;

public interface ISaveGameState
{
    Dictionary<string, object> SaveGameData(Dictionary<string, object> data);
    void SetGameResumeData(Dictionary<string, object> data);
}