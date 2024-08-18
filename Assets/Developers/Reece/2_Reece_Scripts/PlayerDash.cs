using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour

{

    private Rigidbody rb;
    private bool isDashing = true;
    public int totalDashCount = 2;
    public int currentDashCount = 0;
    public float dashSpeed;
    public float dashTimer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // StartCoroutine(DashReset());
    }

    // Update is called once per frame
    void Update()
    {
        if (currentDashCount > 0) 
        {
            dashTimer += Time.deltaTime;
            if (dashTimer >= 5f)
            {
                dashTimer = 0f;
                currentDashCount--;
            }
        }

        // Gets the current amount of dashes stored and compares them to the total amount of times you can dash.
        if (Input.GetKeyDown(KeyCode.LeftShift) && currentDashCount < totalDashCount)
        {
            currentDashCount++;
            rb.AddForce(transform.forward * dashSpeed, ForceMode.Impulse);
        }

    }

    //IEnumerator DashReset()
    //{
    //    while(true)
    //        if (currentDashCount < totalDashCount)
    //        {
    //            currentDashCount--;
    //            yield return new WaitForSeconds(5f);
    //        }
    //}
}




/* after 30 seconds If currentDashCount < totalDashCount
 *      currentDashCount++; 
*/ 