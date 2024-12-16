using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class followtarget : MonoBehaviour
{
    private LilithController lilithController;

    public sorcererabilities sorcerer;


    // Start is called before the first frame update
    public Transform target;
    NavMeshAgent a;
    levels level;
    

    private Transform goblinposition;

    public Transform healthbartransform;

    public bool isFollowing = false;

    Animator animator;

    bool run = true;
    public bool exploded = false;

    private bool withinrange = false;

    private float health;
    public float newhealth;
    public Slider healthslider;
    public AudioSource sfxAudioSource;

    public AudioClip GobDies;


    void Start()
    {
        lilithController = FindObjectOfType<LilithController>();
        Scene currentScene = SceneManager.GetActiveScene();
        print(currentScene);
        if (currentScene.name.Equals("bossLevel"))
        {
            isFollowing = true;
        }
        a = GetComponent<NavMeshAgent>();
        sorcerer = FindObjectOfType<sorcererabilities>();
        level =sorcerer.GetComponent<levels>();
        GameObject obj = GameObject.Find("SkeletonMage");
        target = obj.GetComponent<Transform>();

        GetComponent<NavMeshAgent>().speed = 0.9f;

        goblinposition = GetComponent<Transform>();

        health = 20f;
        newhealth = 20f;
        animator = GetComponentInChildren<Animator>(); // Use this if the Animator is on a child object
        if (animator == null)
        {
            Debug.LogError("Animator is not assigned in Start!");
        }
        else
        {
            Debug.Log("Animator successfully assigned in Start.");
        }


    }

    // Update is called once per frame
    void Update()
    {
        animator = GetComponentInChildren<Animator>();
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

        if (exploded && withinrange)
        {
            newhealth -= 10;
            exploded = false;

            if (newhealth <= 0)
            {
                Die();
            }
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
        }
        if (other.gameObject.tag == "clone")
        {
            run = false;
            animator.SetBool("attackplayer", true);
            animator.SetBool("runtoplayer", false);
            print("within range and will be exploded");
            withinrange = true;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            run = true;
            animator.SetBool("attackplayer", false);
        }
        if (other.gameObject.tag == "clone")
        {
            run = true;
            animator.SetBool("attackplayer", false);
            Debug.Log("not within range");
            withinrange = false;

        }
    }

    public void damagePlayer()
    {
        if (withinrange == false)
        {


            sorcerer.newhealth -= 5;
        }
    }

    public void GobTakeDamage(float damage)
    {
        newhealth -= damage;
        print("New Health : " + newhealth);
        if (newhealth <= 0f)
        {
            Die(); // Handle goblin death (optional)
        }
    }

   public void Die()
    {
        // Handle goblin death (e.g., play animation, destroy object, etc.)
        lilithController.OnMinionDeath();
        level.increaseXp(10);
        PlaySoundEffect(GobDies);
        Destroy(gameObject);
    }


    public void PlaySoundEffect(AudioClip clip, float volume = 1.0f)
    {
        if (clip != null)
        {
            sfxAudioSource.PlayOneShot(clip, volume); // Play the clip with specific volume
        }
    }
}

