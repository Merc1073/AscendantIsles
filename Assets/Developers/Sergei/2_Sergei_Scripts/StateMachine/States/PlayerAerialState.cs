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
        Dash(player);
        GroundCheck(player);
        HandleDrag(player);
        SpeedControl(player);

        //Allow player to jump if their current jump count does not exceed the total jump count amount
        if (Input.GetKeyDown(KeyCode.Space) && player.data.currentJumpCount < player.data.totalJumpCount)
        {
            player.data.currentJumpCount++;
            Jump(player);
        }

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
        player.data.rb.drag = 0;
    }

    private void GroundCheck(PlayerStateManager player)
    {
        //Check if player is touching the ground
        player.data.isGrounded = Physics.Raycast(player.transform.position, Vector3.down, player.data.playerHeight * 0.5f + 0.2f, player.data.groundLayer);
    }

    private void SpeedControl(PlayerStateManager player)
    {
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
            player.data.rb.velocity = new Vector3(0f, 0f, 0f);
            player.data.currentDashCount++;
            player.data.rb.AddForce(player.data.orientation.forward * player.data.dashForce, ForceMode.Impulse);
        }
    }

}
