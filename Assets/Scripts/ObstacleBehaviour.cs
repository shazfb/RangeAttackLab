using UnityEngine;

public class ObstacleBehaviour : MonoBehaviour
{
    private bool hasCollided = false;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Arrow") && !hasCollided)
        {
            hasCollided = true;

            ContactPoint contact = collision.contacts[0];
            collision.transform.position = contact.point;

            Rigidbody arrowRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            if (arrowRigidbody != null)
            {
                arrowRigidbody.isKinematic = false;
            }

            Collider arrowCollider = collision.gameObject.GetComponent<Collider>();
            if (arrowCollider != null)
            {
                arrowCollider.enabled = false;
            }
        }
    }
}
