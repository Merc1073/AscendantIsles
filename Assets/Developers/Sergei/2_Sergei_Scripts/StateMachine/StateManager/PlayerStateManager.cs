using System.Collections.Generic;
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
        currentState = PlayerStates[PlayerState.IDLE];
        currentState.EnterState(this);
    }

    private void Update()
    {
        currentState.UpdateState(this);
        data.state = currentState.ToString();
    }

    public void SwitchState(PlayerState state)
    {
        currentState = PlayerStates[state];
        PlayerStates[state].EnterState(this);
    }

}