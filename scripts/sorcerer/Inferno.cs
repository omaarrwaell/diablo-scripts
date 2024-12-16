using System.Collections.Generic;
using UnityEngine;

public class Inferno : MonoBehaviour
{
    public float effectRadius = 0.2f;   // Radius of the Inferno effect
    public float damagePerSecond = 2f; // Periodic damage dealt per second
    public float duration = 15f;       // Duration of the Inferno effect

    private Dictionary<Collider, float> timeTrackers = new Dictionary<Collider, float>(); // Track individual timers

    private void OnTriggerEnter(Collider collision)
    {
        // Deal initial damage upon entering
        if (collision.CompareTag("goblin"))
        {
            followtarget goblin = collision.GetComponent<followtarget>();
            if (goblin != null)
            {
                goblin.GobTakeDamage(10f); // Initial damage
                Debug.Log($"Goblin entered Inferno. Health: {goblin.newhealth}");
            }
        }
        else if (collision.CompareTag("demon"))
        {
            DemonController demon = collision.GetComponent<DemonController>();
            if (demon != null)
            {
                demon.TakeDamage(10f); // Initial damage
                Debug.Log($"Demon entered Inferno. Health: {demon.currentHealth}");
            }
        }
        else if (collision.CompareTag("boss"))
        {
            LilithController lilith = collision.GetComponent<LilithController>();
            if (lilith != null)
            {
                lilith.TakeDamage(10f); // Initial damage
                Debug.Log($"Lilith entered Inferno. Health: {lilith.currentHealth}");
            }
        }

        // Add to the time tracker for periodic damage
        if (!timeTrackers.ContainsKey(collision))
        {
            timeTrackers[collision] = 0f; // Initialize timer for this object
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        // Update timer for this specific object
        if (timeTrackers.ContainsKey(collision))
        {
            timeTrackers[collision] += Time.deltaTime;

            // Apply periodic damage every second
            if (timeTrackers[collision] >= 1f)
            {
                if (collision.CompareTag("goblin"))
                {
                    followtarget goblin = collision.GetComponent<followtarget>();
                    if (goblin != null)
                    {
                        goblin.GobTakeDamage(damagePerSecond);
                        Debug.Log($"Goblin takes periodic Inferno damage. Health: {goblin.newhealth}");
                    }
                }
                else if (collision.CompareTag("demon"))
                {
                    DemonController demon = collision.GetComponent<DemonController>();
                    if (demon != null)
                    {
                        demon.TakeDamage(damagePerSecond);
                        Debug.Log($"Demon takes periodic Inferno damage. Health: {demon.currentHealth}");
                    }
                }
                else if (collision.CompareTag("boss"))
                {
                    LilithController lilith = collision.GetComponent<LilithController>();
                    if (lilith != null)
                    {
                        lilith.TakeDamage(damagePerSecond);
                        Debug.Log($"Lilith takes periodic Inferno damage. Health: {lilith.currentHealth}");
                    }
                }

                timeTrackers[collision] = 0f; // Reset timer for this object
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        // Remove object from the time tracker when it exits the Inferno
        if (timeTrackers.ContainsKey(collision))
        {
            timeTrackers.Remove(collision);
            Debug.Log($"{collision.tag} left the Inferno.");
        }
    }
}
