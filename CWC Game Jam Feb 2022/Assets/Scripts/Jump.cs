using UnityEngine;
using UnityEngine.UI;

public class Jump : MonoBehaviour
{
    public static Jump JumpScript { get; set; }
    [SerializeField] private float jumpForce = 3;
    //public float gravityModifier;
    public bool jumpAvailable;
    public RawImage jumpIndicator;

    Rigidbody rb;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    // Start is called before the first frame update
    void Awake()
    {
        Physics.gravity = new Vector3(0, -9.81F, 0);
        //Physics.gravity *= gravityModifier;

        if (JumpScript != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            JumpScript = this;
        }

        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        jumpAvailable = true;
    }

    private void Update()
    {
        MarioJump();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PlayerJump();
    }

    private void MarioJump()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += (fallMultiplier - 1) * Physics.gravity.y * Time.deltaTime * Vector3.up;
        }
        else if (rb.velocity.y > 0)
        {
            rb.velocity += (lowJumpMultiplier - 1) * Physics.gravity.y * Time.deltaTime * Vector3.up;
        }
    }

    private void PlayerJump()
    {
        if (Input.GetButton("Jump") && jumpAvailable == true)
        {
            SphereController.Controller.playerRb.AddForce(jumpForce * Time.deltaTime * Vector3.up, ForceMode.VelocityChange);
            jumpAvailable = false;
            jumpIndicator.gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground") && GameManager.Manager.isGameActive == true)
        {
            jumpAvailable = true;
            jumpIndicator.gameObject.SetActive(true);
        }
    }
}
