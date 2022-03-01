using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour
{
    private Rigidbody playerRb;
    [SerializeField] private int southBoundaryZ = 350;
    [SerializeField] private int northBoundaryZ = 4400;
    [SerializeField] private int westBoundaryX = 250;
    [SerializeField] private int eastBoundaryX = 4250;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        ConstrainPlayerPosition();
    }

    void ConstrainPlayerPosition() //Abstraction
    {
        if (transform.position.z < southBoundaryZ)
        {
            playerRb.AddForce(100000 * Time.deltaTime * Vector3.forward, ForceMode.Acceleration);
        }

        if (transform.position.z > northBoundaryZ)
        {
            playerRb.AddForce(100000 * Time.deltaTime * Vector3.back, ForceMode.Acceleration);
        }

        if (transform.position.x < westBoundaryX)
        {
            playerRb.AddForce(100000 * Time.deltaTime * Vector3.right, ForceMode.Acceleration);
        }

        if (transform.position.x > eastBoundaryX)
        {
            playerRb.AddForce(100000 * Time.deltaTime * Vector3.left, ForceMode.Acceleration);
        }
    }
}
