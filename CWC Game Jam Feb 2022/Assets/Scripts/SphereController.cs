using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SphereController : MonoBehaviour
{
    [SerializeField] private float torqueSpeed = 10f;
    [SerializeField] private float forceSpeed;
    [SerializeField] private float jumpForce = 3;
    [SerializeField] private float speed;
    public float gravityModifier;
    public bool isOnGround = true;
    private Rigidbody playerRb;
    private Camera cam;
    [SerializeField] TextMeshProUGUI speedometerText;
    public SphereCollider col;
    public LayerMask groundLayers;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        cam = Camera.main;
        col = GetComponent<SphereCollider>();
        Physics.gravity *= gravityModifier;
    }

    

    // Update is called once per frame
    void FixedUpdate()
    {
        PlayerMovement();
        Jump();

        speed = Mathf.Round(playerRb.velocity.magnitude * 2.237f);
        speedometerText.SetText("Speed: " + speed + "mph");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
        else
        {
            isOnGround = false;
        }   
    }

    private void Jump()
    {
        //isOnGrounded = Physics.CheckCapsule(col.bounds.center, new Vector3(col.bounds.center.x, col.bounds.min.y, col.bounds.center.z), col.radius * .9f, groundLayers);

        if (Input.GetButtonDown("Jump") && isOnGround)
        {
            playerRb.AddForce(jumpForce * Time.deltaTime * Vector3.up, ForceMode.VelocityChange);
            isOnGround = false;
        }
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
        Vector3 forceDir = right * horizontalInput + forward * verticalInput;
        Vector3 torqueDir = right * verticalInput + forward * (-horizontalInput);

        // Set the direction's magnitude to 1 so that it does not interfere with the movement speed
        torqueDir.Normalize();

        // Move the player by the direction multiplied by speed and delta time 
        playerRb.AddTorque(torqueSpeed * Time.deltaTime * torqueDir, ForceMode.Acceleration);

        if (isOnGround == true)
        {
            playerRb.AddForce(forceSpeed * Time.deltaTime * forceDir, ForceMode.Acceleration);
        }
        else
        {
            playerRb.AddForce((forceSpeed/8) * Time.deltaTime * forceDir, ForceMode.Acceleration);
        }
    }
}
