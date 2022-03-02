using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField] private float jumpForce = 3;
    public float gravityModifier;
    //public bool jumpAvailable;

    // Start is called before the first frame update
    void Awake()
    {
        Physics.gravity = new Vector3(0, -9.8F, 0);
        Physics.gravity *= gravityModifier;
    }

    private void Start()
    {
        //jumpAvailable = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PlayerJump();
    }

    private void PlayerJump()
    {
        if (Input.GetButtonDown("Jump") && SphereController.Controller.isOnGround == true)
        {
            SphereController.Controller.playerRb.AddForce(jumpForce * Time.deltaTime * Vector3.up, ForceMode.VelocityChange);
            //jumpAvailable = false;
        }
    }

    //private void OnCollisionEnter(Collision other)
    //{
    //    if (other.gameObject.CompareTag("Ground") && GameManager.Manager.isGameActive == true)
    //    {
    //        jumpAvailable = true;
    //    }
    //}
}
