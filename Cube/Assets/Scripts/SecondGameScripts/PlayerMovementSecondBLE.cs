using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementSecondBLE : MonoBehaviour
{
    public float speed;
    private Rigidbody rbd;
    public GameObject gameWonScreen;

    public float sidewaysForce;
   

    private bool allowedToMove;

    private ESP32BLEApp esp32BLEApp;

    void Start()
    {
        rbd = GetComponent<Rigidbody>();

        allowedToMove = true;

        // Find the ESP32BLEApp instance in the scene
        esp32BLEApp = FindObjectOfType<ESP32BLEApp>();

        if (esp32BLEApp == null)
        {
            Debug.LogError("ESP32BLEApp instance not found in the scene.");
        }
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

    void FixedUpdate()
    {
        if (esp32BLEApp != null && allowedToMove)
        {
            float angleX1 = esp32BLEApp.GetAngleX1(); // Get the X1 angle from ESP32BLEApp
            float angleX2 = esp32BLEApp.GetAngleX2(); // Get the X2 angle from ESP32BLEApp
            MoveBasedOnAngle(angleX1, angleX2);
        }
    }

    void MoveBasedOnAngle(float angleX1, float angleX2)
    {
        // Adjust the threshold values as needed
        if (angleX1 < 80) // Threshold to determine movement to the right
        {
            rbd.AddForce(Vector3.right * sidewaysForce * Time.deltaTime, ForceMode.VelocityChange);
        }
        else if (angleX1 > 90 && angleX1 <= 100) // Threshold to determine movement to the left
        {
            rbd.AddForce(Vector3.left * sidewaysForce * Time.deltaTime, ForceMode.VelocityChange);
        }
        else if (angleX1 > 100 && angleX1 <= 110) // Threshold to determine movement forward
        {
            rbd.AddForce(Vector3.forward * sidewaysForce * Time.deltaTime, ForceMode.VelocityChange);
        }
        else if (angleX1 > 110) // Threshold to determine movement backward
        {
            rbd.AddForce(Vector3.back * sidewaysForce * Time.deltaTime, ForceMode.VelocityChange);
        }
    }
}
