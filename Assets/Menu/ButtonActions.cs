using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
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
        //staticLoadedMap.loadedMap.id_usuario = ""+ms.mco.mc[i].id;
        SceneManager.LoadScene("Editor");
    }
    public void DeleteMap()
    {
        ms = GameObject.Find("Canvas").GetComponent<MenuScript>();
        int i = int.Parse("" + gameObject.name);
        StartCoroutine(DeleteRequest(ms.mco.mc[i].id));

    }
    IEnumerator DeleteRequest(int id)
    {
        UnityWebRequest uwr = UnityWebRequest.Delete(staticLoadedMap.APIURL+"/mapas/"+id);
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Deleted");
            ms.refreshMapsUsuario();
        }
    }

    public void loadGame()
    {
        ms = GameObject.Find("Canvas").GetComponent<MenuScript>();
        int i = int.Parse("" + gameObject.name);
        staticLoadedMap.loadedMap = ms.mco.mc[i];

    }
}
