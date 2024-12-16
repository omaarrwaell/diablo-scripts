using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Linq;

public class barbarianfollowtarget : MonoBehaviour
{
    public barbarianabilities barbarian;

    // Start is called before the first frame update
    public Transform target;
    NavMeshAgent a;

    private Transform goblinposition;

    public Transform healthbartransform;

    public bool isFollowing = false;

    public Animator animator;

    bool run = true;

    public bool withinrange = false;

    private float health;
    public float newhealth;
    public Slider healthslider;


    public bool maelstormgoblin = false;

    public bool dead = false;

    levels level;


    void Start()
    {
        a = GetComponent<NavMeshAgent>();

        GetComponent<NavMeshAgent>().speed = 1.8f;

        barbarian = FindObjectOfType<barbarianabilities>();
        level = barbarian.GetComponent<levels>();

        goblinposition = GetComponent<Transform>();

        health = 20f;
        newhealth = 20f;



    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Health: " + newhealth);
        if (isFollowing)
        {
            
            a.SetDestination(target.position);
            if (run)
            {
                animator.SetBool("runtoplayer", true);
            }

        }
        else if (!isFollowing)
        {
            
            //a.SetDestination(goblinposition.position);
        }

        if (barbarian.maelstormstate)
        {
            maelstormgoblin = true;
        }
        if (barbarian.maelstormstate == false)
        {
            maelstormgoblin = false;
        }

        if (maelstormgoblin && withinrange)
        {
            newhealth -= 5;
            maelstormgoblin = false;
            //barbarian.maelstormstate = false;


        }

        if (newhealth <= 0f)
        {
            Die(); // Handle goblin death (optional)
        }

        healthslider.value = (newhealth / health);
    }

    private void LateUpdate()
    {
        healthbartransform.LookAt(Camera.main.transform.position);
    }

    


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            run = false;
            animator.SetBool("attackplayer", true);
            animator.SetBool("runtoplayer", false);
            withinrange = true;

        }
        /*if (other.gameObject.tag == "clone")
        {
            run = false;
            animator.SetBool("attackplayer", true);
            animator.SetBool("runtoplayer", false);
            Debug.Log("within range");
            withinrange = true;

        }*/
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            run = true;
            animator.SetBool("attackplayer", false);
            withinrange = false;
        }

        /*if (other.gameObject.tag == "clone")
        {
            run = true;
            animator.SetBool("attackplayer", false);
            Debug.Log("not within range");
            withinrange = false;

        }*/
    }

    public void damagePlayer()
    {
        if (barbarian.shieldstate == false && withinrange )
        {
            barbarian.newhealth -= 5;
        }
        
    }

    public void GobTakeDamage(float damage)
    {
        newhealth -= damage;
        Debug.Log("Goblin health: " + newhealth);
        if (newhealth <= 0f)
        {
            Die(); // Handle goblin death (optional)
        }
    }

    public void Die()
    {
        dead = true;
        // Handle goblin death (e.g., play animation, destroy object, etc.)
        level.increaseXp(10);
        Destroy(gameObject);
    }

    
}
