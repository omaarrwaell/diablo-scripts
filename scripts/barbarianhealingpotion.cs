using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarbarianHealingPotion : MonoBehaviour
{

    public barbarianabilities barbarian;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (barbarian.healthpotions < 3)
            {
                barbarian.healthpotions += 1;
                Destroy(gameObject);
            }
        }
    }

}