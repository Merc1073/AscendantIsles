using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{

    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Player is IDLE.");
    }

    public override void UpdateState(PlayerStateManager player)
    {
        if(player.data.moveHorizontal != 0f || player.data.moveVertical != 0f)
        {
            player.SwitchState(PlayerState.MOVING);
        }

        if (Input.GetKey(KeyCode.Space) && player.data.canJump && player.data.isGrounded)
        {
            player.data.canJump = false;
            player.SwitchState(PlayerState.JUMPING);
        }
    }

}
