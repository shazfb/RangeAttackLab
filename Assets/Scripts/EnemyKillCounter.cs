using TMPro;
using UnityEngine;

public class EnemyKillCounter : MonoBehaviour
{
    private int enemyKillCount = 0;
    public TextMeshProUGUI enemyCountText;

    public void IncreaseKillCount()
    {
        enemyKillCount++;
        UpdateUI();
    }

    private void UpdateUI()
    {
        enemyCountText.text = "Enemies Killed: " + enemyKillCount;
    }
}
