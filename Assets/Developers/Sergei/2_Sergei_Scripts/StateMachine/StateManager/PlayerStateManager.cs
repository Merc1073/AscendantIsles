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

        //----------------------------------------

        MoveInput();
        Dash();
        CoyoteJumpTimer();

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
        data.moveDirection = (data.orientation.forward * data.moveVertical) + (data.orientation.right * data.moveHorizontal);
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

}
