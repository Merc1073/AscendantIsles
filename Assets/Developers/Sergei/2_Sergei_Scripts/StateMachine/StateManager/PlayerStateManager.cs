using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum PlayerState
{
    IDLE,
    MOVING,
    AERIAL,
}

public class PlayerStateManager : MonoBehaviour
{

    public PlayerData data;
    PlayerBaseState currentState;

    public Dictionary<PlayerState, PlayerBaseState> PlayerStates = new Dictionary<PlayerState, PlayerBaseState>()
    {
        {PlayerState.IDLE, new PlayerIdleState()},
        {PlayerState.MOVING, new PlayerMovingState()},
        {PlayerState.AERIAL, new PlayerAerialState()},
    };

    private void Start()
    {
        data = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();

        data.rb.freezeRotation = true;
        data.originalVelocity = data.maxVelocity;

        currentState = PlayerStates[PlayerState.IDLE];
        currentState.EnterState(this);
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void Update()
    {
        currentState.UpdateState(this);
        data.velocity = data.rb.velocity;
        data.magnitude = data.rb.velocity.magnitude;

        data.maxVelocity = Mathf.Clamp(data.maxVelocity, data.originalVelocity, data.dashVelocity);

        //----------------------------------------

        MoveInput();
        Dash();
        CoyoteJumpTimer();
        ChangeCameraFOV();

    }

    public void SwitchState(PlayerState state)
    {
        currentState = PlayerStates[state];
        PlayerStates[state].EnterState(this);
    }

    private void MoveInput()
    {
        data.moveHorizontal = Input.GetAxisRaw("Horizontal");
        data.moveVertical = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        data.moveDirection = (data.playerOrientation.forward * data.moveVertical) + (data.playerOrientation.right * data.moveHorizontal);
    }

    private void CoyoteJumpTimer()
    {
        if (data.isGrounded) data.coyoteTimeCounter = data.coyoteTime;
        else data.coyoteTimeCounter -= Time.deltaTime;
    }

    private void Dash()
    {
        if(data.currentDashCount > 0)
        {
            data.dashTimer += Time.deltaTime;

            if(data.dashTimer >= 0f)
            {
                data.currentDashCount--;
                data.dashTimer = 0f;
            }
        }
    }

    private void ChangeCameraFOV()
    {
        float speed = data.rb.velocity.magnitude;

        float targetFOV = Mathf.Lerp(data.minCameraFOV, data.maxCameraFOV, speed / data.originalVelocity * 0.333333333f);

        data.currentCameraFOV = Mathf.SmoothDamp(data.currentCameraFOV, targetFOV, ref data.cameraVelocityFOV, data.cameraSmoothTime);

        data.currentCameraFOV = Mathf.Clamp(data.currentCameraFOV, data.minCameraFOV, data.maxCameraFOV);

        data.mainCamera.fieldOfView = data.currentCameraFOV;
    }

}