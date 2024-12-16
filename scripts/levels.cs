using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class levels : MonoBehaviour
{
    int currlevel = 1;
    int maxlevel = 4;
    int currXp = 0;
    public Slider xpSlider;
    int sum;
    public TMP_Text xpText;
    public TMP_Text levelText;
    public TMP_Text abilityText;
    public int currability = 3;






    // Start is called before the first frame update
    void Start()
    {
         sum = 100 * currlevel;


    }

    // Update is called once per frame
    void Update()
    {
        xpSlider.value = currXp;
        xpSlider.maxValue = sum;
        //print(currXp);  
        xpText.text = "" + currXp +"/"+ sum;
        levelText.text=""+currlevel;
        abilityText.text=""+currability;
    }

    

    public void increaseXp(int xpAmount)
    {

        if (currlevel < maxlevel)
        {
            currXp = currXp + xpAmount;
            int prevsum = sum;
            print("prev:"+prevsum);

            if (currXp >= sum)
            {
                currXp = currXp-prevsum;
                currlevel++;
                sum = currlevel * 100;
                currability += 1;
            }

          
            
            print("da el text beta3et slider " + xpText); 

        }
       
        print("da el text beta3et slider " + xpText.text);

        print("curr level:"+currlevel);
        print("currXp:" + currXp);
    }
}