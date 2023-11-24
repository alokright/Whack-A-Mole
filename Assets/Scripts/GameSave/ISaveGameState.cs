using System;
using System.Collections.Generic;

public interface ISaveGameState
{
    public const string LEVEL_ID_KEY = "p_l_id";
    public const string LEVEL_SCORE_KEY = "p_l_sc";
    public const string LEVEL_LIVES_CONSUMED_KEY = "p_l_lcon";
    public const string LEVEL_AD_WATCHED_KEY = "p_l_ad";

    public Dictionary<string, object> SaveGameData(Dictionary<string,object> data);
    public void SetGameResumeData(Dictionary<string, object> data);
}
