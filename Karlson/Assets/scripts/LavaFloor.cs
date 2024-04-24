using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaFloor : MonoBehaviour
{
    public int lavaDamage = 10; // Lava damage per tick

    private float lavaTimer = 0.0f;
    private bool isInLava = false;

    private void Update()
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInLava = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInLava = false;
        }
    }
}


