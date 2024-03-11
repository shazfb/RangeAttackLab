using UnityEngine;
using TMPro;
using System.Collections;

public class CountdownTimer : MonoBehaviour
{
    public float countdownDuration = 60f;
    private float remainingTime;
    public TextMeshProUGUI countdownText;
    public GameObject addTimeText;
    public GameOverController gameOverController;

    public GameObject gameOverScreen;
    private bool isCountdownPaused = false;

    private void Start()
    {
        remainingTime = countdownDuration;
        UpdateUI();
        gameOverScreen.gameObject.SetActive(false);
        addTimeText.SetActive(false);
    }

    private void Update()
    {
        if (!isCountdownPaused)
        {
            if (remainingTime > 0f)
            {
                remainingTime -= Time.deltaTime;
                UpdateUI();
            }
            else
            {
                remainingTime = 0f;
                UpdateUI();
                gameOverScreen.SetActive(true);
            }
        }
    }

    private void UpdateUI()
    {
        countdownText.text = "Time remaining: " + Mathf.RoundToInt(remainingTime);
    }

    public void AddTime()
    {
        StartCoroutine(AddTimeCoroutine());
    }

    private IEnumerator AddTimeCoroutine()
    {
        remainingTime += 5f;
        addTimeText.SetActive(true); 

        yield return new WaitForSeconds(1.5f); 

        addTimeText.SetActive(false); 
    }

    public void PauseCountdown(bool pause)
    {
        isCountdownPaused = pause;
    }
}
