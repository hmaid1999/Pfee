using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPointer : MonoBehaviour
{
    [SerializeField]
    private Transform target;  // Reference to the target object
    [SerializeField]
    private Transform followObject;  // Reference to the object to follow
    [SerializeField]
    private float arrowDistance = 1.0f;  // Distance from the follow object

    void Update()
    {
        if (target != null && followObject != null)
        {
            // Calculate the direction to the target
            Vector3 directionToTarget = (target.position - followObject.position).normalized;

            // Position the arrow at a distance from the follow object in the direction of the target
            transform.position = followObject.position + directionToTarget * arrowDistance;

            // Update the arrow's rotation to point towards the target
            transform.rotation = Quaternion.LookRotation(directionToTarget);
        }
    }
}