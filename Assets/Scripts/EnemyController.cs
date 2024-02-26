using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float destroyDelay = 5f;
    public float speed = 0.4f; 
    public float rotationSpeed = 5f; 

    private bool hasCollided = false;
    private Transform player; 
    private Animator animator; 
    private Transform childTransform;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponentInChildren<Animator>();
        childTransform = animator.transform;
    }

    void Update()
    {
        if (!hasCollided)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
                        
            Vector3 direction = (player.position - transform.position).normalized;
                       
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            childTransform.rotation = Quaternion.Slerp(childTransform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
                        
            animator.SetBool("isFollowing", true);
        }
        else
        {            
            animator.SetBool("isFollowing", false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Arrow") && !hasCollided)
        {            
            hasCollided = true;
                        
            transform.parent = other.transform;
                        
            Destroy(other.gameObject, destroyDelay);
            Destroy(gameObject, destroyDelay); 
                        
            animator.SetTrigger("Death");
        }
    }
}
