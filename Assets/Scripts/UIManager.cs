using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI gameStateText, playerDeathText, simpleBotDeathText, trainedBotDeathText;

    public GameObject player, playerSimpleBot;

    public GameObject restartButton;

    // Update is called once per frame
    void Update()
    {
        //UpdateDeathText();
        DisplayDeathText();
    }

    void DisplayDeathText()
    {

        //Debug.Log(GameData.gameState);
        switch (GameData.gameState)
        {
            case 0:
                player.SetActive(true);
                playerSimpleBot.SetActive(false);
                // playerDeathText.enabled = true;
                // simpleBotDeathText.enabled = false;
                // trainedBotDeathText.enabled = false;
                restartButton.SetActive(false);
                gameStateText.text = "NPC is learning";

                break;
            case 1:
                player.SetActive(true);
                playerSimpleBot.SetActive(false);
                // playerDeathText.enabled = true;
                // simpleBotDeathText.enabled = true;
                // trainedBotDeathText.enabled = false;

                gameStateText.text = "NPC A is playing";

                break;
            case 2:
                player.SetActive(false);
                playerSimpleBot.SetActive(true);
                // playerDeathText.enabled = true;
                // simpleBotDeathText.enabled = true;
                // trainedBotDeathText.enabled = true;

                gameStateText.text = "NPC B is playing";

                break;
            case 3:
                player.SetActive(false);
                playerSimpleBot.SetActive(false);
                gameStateText.text = "Game Over";
                restartButton.SetActive(true);
                break;
        }
    }

    void UpdateDeathText()
    {
        playerDeathText.text = "Player Deaths: " + GameData.playerDeaths;
        simpleBotDeathText.text = "NPC A Deaths: " + GameData.simpleBotDeaths;
        trainedBotDeathText.text = "NPC B Deaths: " + GameData.simpleBotDeaths;
    }

    public void Restart(){
        SceneManager.LoadScene(0);
    }
}
