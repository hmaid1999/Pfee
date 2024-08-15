using UnityEngine;

namespace FirstGame
{
	public class PlayerMovement : MonoBehaviour
	{

		// This is a reference to the Rigidbody component called "rb"
		public Rigidbody rb;

		public float forwardForce = 2000f;  // Variable that determines the forward force
		public float sidewaysForce = 500f;  // Variable that determines the sideways force

		// We marked this as "Fixed"Update because we
		// are using it to mess with physics.
		void FixedUpdate()
		{

			Debug.Log("Fixed Update");
			// Add a forward force
			rb.AddForce(0, 0, forwardForce * Time.deltaTime);



			if (Input.GetKey(KeyCode.D))
			{
				// Add a force to the right
				rb.AddForce(sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);

				Debug.Log("Moving Right");
			}

			if (Input.GetKey(KeyCode.S))
			{
				// Add a force to the left
				rb.AddForce(-sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);

				Debug.Log("Moving Left");
			}

			if (rb.position.y < -1f)
			{
				FindObjectOfType<GameManager>().EndGame();
			}
		}
	} 
}
