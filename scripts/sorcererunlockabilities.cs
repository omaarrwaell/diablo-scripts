using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class sorcererunlockabilities : MonoBehaviour
{
    public levels sorcererlevels;
    public Image teleportimage;
    public Image cloneimage;
    public Image infernoimage;
    public Image fireballimage;

    public bool teleportunlocked = false;
    public bool cloneunlocked = false;
    public bool infernounlocked = false;
    public bool fireballunlocked = true;

    public TMP_Text fireballtext;
    public TMP_Text teleporttext;
    public TMP_Text clonetext;
    public TMP_Text infernotext;

    public TMP_Text healthbottletext;
    public TMP_Text runefragmenttext;

    public sorcererabilities sabilities;
    public InfernoSpawner ispawner;
    //public fireball fball;



    // Start is called before the first frame update
    void Start()
    {
        fireballimage.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (sabilities.teleportactive)
        {
            sabilities.teleportactive = false;
            teleportunlocked = false;
            cooldownteleport();
        }
        if (sabilities.cloneactive)
        {
            sabilities.cloneactive = false;
            cloneunlocked = false;
            cooldownclone();
        }
        if (ispawner.infernoactive)
        {
            ispawner.infernoactive = false;
            infernounlocked = false;
            cooldowninferno();
        }
        /*if (fball.fireballactive)
        {
            fball.fireballactive = false;
            //fireballunlocked = false;
            cooldownfireball();
        }*/

        healthbottletext.text = sabilities.healthpotions.ToString();
        runefragmenttext.text = sabilities.runefragments.ToString();



    }

    /*private void cooldownfireball()
    {
        StartCoroutine(StartCountdown(fireballimage, 1, fireballtext, "fireball"));
    }*/

    private void cooldowninferno()
    {
        StartCoroutine(StartCountdown(infernoimage, 15, infernotext, "inferno"));
    }

    private void cooldownclone()
    {
        StartCoroutine(StartCountdown(cloneimage, 10, clonetext, "clone"));
    }


    private void cooldownteleport()
    {
        StartCoroutine(StartCountdown(teleportimage, 10, teleporttext, "teleport"));
    }

    public void unlockteleport()
    {
        if (sorcererlevels.currability >= 1)
        {
            teleportimage.gameObject.SetActive(false);
            teleportunlocked = true;
            sorcererlevels.currability -= 1;
        }
    }

    public void unlockclone()
    {
        if (sorcererlevels.currability >= 1)
        {
            cloneimage.gameObject.SetActive(false);
            cloneunlocked = true;
            sorcererlevels.currability -= 1;
        }
    }

    public void unlockinferno()
    {
        if (sorcererlevels.currability >= 1)
        {
            infernoimage.gameObject.SetActive(false);
            infernounlocked = true;
            sorcererlevels.currability -= 1;
        }
    }


    float currCountdownValue;
    
    public IEnumerator StartCountdown(Image img, float countdownValue, TMP_Text text, string ability)
    {
        currCountdownValue = countdownValue;
        img.gameObject.SetActive(true);
        while (currCountdownValue > 0)
        {
            text.text = currCountdownValue.ToString();
            yield return new WaitForSeconds(1.0f);
            currCountdownValue--;
        }
        img.gameObject.SetActive(false);
        if (ability == "teleport")
        {
            teleportunlocked = true;
        }
        if (ability == "clone")
        {
            cloneunlocked = true;
        }
        if (ability == "inferno")
        {
            infernounlocked = true;
        }
        /*if (ability == "fireball")
        {
            fireballunlocked = true;
        }*/

    }
}
