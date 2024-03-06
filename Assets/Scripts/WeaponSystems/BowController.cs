using UnityEngine;
using TMPro;

public class BowController : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Transform arrowSpawnPoint;
    public GameObject bowEquipped;
    public TextMeshProUGUI pullbackText; 

    private BowType bowType;
    private string bowName;

    [SerializeField] private float maxLaunchSpeed = 100f;
    [SerializeField] private float minLaunchSpeed = 5f;

    private float currentPullback = 0f;
    private bool isPulling = false;
    private float drawbackStartTime;

    private float activeMultiplier;
    private float activePullbackTime;
    private float pullbackPercentage;

    private GameObject instantiatedArrow;
    private Vector3 arrowDirection = Vector3.forward;

    private void Start()
    {
        EquipBow();
    }

    private void Update()
    {
        if (isPulling)
        {
            UpdatePull();
            UpdateArrowDirection();
            UpdateArrowPosition();
            UpdatePullbackText(); 
        }

        if (Input.GetMouseButtonDown(0))
        {
            StartPulling();
        }
        else if (Input.GetMouseButtonUp(0) && isPulling)
        {
            ReleaseArrow();
        }

        pullbackText.gameObject.SetActive(isPulling);
    }

    private void UpdateArrowPosition()
    {
        instantiatedArrow.transform.position = arrowSpawnPoint.position;
    }

    private void EquipBow()
    {
        if (bowEquipped != null)
        {
            bowType = bowEquipped.GetComponent<BowType>();
            Debug.Log("Bow Name = " + bowName);
            if (bowType != null)
            {
                activeMultiplier = bowType.damageMultiplier;
                activePullbackTime = bowType.pullbackTime;
                bowName = bowType.typeName;
            }
            else
            {
                Debug.LogError("No BowType script found on the equipped bow.");
            }
        }
        else
        {
            Debug.LogError("No equipped bow GameObject assigned to the bowEquipped field.");
        }
    }

    private void StartPulling()
    {
        isPulling = true;
        drawbackStartTime = Time.time;
        instantiatedArrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, Quaternion.identity);
        instantiatedArrow.transform.parent = transform;
    }

    private void UpdatePull()
    {
        float drawbackTime = Time.time - drawbackStartTime;

        currentPullback = Mathf.Clamp(drawbackTime, float.Epsilon, activePullbackTime);

        pullbackPercentage = Mathf.Clamp((currentPullback / activePullbackTime) * 100, float.Epsilon, 100);
    }

    private void UpdateArrowDirection()
    {
        Vector3 rayEnd = arrowSpawnPoint.position + Camera.main.transform.forward * 10f;
        arrowDirection = (rayEnd - arrowSpawnPoint.position).normalized;

        Debug.DrawRay(arrowSpawnPoint.position, arrowDirection * 10f, Color.green);

        instantiatedArrow.transform.rotation = Quaternion.LookRotation(arrowDirection);
    }

    public void ReleaseArrow()
    {
        isPulling = false;

        Rigidbody arrowRigidbody = instantiatedArrow.AddComponent<Rigidbody>();
        arrowRigidbody.useGravity = true;

        float launchSpeed = Mathf.Lerp(minLaunchSpeed, maxLaunchSpeed, pullbackPercentage / 100);

        arrowRigidbody.AddForce(arrowDirection * launchSpeed, ForceMode.Impulse);

        ArrowScript arrowScript = instantiatedArrow.GetComponent<ArrowScript>();
        float baseDamage = 1f;
        float totalDamage = baseDamage * activeMultiplier * pullbackPercentage;
        arrowScript.SetDamage(totalDamage);

        arrowScript.ReleaseArrow();

        instantiatedArrow.transform.parent = null;
        instantiatedArrow = null;

        Vector3 arrowVelocity = arrowRigidbody.velocity;
        if (arrowVelocity != Vector3.zero)
        {
            instantiatedArrow.transform.rotation = Quaternion.LookRotation(arrowVelocity);
        }
    }

    private void UpdatePullbackText()
    {
        if (pullbackText != null)
        {
            pullbackText.text = pullbackPercentage.ToString("F0") + "%";
        }
    }
}
