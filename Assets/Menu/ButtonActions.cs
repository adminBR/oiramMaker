using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonActions : MonoBehaviour
{
    //MENU SCRIPT


    MenuScript ms;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadEditor()
    {
        ms = GameObject.Find("Canvas").GetComponent<MenuScript>();
        int i = int.Parse("" + gameObject.name);
        staticLoadedMap.loadedMap = ms.mco.mc[i];
        staticLoadedMap.loadedMap.branch = ""+ms.mco.mc[i].id;
        SceneManager.LoadScene("Editor");
    }

    public void loadGame()
    {
        ms = GameObject.Find("Canvas").GetComponent<MenuScript>();
        int i = int.Parse("" + gameObject.name);
        staticLoadedMap.loadedMap = ms.mco.mc[i];

    }
}
