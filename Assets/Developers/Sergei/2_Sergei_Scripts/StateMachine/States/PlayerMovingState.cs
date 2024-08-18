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
        Dash(player);
        DashTimer(player);
        GroundCheck(player);
        HandleDrag(player);
        SpeedControl(player);

        //Reset the player jump count so they can jump again
        //Reset the max velocity of player
        if (player.data.isGrounded)
        {
            player.data.currentJumpCount = 0;
            player.data.maxVelocity = player.data.originalVelocity;
        }

        //Switch State to IDLE if player's magnitude is 0
        if (player.data.rb.velocity.magnitude == 0f)
        {
            player.SwitchState(PlayerState.IDLE);
        }

        //Switch state to AERIAL if player presses Space key
        if (Input.GetKeyDown(KeyCode.Space) && player.data.currentJumpCount < player.data.totalJumpCount)
        {
            player.data.currentJumpCount++;
            Jump(player);
            player.SwitchState(PlayerState.AERIAL);
        }

        //Switch state to AERIAL if player is not touching the floor
        if (!player.data.isGrounded)
        {
            player.SwitchState(PlayerState.AERIAL);
        }

    }

    private void Jump(PlayerStateManager player)
    {
        //Reset your Y velocity
        player.data.rb.velocity = new Vector3(player.data.rb.velocity.x, 0f, player.data.rb.velocity.z);

        //Give player upwards impulse force to jump
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
        if (player.data.isDashing) return;

        Vector3 flatVelocity = new Vector3(player.data.rb.velocity.x, 0f, player.data.rb.velocity.z);

        //Limit the player's velocity
        if(flatVelocity.magnitude > player.data.maxVelocity)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * player.data.maxVelocity;
            player.data.rb.velocity = new Vector3(limitedVelocity.x, player.data.rb.velocity.y, limitedVelocity.z);
        }
    }

    private void Dash(PlayerStateManager player)
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && player.data.currentDashCount < player.data.totalDashCount)
        {
            player.data.isDashing = true;
            player.data.dashTimer = 0f;
            player.data.rb.velocity = Vector3.zero;
            player.data.maxVelocity = player.data.dashVelocity;
            //player.data.maxVelocity = Mathf.Lerp(player.data.dashVelocity, player.data.maxVelocity, player.data.dashVelocityReduceTime);
            player.data.currentDashCount++;
            player.data.rb.AddForce(player.data.cameraOrientation.forward * player.data.dashForce, ForceMode.Impulse);
        }
    }

    private void DashTimer(PlayerStateManager player)
    {
        if (player.data.isDashing)
        {
            player.data.dashTimer += Time.deltaTime;

            player.data.maxVelocity -= 15f * Time.deltaTime;

            if (player.data.dashTimer > 2f)
            {
                player.data.maxVelocity = player.data.originalVelocity;
                player.data.isDashing = false;
                player.data.dashTimer = 0f;
            }
        }
    }

}
