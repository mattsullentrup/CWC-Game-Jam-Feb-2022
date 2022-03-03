using UnityEngine;
using TMPro;

public class SphereController : MonoBehaviour
{
    public static SphereController Controller { get; set; }
    [SerializeField] private float addTorqueSpeed = 10f;
    [SerializeField] private float addForceSpeed;
    //[SerializeField] private float airborneForceSpeed;
    [SerializeField] private float turnSpeed = 5;
    [SerializeField] private float forwardSpeed = 2;
    public bool isOnGround = true;
    public Rigidbody playerRb;
    private Camera cam;

    public SphereCollider col;
    public LayerMask groundLayers;
    public float distToGround = 1f;

    private void Awake()
    {

        if (Controller != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Controller = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        cam = Camera.main;
        col = GetComponent<SphereCollider>();
        //isOnGround = true;
        //distToGround = col.bounds.extents.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.Manager.isGameActive == true)
        {
            GroundCheck();
            PlayerMovement();
        }
    }

    void GroundCheck()
    {
        if (Physics.Raycast(transform.position, Vector3.down, distToGround + 4f))
        {
            isOnGround = true;
            ScoreManager.Score.scoreMultiplier = 1;
        }
        else
        {
            isOnGround = false;
            ScoreManager.Score.scoreMultiplier += Time.deltaTime * 5;
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

        //if (isOnGround == true)
        //{
        //    forwardSpeed = 3;
        //    turnSpeed = 4;
        //}
        //else
        //{
        //    forwardSpeed = 1;
        //    turnSpeed = 2;
        //}

        // Set the direction for the player to move
        Vector3 forceDir = right * (horizontalInput * turnSpeed) + forward * (verticalInput * forwardSpeed);
        Vector3 torqueDir = right * verticalInput + forward * (-horizontalInput);

        // Set the direction's magnitude to 1 so that it does not interfere with the movement speed
        torqueDir.Normalize();
        forceDir.Normalize();



        // Move the player by the direction multiplied by speed and delta time 
        playerRb.AddTorque(addTorqueSpeed * Time.deltaTime * torqueDir, ForceMode.Acceleration);
        playerRb.AddForce(addForceSpeed * Time.deltaTime * forceDir, ForceMode.Acceleration);

        
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Cube") && GameManager.Manager.isGameActive == true)
        {
            GameManager.Manager.timeRemaining += 15f;
            ScoreManager.Score.score += 1000;
            Destroy(other.gameObject);
        }
    }
}
