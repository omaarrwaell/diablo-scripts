using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barbarianrunefragments : MonoBehaviour
{
    public barbarianabilities barbarian;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            barbarian.runefragments += 1;
            Destroy(gameObject);

        }
    }
}
