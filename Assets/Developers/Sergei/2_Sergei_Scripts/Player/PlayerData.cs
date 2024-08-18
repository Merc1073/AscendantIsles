using UnityEngine;

public class PlayerData : MonoBehaviour
{

    [Header("Generic Variables")]
    public float health = 100f;

    [Header("Movement Variables")]
    public float moveSpeed = 5f;
    public float moveHorizontal;
    public float moveVertical;
    public Vector3 moveDirection;
    public Vector3 velocity;
    public float magnitude;
    public float groundDrag;
    public float maxVelocity;
    public float originalVelocity;

    [Header("Jumping Variables")]
    public int totalJumpCount = 1;
    public int currentJumpCount = 1;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;

    [Header("Dashing Variables")]
    public int totalDashCount = 2;
    public int currentDashCount = 0;
    public float dashForce;
    public float dashTimer;
    public float dashVelocity;
    public float dashVelocityReduceTime;
    public bool isDashing = false;

    [Header("QoL Platforming Variables")]
    public float coyoteTime = 0.2f;
    public float coyoteTimeCounter;
    public float jumpBufferTime = 0.2f;
    public float jumpBufferCounter;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask groundLayer;
    public bool isGrounded;

    [Header("Camera Variables")]
    public float currentCameraFOV;
    public float minCameraFOV = 90f;
    public float maxCameraFOV = 150f;
    public float cameraSmoothTime = 0.2f;
    public float cameraVelocityFOV;

    [Header("Components")]
    public Camera mainCamera;
    public Transform playerOrientation;
    public Transform cameraOrientation;
    public Rigidbody rb;

}
