using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class runefragments : MonoBehaviour
{
    public sorcererabilities sorcerer;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            sorcerer.runefragments += 1;
            Destroy(gameObject);
            
        }
    }
}
