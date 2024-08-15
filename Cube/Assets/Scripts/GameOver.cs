using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{

    public GameObject GameOverDisplay;
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        GameOverDisplay.SetActive(true);
    }
}
