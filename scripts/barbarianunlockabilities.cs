using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class barbarianunlockabilities : MonoBehaviour
{

    public levels barbarianlevels;

    //public Image bashimage;
    public Image maelstormimage;
    public Image shieldimage;
    public Image chargeimage;

    //public bool bashunlocked = true;
    public bool maelstormunlocked = false;
    public bool shieldunlocked = false;
    public bool chargeunlocked = false;

    //public TMP_Text bashtext;
    public TMP_Text maelstormtext;
    public TMP_Text shieldtext;
    public TMP_Text chargetext;

    public TMP_Text healthbottletext;
    public TMP_Text runefragmenttext;

    public barbarianabilities babilities;

    // Start is called before the first frame update
    void Start()
    {
        //bashimage.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (babilities.maelstormactive)
        {
            babilities.maelstormactive = false;
            maelstormunlocked = false;
            cooldownmaelstorm();
        }

        if (babilities.shieldactive)
        {
            babilities.shieldactive = false;
            shieldunlocked = false;
            cooldownshield();
        }
        if (babilities.chargeactive)
        {
            babilities.chargeactive = false;
            chargeunlocked = false;
            cooldowncharge();
        }
        
        /*if (fball.fireballactive)
        {
            fball.fireballactive = false;
            //fireballunlocked = false;
            cooldownfireball();
        }*/

        healthbottletext.text = babilities.healthpotions.ToString();
        runefragmenttext.text = babilities.runefragments.ToString();


    }



    private void cooldownmaelstorm()
    {
        StartCoroutine(StartCountdown(maelstormimage, 5, maelstormtext, "maelstorm"));
    }

    private void cooldownshield()
    {
        StartCoroutine(StartCountdown(shieldimage, 10, shieldtext, "shield"));
    }

    private void cooldowncharge()
    {
        StartCoroutine(StartCountdown(chargeimage, 10, chargetext, "charge"));
    }

    

    public void unlockmaelstorm()
    {
        if (barbarianlevels.currability >= 1)
        {
            maelstormimage.gameObject.SetActive(false);
            maelstormunlocked = true;
            barbarianlevels.currability -= 1;
        }
    }

    public void unlockshield()
    {
        if (barbarianlevels.currability >= 1)
        {
            shieldimage.gameObject.SetActive(false);
            shieldunlocked = true;
            barbarianlevels.currability -= 1;
        }
    }

    public void unlockcharge()
    {
        if (barbarianlevels.currability >= 1)
        {
            chargeimage.gameObject.SetActive(false);
            chargeunlocked = true;
            barbarianlevels.currability -= 1;
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
        if (ability == "maelstorm")
        {
            maelstormunlocked = true;
        }
        if (ability == "shield")
        {
            shieldunlocked = true;
        }
        if (ability == "charge")
        {
            chargeunlocked = true;
        }
        /*if (ability == "fireball")
        {
            fireballunlocked = true;
        }*/

    }

}
