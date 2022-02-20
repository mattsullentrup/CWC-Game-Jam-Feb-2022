using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SphereController : MonoBehaviour
{
    public static SphereController Controller { get; set; }
    [SerializeField] private float addTorqueSpeed = 10f;
    [SerializeField] private float addForceSpeed;
    [SerializeField] private float airborneForceSpeed;
    [SerializeField] private bool countAvailable;
    [SerializeField] private float jumpForce = 3;
    public float score = 0;
    [SerializeField] TextMeshProUGUI scoreText;
    public float gravityModifier;
    public bool isOnGround = true;
    private Rigidbody playerRb;
    private Camera cam;
   
    public SphereCollider col;
    public LayerMask groundLayers;
    public float distToGround;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        cam = Camera.main;
        col = GetComponent<SphereCollider>();
        Physics.gravity *= gravityModifier;
        
        distToGround = col.bounds.extents.y;
    }

    private void Update()
    {
        if (GameManager.Manager.isGameActive == true)
        {
            CountScore();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.Manager.isGameActive == true)
        {
            GroundCheck();
            PlayerMovement();
            Jump();
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Ground"))
    //    {
    //        isOnGround = true;
    //        //StopCoroutine(CountScore());
    //    } 
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    isOnGround = false;
    //    //StartCoroutine(CountScore());
    //}

    //IEnumerator CountScore()
    //{
    //    if (isOnGround == false)
    //    {
    //        yield return new WaitForSecondsRealtime(0.5f);
    //        score += (Time.deltaTime * GameManager.Manager.playerSpeed);
    //        scoreText.text = "Score: " + score;
    //    }
    //}

    void GroundCheck() {
   if (Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.5f))
        {
            isOnGround = true;
        }
        else
        {
            isOnGround = false;
        }
 }

void CountScore()
    {
        if (isOnGround == false)
        {
            score += (Time.deltaTime * GameManager.Manager.playerSpeed);
            scoreText.text = "Score: " + Mathf.Round(score);
        }
    }



    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && isOnGround)
        {
            playerRb.AddForce(jumpForce * Time.deltaTime * Vector3.up, ForceMode.VelocityChange);
            //isOnGround = false;
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
        forceDir.Normalize();

        

        // Move the player by the direction multiplied by speed and delta time 
        playerRb.AddTorque(addTorqueSpeed * Time.deltaTime * torqueDir, ForceMode.Acceleration);

        if (isOnGround == true)
        {
            playerRb.AddForce(addForceSpeed * Time.deltaTime * forceDir, ForceMode.Acceleration);
        }
        else
        {
            playerRb.AddForce(airborneForceSpeed * Time.deltaTime * forceDir, ForceMode.Acceleration);
        }
    }
}
