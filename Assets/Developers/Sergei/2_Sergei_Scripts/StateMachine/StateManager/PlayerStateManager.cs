using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    IDLE,
    MOVING,
    JUMPING,
}

public class PlayerStateManager : MonoBehaviour
{

    public PlayerData data;
    PlayerBaseState currentState;

    public Dictionary<PlayerState, PlayerBaseState> PlayerStates = new Dictionary<PlayerState, PlayerBaseState>()
    {
        {PlayerState.IDLE, new PlayerIdleState()},
        {PlayerState.MOVING, new PlayerMovingState()},
        {PlayerState.JUMPING, new PlayerJumpingState()},
    };

    private void Start()
    {
        data = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();

        data.rb.freezeRotation = true;

        data.canJump = true;

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
        data.moveDirection = data.orientation.forward * data.moveVertical + data.orientation.right * data.moveHorizontal;
    }

    private void ResetJump()
    {
        data.canJump = true;
    }

    public void ResetJumpInvoke()
    {
        Invoke(nameof(ResetJump), data.jumpCooldown);
    }

}
