using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{

    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Player is IDLE.");
    }

    public override void UpdateState(PlayerStateManager player)
    {

        GroundCheck(player);
        Dash(player);

        //Reset the player jump count so they can jump again
        if (player.data.isGrounded && !Input.GetKey(KeyCode.Space))
        {
            player.data.currentJumpCount = 0;
        }

        //Switch state to MOVING if player inputs a movement key
        if (player.data.moveHorizontal != 0f || player.data.moveVertical != 0f)
        {
            player.SwitchState(PlayerState.MOVING);
        }

        //Switch state to MOVING if player's velocity does not equal 0
        if (player.data.rb.velocity != new Vector3(0, 0, 0))
        {
            player.SwitchState(PlayerState.MOVING);
        }

        //Switch state to AERIAL if player presses jump key
        if (Input.GetKeyDown(KeyCode.Space) && player.data.coyoteTimeCounter > 0f && player.data.currentJumpCount < player.data.totalJumpCount)
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

    private void GroundCheck(PlayerStateManager player)
    {
        //Check if player is touching the ground
        player.data.isGrounded = Physics.Raycast(player.transform.position, Vector3.down, player.data.playerHeight * 0.5f + 0.2f, player.data.groundLayer);
    }

    private void Jump(PlayerStateManager player)
    {
        //Reset your Y velocity
        player.data.rb.velocity = new Vector3(player.data.rb.velocity.x, 0f, player.data.rb.velocity.z);

        //Give player upwards impulse force to jump
        player.data.rb.AddForce(player.transform.up * player.data.jumpForce, ForceMode.Impulse);
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
