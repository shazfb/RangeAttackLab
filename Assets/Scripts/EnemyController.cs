using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float destroyDelay = 5f;
    public float speed = 0.4f;
    public float rotationSpeed = 5f;

    private bool hasCollided = false;
    private Transform player;
    private Animator animator;
    private EnemyKillCounter killCounter;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponentInChildren<Animator>();
        killCounter = FindObjectOfType<EnemyKillCounter>();
    }

    void Update()
    {
        if (!hasCollided)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

            Vector3 direction = (player.position - transform.position).normalized;

            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

            animator.SetBool("isFollowing", true);
        }
        else
        {
            animator.SetBool("isFollowing", false);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Arrow") && !hasCollided)
        {
            hasCollided = true;

            collision.transform.parent = transform;

            animator.SetTrigger("Death");

            if (killCounter != null)
            {
                killCounter.IncreaseKillCount();
            }

            Destroy(collision.gameObject);
            Destroy(gameObject, destroyDelay);
        }
    }
}
