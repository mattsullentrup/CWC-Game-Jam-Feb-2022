using UnityEngine;
using TMPro;

public class SphereController : MonoBehaviour
{
    public static SphereController Controller { get; set; }
    [SerializeField] private float addTorqueSpeed = 10f;
    [SerializeField] private float addForceSpeed;
    [SerializeField] private float airborneForceSpeed;
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
        isOnGround = true;
        //distToGround = col.bounds.extents.y;
    }

    

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.Manager.isGameActive == true)
        {
            GroundCheck();
            PlayerMovement();
            ConstrainPlayerPosition();
        }
    }

    void GroundCheck() {
   if (Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.5f))
        {
            isOnGround = true;
            ScoreManager.Score.scoreMultiplier = 1;
        }
        else
        {
            isOnGround = false;
            ScoreManager.Score.scoreMultiplier *= Time.deltaTime;
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
        Vector3 forceDir = right * (horizontalInput * turnSpeed) + forward * (verticalInput * forwardSpeed);
        Vector3 torqueDir = right * verticalInput + forward * (-horizontalInput);

        // Set the direction's magnitude to 1 so that it does not interfere with the movement speed
        torqueDir.Normalize();
        //forceDir.Normalize();

        

        // Move the player by the direction multiplied by speed and delta time 
        playerRb.AddTorque(addTorqueSpeed * Time.deltaTime * torqueDir, ForceMode.Acceleration);

        //if (isOnGround == true)
        //{
            playerRb.AddForce(addForceSpeed * Time.deltaTime * forceDir, ForceMode.Acceleration);
        //}
        //else
        //{
        //    playerRb.AddForce(airborneForceSpeed * Time.deltaTime * forceDir, ForceMode.Acceleration);
        //}
    }

    void ConstrainPlayerPosition() //Abstraction
    {
        if (transform.position.z < 0)
        {
            playerRb.AddForce(100000 * Time.deltaTime * Vector3.forward, ForceMode.Acceleration);
        }

        if (transform.position.z > 5000)
        {
            playerRb.AddForce(100000 * Time.deltaTime * Vector3.back, ForceMode.Acceleration);
        }

        if (transform.position.x < 0)
        {
            playerRb.AddForce(100000 * Time.deltaTime * Vector3.right, ForceMode.Acceleration);
        }

        if (transform.position.x > 5000)
        {
            playerRb.AddForce(100000 * Time.deltaTime * Vector3.left, ForceMode.Acceleration);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
            if (other.gameObject.CompareTag("Capsule") && GameManager.Manager.isGameActive == true)
            {
                GameManager.Manager.timeRemaining += 15f;
                Destroy(other.gameObject);
        }
    }
}
