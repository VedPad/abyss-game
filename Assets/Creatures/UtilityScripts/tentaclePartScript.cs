using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tentaclePartScript : MonoBehaviour
{
    [HideInInspector]
    public bool isGrabbed;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger == false)
        {
            isGrabbed = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.isTrigger == false)
        {
            isGrabbed = false;
        }
    }
}
