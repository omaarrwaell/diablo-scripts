using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using UnityEngine.AI;

public class barbarianabilities : MonoBehaviour
{

    [SerializeField]
    Camera _maincamera;


    private float health;
    public float newhealth;


    public Slider healthslider;
    public TMP_Text healthtext;

    public GameObject shield;
    public bool shieldstate;

    public bool maelstorm;
    public bool maelstormstate;

    private bool activatecharge = false;

    public Transform barbariantransform;

    float rotateSpeed = 400f;

    public Animator animator;

    private NavMeshAgent agent;
    private bool bashstate;

    public barbarianfollowtarget goblin;
    public barbarianDemonController demon;


    public bool maelstormactive = false;
    public bool shieldactive = false;
    public bool chargeactive = false;

    public float healthpotions = 0;
    public float runefragments = 0;

    public GameObject portal;

    private string target;

    public barbarianunlockabilities unlockabilities;
    // Start is called before the first frame update
    void Start()
    {

        health = 100f;
        newhealth = 100f;

        shield.SetActive(false);
        shieldstate = false;
        maelstorm = false;
        maelstormstate = false;
        bashstate = false;
        agent = GetComponent<NavMeshAgent>();
        portal.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        healthslider.value = (newhealth / health);

        if (Input.GetKeyDown(KeyCode.W) && unlockabilities.shieldunlocked)
        {
            shield.SetActive(true);
            shieldstate = true;
            activateshield();


            shieldactive = true;
        }

        healthtext.text = newhealth + "/" + health;

        if (Input.GetKeyDown(KeyCode.Q) && unlockabilities.maelstormunlocked)
        {
            maelstorm = true;
            animator.SetBool("maelstorm", true);
            
            activatemaelstorm();

            maelstormactive = true;
        }

        if (Input.GetKeyDown(KeyCode.E) && unlockabilities.chargeunlocked)
        {
            activatecharge = true;
            //agent.speed = 7.0f;
            chargeactive = true;
        }




        if (bashstate)
        {
            if (target == "goblin")
            {
                goblin.newhealth -= 5;
            }
            else if (target == "demon")
            {
                demon.TakeDamage(5);
            }
            animator.SetBool("bash", false);
            bashstate = false;
            
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Check if the clicked object is an enemy
                if (hit.collider.CompareTag("goblin") )
                {
                    goblin = hit.collider.GetComponent<barbarianfollowtarget>();
                    if (goblin.withinrange)
                    {
                        animator.SetBool("bash", true);
                        target = "goblin";
                    }
                    
                }

                if (hit.collider.CompareTag("demon"))
                {
                    demon = hit.collider.GetComponent<barbarianDemonController>();
                    if (demon.withinrange)
                    {
                        animator.SetBool("bash", true);
                        target = "demon";
                        

                    }

                }
            }
        }

            if (agent.remainingDistance < 0.2f && agent.remainingDistance > 0.01f)
        {
            activatecharge = false;
            agent.speed = 5.0f;
        }



        if (maelstorm)
        {
            barbariantransform.Rotate(0, -(rotateSpeed * Time.deltaTime), 0);
            
        }

        if (Input.GetKeyDown(KeyCode.F))
        {

            if (healthpotions > 0)
            {
                healthpotions -= 1;
                newhealth += (health / 2);
            }


        }

        if (runefragments == 3)
        {
            portal.SetActive(true);
        }

        if (newhealth > health)
        {
            newhealth = health;
        }
    }

    void LateUpdate()
    {
        Debug.Log("Maelstorm: " + maelstormstate);
        
        
    }

    void OnTriggerEnter(Collider other)
    {

        if (activatecharge)
        {
            if (other.gameObject.tag == "goblin")
            {
                goblin = other.GetComponent<barbarianfollowtarget>();
                goblin.Die();
            }
            else if (other.gameObject.tag == "demon")
            {
                demon = other.GetComponent<barbarianDemonController>();
                demon.Die();
            }
            else
            {
                Destroy(other.gameObject);
            }
            
            
           
        }
    }

    private void activateshield()
    {
        StartCoroutine(timedshield());
    }

    private IEnumerator timedshield()
    {
        yield return new WaitForSeconds(3);

        shield.SetActive(false);
        shieldstate = false;
    }


    private void activatemaelstorm()
    {
        StartCoroutine(timedmaelstorm());
    }

    private IEnumerator timedmaelstorm()
    {
        yield return new WaitForSeconds(2);

        maelstorm = false;
        animator.SetBool("maelstorm", false);
    }
    

    public void maelstormdamage()
    {
        maelstormstate = true;
    }
    public void antimaelstormdamage()
    {
        maelstormstate = false;
    }

    public void bashdamage()
    {
        bashstate = true;
    }
    
}
