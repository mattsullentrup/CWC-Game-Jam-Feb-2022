using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereController : MonoBehaviour
{
    public float playerSpeed = 10f;
    private Rigidbody playerRb;
    private Camera cam;

    public float jumpForce = 3;
    public SphereCollider col;
    public LayerMask groundLayers;
    bool isGrounded;


    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        cam = Camera.main;
        col = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        //jump

        isGrounded = Physics.CheckCapsule(col.bounds.center, new Vector3(col.bounds.center.x, col.bounds.min.y, col.bounds.center.z), col.radius * .9f, groundLayers);

        if (Input.GetButtonDown("Jump") && isGrounded)

        {

            playerRb.AddForce(jumpForce * Time.deltaTime * Vector3.up, ForceMode.Impulse);

        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PlayerMovement();
    }

    // Moves the player
    void PlayerMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //Vector3 movement = new Vector3(horizontalInput, 0, verticalInput).normalized;
        //playerRb.AddForce(playerSpeed * Time.fixedDeltaTime * movement, ForceMode.Acceleration);

        // Get directions relative to camera
        Vector3 forward = cam.transform.forward;
        Vector3 right = cam.transform.right;

        // Project forward and right direction on the horizontal plane (not up and down), then
        // normalize to get magnitude of 1
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        // Set the direction for the player to move
        //Vector3 dir = right * horizontalInput + forward * verticalInput;
        Vector3 dir = right * verticalInput + forward * (-horizontalInput);

        // Set the direction's magnitude to 1 so that it does not interfere with the movement speed
        dir.Normalize();

        // Move the player by the direction multiplied by speed and delta time 
        playerRb.AddTorque(playerSpeed * Time.deltaTime * dir, ForceMode.Acceleration);

        // Set rotation to direction of movement if moving
        //if (dir != Vector3.zero)
        //{
        //    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(forward), 0.2f);
        //}
    }
}
