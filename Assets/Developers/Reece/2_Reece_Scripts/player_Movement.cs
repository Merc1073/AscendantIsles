using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_Movement : MonoBehaviour
{
    private Rigidbody rb;

    private float moveHorizontal;
    private float moveVertical;

    [SerializeField]
    private float moveSpeed = 1f;

    Vector3 movement;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        transform.position = new Vector3(2, 4, 5);
    }

    // Update is called once per frame
    void Update()
    {
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        movement = new Vector3(moveHorizontal, 0, moveVertical);

        // transform.position += movement * Time.deltaTime * moveSpeed;

        rb.AddForce(movement * Time.deltaTime * moveSpeed);
    }
}
/* i love doing
 * comments this way
 * :P */
