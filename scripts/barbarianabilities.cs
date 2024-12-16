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

    }

    // Update is called once per frame
    void Update()
    {
        healthslider.value = (newhealth / health);

        if (Input.GetKeyDown(KeyCode.W))
        {
            shield.SetActive(true);
            shieldstate = true;
            activateshield();
        }

        healthtext.text = newhealth + "/" + health;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            maelstorm = true;
            animator.SetBool("maelstorm", true);
            activatemaelstorm();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            activatecharge = true;
            //agent.speed = 7.0f;
        }
        if (bashstate)
        {
            goblin.newhealth -= 5;
            bashstate = false;
            animator.SetBool("bash", false);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Check if the clicked object is an enemy
                if (hit.collider.CompareTag("goblin"))
                {
                    goblin = hit.collider.GetComponent<barbarianfollowtarget>();
                    if (goblin.withinrange)
                    {
                        animator.SetBool("bash", true);
                        
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
    }

    void OnTriggerEnter(Collider other)
    {

        if (activatecharge)
        {
            if (other.gameObject.tag != "goblin")
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

    public void bashdamage()
    {
        bashstate = true;
    }
}
