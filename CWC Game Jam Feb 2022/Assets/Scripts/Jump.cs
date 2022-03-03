using UnityEngine;
using UnityEngine.UI;

public class Jump : MonoBehaviour
{
    public static Jump JumpScript { get; set; }
    [SerializeField] private float jumpForce = 3;
    public float gravityModifier;
    public bool jumpAvailable;
    public RawImage jumpIndicator;

    // Start is called before the first frame update
    void Awake()
    {
        Physics.gravity = new Vector3(0, -9.8F, 0);
        Physics.gravity *= gravityModifier;

        if (JumpScript != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            JumpScript = this;
        }
    }

    private void Start()
    {
        jumpAvailable = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PlayerJump();
    }

    private void PlayerJump()
    {
        if (Input.GetButtonDown("Jump") && jumpAvailable == true)
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
