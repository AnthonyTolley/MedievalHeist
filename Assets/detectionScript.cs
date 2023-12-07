using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detectionScript : MonoBehaviour
{
    // Start is called before the first frame update
    private bool playerDetected;
    public bool getPlayerDetected
    {
        get => playerDetected;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerDetected = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerDetected= false;
        }
    }
}
