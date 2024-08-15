using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovementSecond : MonoBehaviour
{
    public float speed;
    private Rigidbody rbd;
    public GameObject gameWonScreen;

    private bool allowedToMove;

    void Start()
    {
        rbd = GetComponent<Rigidbody>();

        allowedToMove = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hole"))
        {
            allowedToMove = false;

            gameWonScreen.SetActive(true);

            rbd.velocity = Vector3.zero;
            rbd.angularVelocity = Vector3.zero;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (allowedToMove)
        {
            float vertical = Input.GetAxis("Vertical");
            float horizontal = Input.GetAxis("Horizontal");

            Vector3 force = new Vector3(horizontal, 0.0f, vertical) * speed;

            rbd.AddForce(force, ForceMode.Acceleration);


        }

    }
}
