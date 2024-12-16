using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPotion : MonoBehaviour
{

    public sorcererabilities sorcerer;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (sorcerer.healthpotions < 3)
            {
                sorcerer.healthpotions += 1;
                Destroy(gameObject);
            }
        }
    }

}