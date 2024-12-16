using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barbariancampenter : MonoBehaviour
{
    // Start is called before the first frame update



    public barbarianfollowtarget goblin1follow;
    public barbarianfollowtarget goblin2follow;
    public barbarianfollowtarget goblin3follow;
    public barbarianfollowtarget goblin4follow;
    public barbarianfollowtarget goblin5follow;

    public barbarianabilities barbarian;

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

        if (barbarian.maelstormstate == true)
        {
            goblin1follow.maelstormgoblin = true;
            goblin2follow.maelstormgoblin = true;
            goblin3follow.maelstormgoblin = true;
            goblin4follow.maelstormgoblin = true;
            goblin5follow.maelstormgoblin = true;

            barbarian.maelstormstate = false;
        }
        else if (barbarian.maelstormstate == false)
        {
            goblin1follow.maelstormgoblin = false;
            goblin2follow.maelstormgoblin = false;
            goblin3follow.maelstormgoblin = false;
            goblin4follow.maelstormgoblin = false;
            goblin5follow.maelstormgoblin = false;
        }




    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            following = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            following = false;

        }
    }



}
