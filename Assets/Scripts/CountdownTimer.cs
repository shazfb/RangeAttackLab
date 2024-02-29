using TMPro;
using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    public float countdownDuration = 60f; 
    private float remainingTime; 
    public TextMeshProUGUI countdownText;

    private void Start()
    {
        remainingTime = countdownDuration;
        UpdateUI();
    }

    private void Update()
    {
        if (remainingTime > 0f)
        {
            remainingTime -= Time.deltaTime;
            UpdateUI();
        }
        else
        {
            //implement game over/end screen
        }
    }

    private void UpdateUI()
    {
        countdownText.text = "Time Left: " + Mathf.RoundToInt(remainingTime);
    }
}
