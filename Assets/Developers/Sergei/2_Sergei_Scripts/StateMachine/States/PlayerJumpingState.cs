using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpingState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Player is JUMPING.");
    }

    public override void UpdateState(PlayerStateManager player)
    {
        MovePlayer(player);
        HandleDrag(player);
        Jump(player);
        player.ResetJumpInvoke();

        //Switch state to MOVING if player is touching ground and velocity is higher than 0
        if (player.data.isGrounded && player.data.rb.velocity != new Vector3(0, 0, 0))
        {
            player.SwitchState(PlayerState.MOVING);
        }

        //Switch state to IDLE if player is touching ground and velocity is 0
        if (player.data.isGrounded && player.data.rb.velocity == new Vector3(0, 0, 0))
        {
            player.SwitchState(PlayerState.IDLE);
        }

    }

    private void Jump(PlayerStateManager player)
    {
        //Reset your Y velocity
        player.data.rb.velocity = new Vector3(player.data.rb.velocity.x, 0f, player.data.rb.velocity.z);

        player.data.rb.AddForce(player.transform.up * player.data.jumpForce, ForceMode.Impulse);
    }

    private void MovePlayer(PlayerStateManager player)
    {
        //Move the player (when on ground)
        if (player.data.isGrounded)
        {
            player.data.rb.AddForce(player.data.moveSpeed * Time.deltaTime * player.data.moveDirection.normalized, ForceMode.Force);
        }

        //Move the player (when in air)
        else if (!player.data.isGrounded)
        {
            player.data.rb.AddForce(player.data.moveSpeed * player.data.airMultiplier * Time.deltaTime * player.data.moveDirection.normalized, ForceMode.Force);
        }
    }

    private void HandleDrag(PlayerStateManager player)
    {
        //Handle the drag
        if (player.data.isGrounded)
        {
            player.data.rb.drag = player.data.groundDrag;
        }
        else
        {
            player.data.rb.drag = 0;
        }
    }

}
