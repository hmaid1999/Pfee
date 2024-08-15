using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BallGameManager : MonoBehaviour
{
    [SerializeField] Transform enviornmentTransform;


    //Update
    //Frames per Seconds = 60 frame per second 


    /// <summary>
    /// 
    /// </summary>
    private void FixedUpdate()
    {
        //Input will be taken from the capture and not from the keyboard
        //10f = speed
        float inputRotationX = Input.GetAxis("Horizontal") * 10f * Time.deltaTime;

        float inputRotationZ = Input.GetAxis("Vertical") * 10f * Time.deltaTime;


        enviornmentTransform.Rotate(inputRotationX, 0f, inputRotationZ);
    }
}
