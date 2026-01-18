using UnityEngine;
using TMPro;

public class TimerDisplayTMP : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float startTime = 15f;
    public bool countdown = true;


    public GameObject player, enemy;
    private float currentTime;
    private bool running = true;

    void Start()
    {
        currentTime = startTime;
        UpdateTimerText();
    }

    void Update()
    {
        if (!running) return;

        currentTime += countdown ? -Time.deltaTime : Time.deltaTime;

        // if (countdown && currentTime <= 0f)
        // {
        //     currentTime = 0f;
        //     running = false;
        // }

        UpdateTimerText();


        if(currentTime <= 0)
        {
            switch (GameData.gameState)
            {
                case 0:
                    GameData.gameState = 1;
                    currentTime = startTime;
                    player.transform.position = new Vector3(0, 1, -6);
                    player.GetComponent<PlayerHealth>().ResetHealth();
                    enemy.GetComponent<EnemyHealth>().ResetHealth();
                    break;

                case 1:
                    GameData.gameState = 2;
                    currentTime = startTime;
                    player.transform.position = new Vector3(0, 1, -6);
                    player.GetComponent<PlayerHealth>().ResetHealth();
                    enemy.GetComponent<EnemyHealth>().ResetHealth();
                    break;
                case 2:
                    GameData.gameState = 3;
                    currentTime = 0f;
                    running = false;
                    player.transform.position = new Vector3(0, 1, -6);
                    player.GetComponent<PlayerHealth>().ResetHealth();
                    enemy.GetComponent<EnemyHealth>().ResetHealth();
                    break;
            }
        }
    }

    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public void StartTimer() => running = true;
    public void StopTimer() => running = false;
}
