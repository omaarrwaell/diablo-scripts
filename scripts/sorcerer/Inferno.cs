using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inferno : MonoBehaviour
{
    public float effectRadius = 0.2f;  // Radius of the Inferno effect
    public float damagePerSecond = 2f; // Damage dealt to goblins per second
    public float duration = 15f;        // Duration of Inferno effect

    private float timeSinceLastDamage = 0f; // Track time passed since last damage

    private void OnTriggerEnter(Collider collision)
    {
        //print("gowa el collision");
        // Check if the fireball collided with a goblin
        if (collision.CompareTag("goblin"))
        {
           // print("inside if destroy");

            followtarget goblin = collision.GetComponent<followtarget>();
            
           
           
            goblin.newhealth -= 10;
            //demon.TakeDamage(10f);

        }
        else if (collision.CompareTag("demon"))
        {
            DemonController demon = collision.GetComponent<DemonController>();

            if (demon != null)
            {
                Debug.Log($"Demon Health: {demon.currentHealth}");
                demon.TakeDamage(10f);
            }

        }
    }

    private void OnTriggerStay(Collider collision)
    {
        // Increment the time passed in the trigger area
        timeSinceLastDamage += Time.deltaTime;

        // Only apply damage every 1 second
        if (timeSinceLastDamage >= 2f)
        {
           // print("gowa el collision stayyy");
            // Check if the fireball collided with a goblin
            if (collision.CompareTag("goblin"))
            {
               // print("inside if destroy");

                followtarget goblin = collision.GetComponent<followtarget>();
                goblin.GobTakeDamage(2); // Apply damage
                
               

                // Reset the timer after dealing damage
                timeSinceLastDamage = 0f;
            }
            else if (collision.CompareTag("demon"))
            {
                DemonController demon = collision.GetComponent<DemonController>();
                if (demon != null)
                {
                    //Debug.Log($"Demon Health: {demon.currentHealth}");
                    demon.TakeDamage(10f);
                }

                timeSinceLastDamage = 0f;

            }
        }
    }
}
