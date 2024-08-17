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
    public float groundDrag;

    [Header("Jumping Variables")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public bool canJump;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask groundLayer;
    public bool isGrounded;

    [Header("Components")]
    public Transform orientation;
    public Rigidbody rb;

}
