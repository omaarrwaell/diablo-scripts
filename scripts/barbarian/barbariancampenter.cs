using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barbariancampenter : MonoBehaviour
{
    // Start is called before the first frame update


    public List<barbarianDemonController> demons; // List of demons in the camp

    private bool playerDetected = false;
    public barbarianfollowtarget goblin1follow;
    public barbarianfollowtarget goblin2follow;
    public barbarianfollowtarget goblin3follow;
    public barbarianfollowtarget goblin4follow;
    public barbarianfollowtarget goblin5follow;


    public GameObject runefragment;

    private bool following = false;
    void Start()
    {
        runefragment.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (following)
        {
            goblin1follow.isFollowing = true;
            goblin2follow.isFollowing = true;
            goblin3follow.isFollowing = true;
            goblin4follow.isFollowing = true;
            goblin5follow.isFollowing = true;
        }
        else if (!following)
        {
            goblin1follow.isFollowing = false;
            goblin2follow.isFollowing = false;
            goblin3follow.isFollowing = false;
            goblin4follow.isFollowing = false;
            goblin5follow.isFollowing = false;
        }

       
        
        for (int i = demons.Count - 1; i >= 0; i--) // Iterate backward to handle removal safely
        {
            var demon = demons[i];

            if (playerDetected && i == 0)
            {
                demon.isPatrolling = false;
                demon.isRunning = true;
                //  Debug.Log($"Demon at index {i} is now running.");
            }
            else
            {
                demon.isPatrolling = true;
                demon.isRunning = false;
                demon.isAttacking = false;
                //  Debug.Log($"Demon at index {i} is now patrolling.");
            }
        }


        if (Input.GetKeyDown("k"))
        {
            if (demons.Count > 0)
            {
                // RemoveDemon(demons[0]); // Remove the first demon in the list
                demons.Remove(demons[0]);
            }
            else
            {
                Debug.Log("No demons to remove.");
            }
        }

        spawnrune();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "clone")
        {
            following = true;

        }
        if (other.CompareTag("Player"))
        {
            playerDetected = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "clone")
        {
            following = false;

        }
        if (other.CompareTag("Player"))
        {
            playerDetected = false;
        }
    }

    public void RemoveDemon(barbarianDemonController demon)
    {
        if (demons.Contains(demon))
        {
            demons.Remove(demon);
            Debug.Log($"Demon removed. Remaining demons: {demons.Count}");

            Destroy(demon.gameObject);
        }
    }
    private bool demonsdead = true;

    private void spawnrune()
    {

        if (goblin1follow.dead && goblin2follow.dead && goblin3follow.dead && goblin4follow.dead && goblin5follow.dead)
        {
            Debug.Log("awel if");

            demonsdead = true;

            for (int i = demons.Count - 1; i >= 0; i--) // Iterate backward to handle removal safely
            {


                Debug.Log("for");
                var demon = demons[i];

                if (demon.isDead == false)
                {
                    demonsdead = false;
                    Debug.Log("fih demon mamatsh");
                }

            }

            if (demonsdead && runefragment != null)
            {
                runefragment.SetActive(true);
            }


        }


    }




}




