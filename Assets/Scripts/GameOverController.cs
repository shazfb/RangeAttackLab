using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    public EnemySpawner enemySpawner;
    public EnemyKillCounter enemyKillCounter;
    public CountdownTimer countdownTimer;
    public PlayerMovementController playerMovement;
    public BowController bowController;

    public TextMeshProUGUI waveText;
    public TextMeshProUGUI enemiesKilledText;

    private bool activeEndScreen;
    
    void Start()
    {
        activeEndScreen = false;
        waveText.text = "";
        enemiesKilledText.text = "";
    }

    void Update()
    {
        if (countdownTimer.gameOverScreen.activeSelf && !activeEndScreen)
        {
            OpenEndScene();

            if (enemySpawner != null)
                waveText.text = "Waves Completed: " + enemySpawner.GetCurrentWave().ToString();

            if (enemyKillCounter != null)
                enemiesKilledText.text = "Total Enemies Killed: " + enemyKillCounter.GetEnemiesKilled().ToString();
            activeEndScreen=true;

        }
    }

    public void OpenEndScene()
    {
        Time.timeScale = 0f;
        countdownTimer.gameOverScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (playerMovement != null)
            playerMovement.enabled = false;
        if (bowController != null)
            bowController.enabled = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
        
    }
}
