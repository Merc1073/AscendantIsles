using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovingState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Player is MOVING.");
    }

    public override void UpdateState(PlayerStateManager player)
    {
        MovePlayer(player);
        GroundCheck(player);
        HandleDrag(player);
        SpeedControl(player);

        //Switch State to IDLE if player's velocity is 0
        if (player.data.rb.velocity == new Vector3(0, 0, 0))
        {
            player.SwitchState(PlayerState.IDLE);
        }

        //Switch state to JUMPING if player presses Space key
        if (Input.GetKey(KeyCode.Space) && player.data.canJump && player.data.isGrounded)
        {
            player.data.canJump = false;
            player.SwitchState(PlayerState.JUMPING);
        }

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

    private void GroundCheck(PlayerStateManager player)
    {
        //Check if player is touching the ground
        player.data.isGrounded = Physics.Raycast(player.transform.position, Vector3.down, player.data.playerHeight * 0.5f + 0.2f, player.data.groundLayer);
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

    private void SpeedControl(PlayerStateManager player)
    {
        Vector3 flatVelocity = new Vector3(player.data.rb.velocity.x, 0f, player.data.rb.velocity.z);

        //Limit the player's velocity
        if(flatVelocity.magnitude > 10f)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * 10f;
            player.data.rb.velocity = new Vector3(limitedVelocity.x, player.data.rb.velocity.y, limitedVelocity.z);
        }
    }

}
