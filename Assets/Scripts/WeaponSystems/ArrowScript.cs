using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    private float totalDamage = 0f;
    private EnemyKillCounter enemyKillCounter;

    private bool hitSomething = false;
    private bool arrowReleased = false; 
    private float destroyDelay = 5f;
    private float releaseTime; 

    private void Start()
    {
        enemyKillCounter = FindObjectOfType<EnemyKillCounter>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemyKillCounter.IncreaseKillCount();
            hitSomething = true;
        }

        if (arrowReleased && other.CompareTag("Obstacle"))
        {
            hitSomething = true;
        }
    }

    public void SetDamage(float damage)
    {
        totalDamage = damage;
    }

    private void Update()
    {
        if (hitSomething || !arrowReleased)
            return;

        if (Time.time - releaseTime >= destroyDelay)
        {
            Destroy(gameObject);
        }
    }

    public void ReleaseArrow()
    {
        arrowReleased = true;
        releaseTime = Time.time;
    }
}
