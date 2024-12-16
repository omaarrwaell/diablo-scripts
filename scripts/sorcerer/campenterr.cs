using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class campenterr : MonoBehaviour
{
    // Start is called before the first frame update


    public List<DemonController> demons; // List of demons in the camp

    private bool playerDetected = false;
    public followtarget goblin1follow;
    public followtarget goblin2follow;
    public followtarget goblin3follow;
    public followtarget goblin4follow;
    public followtarget goblin5follow;

    public sorcererabilities sorcerer;
    public GameObject cloneskeleton;
    public GameObject skeleton;

    private bool following = false;
    void Start()
    {

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

        if (sorcerer.detonated == true)
        {
            goblin1follow.target = cloneskeleton.transform;
            goblin2follow.target = cloneskeleton.transform;
            goblin3follow.target = cloneskeleton.transform;
            goblin4follow.target = cloneskeleton.transform;
            goblin5follow.target = cloneskeleton.transform;
            
        }
        if (sorcerer.exploded == true)
        {
            Debug.Log("sorcerer exploded");
            goblin1follow.exploded = true;
            goblin2follow.exploded = true;
            goblin3follow.exploded = true;
            goblin4follow.exploded = true;
            goblin5follow.exploded = true;

            goblin1follow.target = skeleton.transform;
            goblin2follow.target = skeleton.transform;
            goblin3follow.target = skeleton.transform;
            goblin4follow.target = skeleton.transform;
            goblin5follow.target = skeleton.transform;
            sorcerer.exploded = false;

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

    public void RemoveDemon(DemonController demon)
    {
        if (demons.Contains(demon))
        {
            demons.Remove(demon);
            Debug.Log($"Demon removed. Remaining demons: {demons.Count}");

            Destroy(demon.gameObject);
        }
    }




}
