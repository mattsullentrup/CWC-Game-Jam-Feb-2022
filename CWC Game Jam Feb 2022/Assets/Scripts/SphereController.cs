using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SphereController : MonoBehaviour
{
    [SerializeField] private float addTorqueSpeed = 10f;
    [SerializeField] private float addForceSpeed;
    [SerializeField] private float airborneForceSpeed;
    [SerializeField] private bool countAvailable;
    //[SerializeField] private float jumpForce = 3;
    public float score = 0;
    [SerializeField] TextMeshProUGUI scoreText;
    public float gravityModifier;
    public bool isOnGround = true;
    private Rigidbody playerRb;
    private Camera cam;
    //private GameManager gameManager;
    public SphereCollider col;
    public LayerMask groundLayers;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        cam = Camera.main;
        col = GetComponent<SphereCollider>();
        Physics.gravity *= gravityModifier;
        //gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    private void Update()
    {
        StartCoroutine(CountScore());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PlayerMovement();
        //Jump();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            //StopCoroutine(CountScore());
        } 
    }

    private void OnCollisionExit(Collision collision)
    {
        isOnGround = false;
        //StartCoroutine(CountScore());
    }

    IEnumerator CountScore()
    {
        if (isOnGround == false)
        {
            yield return new WaitForSecondsRealtime(0.5f);
            score += (Time.deltaTime * GameManager.Manager.playerSpeed);
            scoreText.text = "Score: " + score;
        }
    }

    //void CountScore()
    //{
    //    if (countAvailable == true && isOnGround == false)
    //    {
    //        score *= Time.deltaTime;
    //        scoreText.text = "Score: " + Mathf.Round(score);
    //    }
    //}

    //IEnumerator WaitToCount()
    //{
    //    countAvailable = false;
    //    yield return new WaitForSeconds(0.25f);
    //    countAvailable = true;
    //}

    //private void Jump()
    //{
    //    if (Input.GetButtonDown("Jump") && isOnGround)
    //    {
    //        playerRb.AddForce(jumpForce * Time.deltaTime * Vector3.up, ForceMode.VelocityChange);
    //        isOnGround = false;
    //    }
    //}

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
