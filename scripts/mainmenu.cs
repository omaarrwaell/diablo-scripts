using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainmenu : MonoBehaviour
{
    public GameObject maincanvas;
    public GameObject playcanvas;
    public GameObject lvlselectcanvas;
    public GameObject charactercanvas;
    public GameObject bosscharactercanvas;
    public GameObject optioncanvas;

    // Start is called before the first frame update
    void Start()
    {
        maincanvas.SetActive(true);
        charactercanvas.SetActive(false);
        bosscharactercanvas.SetActive(false);
        lvlselectcanvas.SetActive(false);
        playcanvas.SetActive(false);
        optioncanvas.SetActive(false);


    }

    public void playgame()
    {
        maincanvas.SetActive(false);
        playcanvas.SetActive(true);
    }

    public void newgame()
    {
        playcanvas.SetActive(false);
        charactercanvas.SetActive(true);
    }

    public void selectlevel()
    {
        playcanvas.SetActive(false);
        lvlselectcanvas.SetActive(true);
    }


    public void selectboss()
    {
        lvlselectcanvas.SetActive(false);
        bosscharactercanvas.SetActive(true);
    }

    public void selectmainlevel()
    {
        lvlselectcanvas.SetActive(false);
        charactercanvas.SetActive(true);
    }

    public void options()
    {
        maincanvas.SetActive(false);
        optioncanvas.SetActive(true);
    }

    public void back()
    {
        maincanvas.SetActive(true);
        playcanvas.SetActive(false);
        charactercanvas.SetActive(false);
        bosscharactercanvas.SetActive(false);
        lvlselectcanvas.SetActive(false);
        optioncanvas.SetActive(false);
    }



    public void quitgame()
    {
        Application.Quit();
    }
}
