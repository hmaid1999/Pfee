using UnityEngine;

public class PlayerMovementBLE : MonoBehaviour
{
    public Rigidbody rb;
    public float forwardForce = 2000f;
    public float sidewaysForce = 500f;

    private ESP32BLEApp esp32BLEApp;

    void Start()
    {
        // Ensure Rigidbody is attached
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }

        if (rb == null)
        {
            Debug.LogError("Rigidbody component is missing from the game object.");
            return;
        }

        // Find the ESP32BLEApp instance in the scene
        esp32BLEApp = FindObjectOfType<ESP32BLEApp>();

        if (esp32BLEApp == null)
        {
            Debug.LogError("ESP32BLEApp instance not found in the scene.");
        }
    }

    void FixedUpdate()
    {
        // Add a forward force
        rb.AddForce(0, 0, forwardForce * Time.deltaTime);

        if (esp32BLEApp != null)
        {
            float angleX1 = esp32BLEApp.GetAngleX1(); // Get the X1 angle from ESP32BLEApp
            float angleX2 = esp32BLEApp.GetAngleX2(); // Get the X2 angle from ESP32BLEApp
            MoveBasedOnAngle(angleX1, angleX2);
        }

        if (rb.position.y < -1f)
        {
            FindObjectOfType<GameManager>().EndGame();
        }
    }

    void MoveBasedOnAngle(float angleX1 , float angleX2)
    {
        // Adjust the threshold values as needed
        if (angleX1 < 80) // Threshold to determine movement to the right
        {
            rb.AddForce(sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }
        else if (angleX2 > 90) // Threshold to determine movement to the left
        {
            rb.AddForce(-sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }
    }
}
