using System.Collections;

public class dfDefine  
{
    public static readonly int MAX_PARTS_COUNT = 5;

    public static readonly int MAX_AI_OBJECT = 5;


    public enum GameState
    {
        GAMESTATE_NONE = 0,
        GAMESTATE_LAUNCH = 1,
        GAMESTATE_LOGO = 2,
        GAMESTATE_PATCH = 3,
        GAMESTATE_LOBBY = 4,
        GAMESTATE_INGAME = 5
    }

    public static readonly float INTERVAL_LOGO = 5.0f;

 

}
