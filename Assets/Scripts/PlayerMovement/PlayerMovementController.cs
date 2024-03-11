using UnityEngine;
using System.Collections;
using Cinemachine;

public class PlayerMovementController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float maxSpeedMultiplier = 3f;
    public float currentSpeedMultiplier = 1f;
    public float cameraSensitivity = 2f;
    public float maxVerticalAngle = 80f;
    public float jumpForce = 8f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public RectTransform canvasRect;
    public CinemachineVirtualCamera thirdPersonCamera;
    public CinemachineVirtualCamera aimCamera;

    private bool isAiming = false;
    public bool isGrounded = true;
    private Rigidbody playerRigidbody;
    private Transform playerTransform;
    private float verticalRotation = 0f;

    public Animator animator;

    public GameObject pauseUI;
    private bool isPaused = false;

    public bool hasCollided;

    private CountdownTimer countdownTimer;
   
    private EnemyController enemyController;

    private void Start()
    {
        playerTransform = transform;
        playerRigidbody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        thirdPersonCamera.gameObject.SetActive(true);
        aimCamera.gameObject.SetActive(false);
        hasCollided = false;

        countdownTimer = FindObjectOfType<CountdownTimer>();
       
    }

    private void Update()
    {
        if (isPaused)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePause();
                pauseUI.gameObject.SetActive(false);
            }
            return;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeedMultiplier = maxSpeedMultiplier;
        }
        else
        {
            currentSpeedMultiplier = 1;
        }

        Vector3 movement = new Vector3(horizontal, 0f, vertical).normalized;
        playerTransform.Translate(movement * moveSpeed * currentSpeedMultiplier * Time.deltaTime);

        if (playerRigidbody.velocity.y < 0)
        {
            playerRigidbody.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (playerRigidbody.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            playerRigidbody.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        float mouseX = Input.GetAxis("Mouse X") * cameraSensitivity;
        playerTransform.Rotate(Vector3.up, mouseX);

        if (Input.GetMouseButtonDown(1))
        {
            isAiming = true;
            thirdPersonCamera.gameObject.SetActive(false);
            aimCamera.gameObject.SetActive(true);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            isAiming = false;
            aimCamera.gameObject.SetActive(false);
            thirdPersonCamera.gameObject.SetActive(true);
        }

        if (isAiming)
        {
            float mouseY = Input.GetAxis("Mouse Y") * cameraSensitivity * -1;
            verticalRotation += mouseY;
            verticalRotation = Mathf.Clamp(verticalRotation, -maxVerticalAngle, maxVerticalAngle);
            aimCamera.transform.localRotation = Quaternion.Euler(verticalRotation, aimCamera.transform.localEulerAngles.y, 0f);
        }
        else
        {
            float mouseY = Input.GetAxis("Mouse Y") * cameraSensitivity * -1;
            verticalRotation += mouseY;
            verticalRotation = Mathf.Clamp(verticalRotation, -maxVerticalAngle, maxVerticalAngle);
            thirdPersonCamera.transform.localRotation = Quaternion.Euler(verticalRotation, thirdPersonCamera.transform.localEulerAngles.y, 0f);
        }

        if (canvasRect != null)
        {
            Vector2 mousePos = Input.mousePosition;
            Vector2 canvasSize = canvasRect.sizeDelta;
            float xRotation = (mousePos.y / canvasSize.y) * maxVerticalAngle * 2f - maxVerticalAngle;
            aimCamera.transform.localRotation = Quaternion.Euler(xRotation, aimCamera.transform.localEulerAngles.y, 0f);
        }

        if (isGrounded)
        {
            animator.SetBool("isJumping", false);
        }
        else if (!isGrounded)
        {
            animator.SetBool("isJumping", true);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
            pauseUI.gameObject.SetActive(true);

        }
    }

    private void Jump()
    {
        playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Enemy") && !hasCollided)
        {
            EnemyController enemyController = collision.gameObject.GetComponent<EnemyController>();
            if (enemyController != null && !enemyController.isDead)
            {
                hasCollided = true;
                countdownTimer.ReduceTime();

                Debug.Log("You got hit, 10 seconds deducted");

                StartCoroutine(ResetCollisionAfterDelay());
            }
        }
    }

    private IEnumerator ResetCollisionAfterDelay()
    {
        yield return new WaitForSeconds(1.5f);
        hasCollided = false;
    }


    private void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
    }
}