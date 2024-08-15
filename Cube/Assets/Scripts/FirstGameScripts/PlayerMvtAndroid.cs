using UnityEngine;

public class PlayerMovementAndroid : MonoBehaviour
{

    // This is a reference to the Rigidbody component called "rb"
    public Rigidbody rb;

    public float forwardForce = 2000f;  // Variable that determines the forward force
    public float sidewaysForce = 500f;  // Variable that determines the sideways force

    // Update is called once per frame
    void FixedUpdate()
    {
        // Add a forward force
        rb.AddForce(0, 0, forwardForce * Time.deltaTime);

        // Check for touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Check if the touch is in the moving phase
            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                // Get the touch position
                Vector2 touchPosition = touch.position;

                // Determine if the touch is on the left or right side of the screen
                if (touchPosition.x < Screen.width / 2)
                {
                    // Add a force to the left
                    rb.AddForce(-sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
                }
                else if (touchPosition.x > Screen.width / 2)
                {
                    // Add a force to the right
                    rb.AddForce(sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
                }
            }
        }

        // Check if the player has fallen off the platform
        if (rb.position.y < -1f)
        {
            FindObjectOfType<GameManager>().EndGame();
        }
    }
}
