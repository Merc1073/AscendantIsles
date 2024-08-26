using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public PlayerData data;
    public CameraShake playerVCam;

    //-------------------------------

    private void Start()
    {
        data = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();

        data.rb.freezeRotation = true;
        data.originalVelocity = data.maxVelocity;
    }

    private void Update()
    {
        data.velocity = data.rb.velocity;
        data.magnitude = data.rb.velocity.magnitude;

        data.maxVelocity = Mathf.Clamp(data.maxVelocity, data.originalVelocity, data.dashVelocity);

        MoveInput();
        RegenerateDash();
        CoyoteJumpTimer();
        ChangeCameraFOV();
        CalculateImpactVelocity();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        GravityControl();
    }



    //-------------------------------



    private void MoveInput()
    {
        //Get input for player's movement
        data.moveHorizontal = Input.GetAxisRaw("Horizontal");
        data.moveVertical = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        //Determines the player's move direction
        data.moveDirection = (data.playerOrientation.forward * data.moveVertical) + (data.playerOrientation.right * data.moveHorizontal);
    }

    private void CoyoteJumpTimer()
    {
        //Allows player to jump a few milliseconds after moving off of a platform
        if (data.isGrounded) data.coyoteTimeCounter = data.coyoteTime;
        else data.coyoteTimeCounter -= Time.deltaTime;
    }

    private void RegenerateDash()
    {
        //Regenerates player's dash based on dash cooldown
        if (data.currentDashCount > 0)
        {
            data.dashTimer += Time.deltaTime;

            if (data.dashTimer >= 0f)
            {
                data.currentDashCount--;
                data.dashTimer = 0f;
            }
        }
    }

    private void CalculateImpactVelocity()
    { 
        //Calculates the downwards Y Velocity of player before impact with floor
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 5f))
        {
            if (data.rb.velocity.y < 0)
            {
                playerVCam.previousImpactVelocity = data.rb.velocity.y;
            }
        }
    }

    private void ChangeCameraFOV()
    {

        //Change camera FOV based on player's forward Z vector move speed
        Vector3 forwardDirection = data.playerOrientation.forward;

        float forwardSpeed = Vector3.Dot(data.rb.velocity, forwardDirection);

        if (forwardSpeed > 0)
        {
            float targetFOV = Mathf.Lerp(playerVCam.minCameraFOV, playerVCam.maxCameraFOV, Mathf.Abs(forwardSpeed) / data.originalVelocity * 0.333333333f);

            playerVCam.currentCameraFOV = Mathf.SmoothDamp(playerVCam.currentCameraFOV, targetFOV, ref playerVCam.cameraVelocityFOV, playerVCam.cameraSmoothTime);
        }

        else
        {
            playerVCam.currentCameraFOV = Mathf.SmoothDamp(playerVCam.currentCameraFOV, playerVCam.minCameraFOV, ref playerVCam.cameraVelocityFOV, playerVCam.cameraSmoothTime);
        }

        playerVCam.currentCameraFOV = Mathf.Clamp(playerVCam.currentCameraFOV, playerVCam.minCameraFOV, playerVCam.maxCameraFOV);

        playerVCam.vCam.m_Lens.FieldOfView = playerVCam.currentCameraFOV;

    }

    private void GravityControl()
    {
        //Have player decelerate if Y Velocity is bigger than 0
        if (data.rb.velocity.y > 0f)
        {
            float airDeceleration = Mathf.MoveTowards(data.rb.velocity.y, 0.0f, 0.3f);
            data.rb.velocity = new Vector3(data.rb.velocity.x, airDeceleration, data.rb.velocity.z);
        }

        //Start decelerating the player towards the ground once Y Velocity once reached peak of jump
        if (data.rb.velocity.y < 0f)
        {
            float airAcceleration = Mathf.MoveTowards(data.rb.velocity.y, data.maxTerminalVelocity, 0.4f);
            data.rb.velocity = new Vector3(data.rb.velocity.x, airAcceleration, data.rb.velocity.z);
        }

        //Limit the player's terminal velocity when falling
        if (data.rb.velocity.y < data.maxTerminalVelocity)
        {
            data.rb.velocity = new Vector3(data.rb.velocity.x, data.maxTerminalVelocity, data.rb.velocity.z);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Shake camera upon impact with floor, based on Y Velocity just before impact
        if (collision.gameObject.CompareTag("Ground"))
        {
            playerVCam.ShakeCamera(playerVCam.shakeIntensity, Mathf.Abs(playerVCam.previousImpactVelocity) * 0.1f);
        }
    }

}
