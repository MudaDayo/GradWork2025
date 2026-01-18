using UnityEngine;

public class GameData : MonoBehaviour
{
    public static int playerHealth = 10;

    public static int gameState = 0; //0 is when player plays, 1 is when normal bot plays, 2 is when trained bot plays

    public static int playerDeaths, simpleBotDeaths, trainedBotDeaths = 0;
}
