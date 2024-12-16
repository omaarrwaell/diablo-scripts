using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class sorcererabilities : MonoBehaviour
{
    bool teleport = false;
    [SerializeField]
    Camera _maincamera;

    public GameObject cloneskeleton;
    private UnityEngine.AI.NavMeshAgent agent;

    private float health;
    public float newhealth;

    public Slider healthslider;
    public TMP_Text healthtext;

    

    public bool detonated = false;
    public bool exploded = false;

    public sorcererunlockabilities unlockabilities;

    public bool teleportactive = false;
    public bool cloneactive = false;

    public float healthpotions = 0;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        health = 100f;
        newhealth = 100f;
       
    }

    // Update is called once per frame
    void Update()
    {
        healthslider.value = (newhealth / health);

        if (Input.GetKeyDown(KeyCode.W) && unlockabilities.teleportunlocked)
        {
            teleport = true;
        }
        if (Input.GetMouseButtonDown(1) && teleport)
        {
            Ray ray = _maincamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                agent.Warp(hit.point);
            }
            teleport = false;
            teleportactive = true;
        }
        if (exploded)
        {
            cloneskeleton.SetActive(false);
        }


        


        healthtext.text = newhealth + "/" + health; 

        if (Input.GetKeyDown(KeyCode.Q) && unlockabilities.cloneunlocked)
        {
            cloneskeleton.SetActive(true);
            cloneskeleton.transform.position = transform.position;
            cloneskeleton.transform.rotation = transform.rotation;

            detonated = true;

            explodeclone();


            
        }
        if (Input.GetKeyDown(KeyCode.F))
        {

            if (healthpotions > 0)
            {
                healthpotions -= 1;
                newhealth += (health / 2);
            }


        }

        if (newhealth > health)
        {
            newhealth = health;
        }


    }

    private void explodeclone()
    {
        cloneactive = true;
        StartCoroutine(timedexplosion());
    }

    private IEnumerator timedexplosion()
    {
        yield return new WaitForSeconds(5);
        
        exploded = true;
        detonated = false;

        cloneskeleton.transform.position = new Vector3(0,0,0);
    }
    /*private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "goblin")
        {
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "goblin")
        {
            
        }
    }*/



}
