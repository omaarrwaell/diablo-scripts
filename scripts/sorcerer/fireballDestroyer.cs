using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireballDestroyer : MonoBehaviour
{
    void Update()
    {
        destroyfireball();
    }

    private void destroyfireball()
    {
        StartCoroutine(destroytimer());
    }

    public IEnumerator destroytimer()
    {
        yield return new WaitForSeconds(0.9f);
        Destroy(gameObject);
    }


    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {

        // Check if the fireball collided with a goblin

        if (collision.collider.CompareTag("goblin"))

        {


            followtarget goblin = collision.collider.GetComponent<followtarget>();
            //DemonController demon = collision.collider.GetComponent<DemonController>();
            goblin.newhealth -= 5;
            //demon.TakeDamage(5f);
            if (goblin.newhealth <= 0)
            {
                goblin.Die();
            }


            // Destroy the fireball
            Destroy(gameObject);
        }

        else if (collision.collider.CompareTag("demon")){
            DemonController demon = collision.collider.GetComponent<DemonController>();
            demon.TakeDamage(5f);

            Destroy(gameObject);
        }
        //else if (!collision.collider.CompareTag("terrain"))
        //{
        //    print(collision.collider);
        //    Destroy(gameObject);
        //}

       
    }
}
