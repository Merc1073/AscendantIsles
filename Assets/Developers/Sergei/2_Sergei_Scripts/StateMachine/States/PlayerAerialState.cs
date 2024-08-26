using UnityEngine;

public class PlayerAerialState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Player is AERIAL.");
    }

    public override void UpdateState(PlayerStateManager player)
    {
        MovePlayer(player);
        //GravityControl(player);
        Dash(player);
        DashTimer(player);
        GroundCheck(player);
        HandleDrag(player);
        SpeedControl(player);

        //Allow player to jump if their current jump count does not exceed the total jump count amount
        if (Input.GetKeyDown(KeyCode.Space) && player.data.currentJumpCount < player.data.totalJumpCount)
        {
            player.data.currentJumpCount++;
            Jump(player);
        }

        //Switch state to MOVING if player is touching ground and magnitude is higher than 0
        if (player.data.isGrounded && player.data.rb.velocity.magnitude > 0f)
        {
            player.SwitchState(PlayerState.MOVING);
        }

        //Switch state to IDLE if player is touching ground and magnitude is 0
        if (player.data.isGrounded && player.data.rb.velocity.magnitude == 0f)
        {
            player.SwitchState(PlayerState.IDLE);
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
        //Player can move mid-air based on air multiplier
        player.data.rb.AddForce(player.data.moveSpeed * player.data.airMultiplier * Time.deltaTime * player.data.moveDirection.normalized, ForceMode.Force);
    }

    private void HandleDrag(PlayerStateManager player)
    {
        //Make player not have any drag when mid-air
        player.data.rb.drag = player.data.airDrag;
    }

    private void GroundCheck(PlayerStateManager player)
    {
        //Check if player is touching the ground
        player.data.isGrounded = Physics.Raycast(player.transform.position, Vector3.down, player.data.playerHeight * 0.5f + 0.2f, player.data.groundLayer);
    }

    private void SpeedControl(PlayerStateManager player)
    {
        //if (player.data.isDashing) return;

        Vector3 flatVelocity = new Vector3(player.data.rb.velocity.x, 0f, player.data.rb.velocity.z);

        //Limit the player's velocity
        if (flatVelocity.magnitude > player.data.maxVelocity)
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

            player.data.maxVelocity -= player.data.dashVelocityReduceTime * Time.deltaTime;

            if (player.data.dashTimer > 2f)
            {
                player.data.maxVelocity = player.data.originalVelocity;
                player.data.isDashing = false;
                player.data.dashTimer = 0f;
            }
        }
    }

    private void GravityControl(PlayerStateManager player)
    {
        //Limit the player's terminal velocity when falling
        if (player.data.rb.velocity.y < -40f)
        {
            player.data.rb.velocity = new Vector3(player.data.rb.velocity.x, -40f, player.data.rb.velocity.z);
        }

        if (player.data.rb.velocity.y < 0f)
        {
            player.data.rb.velocity = new Vector3(player.data.rb.velocity.x, (player.data.rb.velocity.y * 1.002f), player.data.rb.velocity.z);
        }
    }

}
