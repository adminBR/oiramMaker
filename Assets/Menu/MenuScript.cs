using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public List<GameObject> menus;
    int lastMenuOpen = 0;

    public GameObject mapPanelPrefab;
    public GameObject mapPanelPrefabUsuario;

    public GameObject panelParentMenu2;
    public GameObject panelParentMenu3;

    public TMP_InputField seleIDField;
    public TextMeshProUGUI loggedID;

    public MapaClassOBJ mco;

    int lastMap = 0;
    //string URL = "http://localhost:5000/api/mapas";
    void Start()
    {
        //apiField.text = "http://localhost:5000/api/mapas";

        //staticLoadedMap.APIURL = apiField.text;
        loggedID.text = staticLoadedMap.contaNome + " #"+ staticLoadedMap.contaID;
    }

    // Update is called once per frame
    void Update()
    {

    }

    

    public void changeMenu(int id)
    {
        menus[lastMenuOpen].SetActive(false);
        menus[id].SetActive(true);
        lastMenuOpen = id;

        if(id == 1)
        {
            refreshMaps();
        }
        if (id == 2)
        {
            refreshMapsUsuario();
        }
    }
    public void refreshMaps()
    {
        foreach (Transform child in panelParentMenu2.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        StartCoroutine(getRequest());
    }
    public void refreshMapsUsuario()
    {
        foreach (Transform child in panelParentMenu3.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        StartCoroutine(getRequestUsuario());
    }

    IEnumerator getRequest()
    {
        Debug.Log("carregando mapas id:" + seleIDField.text);
        UnityWebRequest www = UnityWebRequest.Get(staticLoadedMap.APIURL + "/mapas/" + seleIDField.text);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError(www.error);
            yield break;
        }

        string mapas = www.downloadHandler.text;
        mapas = "{\"mc\":" + mapas + "}";

        mco = JsonUtility.FromJson<MapaClassOBJ>(mapas); //carregando todos os mapas na array

        loadPanelMapas(mco);
        Debug.Log("Map Loaded");
    }

    IEnumerator getRequestUsuario()
    {
        UnityWebRequest www = UnityWebRequest.Get(staticLoadedMap.APIURL + "/mapas/" + staticLoadedMap.contaID);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError(www.error);
            yield break;
        }

        string mapas = www.downloadHandler.text;
        Debug.Log("" + www.downloadHandler.text);
        mapas = "{\"mc\":" + mapas + "}";

        mco = JsonUtility.FromJson<MapaClassOBJ>(mapas); //carregando todos os mapas na array

        for (int i = 0; i < mco.mc.Length; i++)
        {
            GameObject temp = Instantiate(mapPanelPrefabUsuario, mapPanelPrefab.transform.position, Quaternion.identity, panelParentMenu3.transform);

            string tempID = "" + mco.mc[i].id;
            temp.name = "" + (i);
            temp.transform.Find("Map_name").GetComponent<TextMeshProUGUI>().SetText("" + mco.mc[i].nome);
            temp.transform.Find("Map_id").GetComponent<TextMeshProUGUI>().SetText("" + tempID);
            temp.transform.Find("Map_creator").GetComponent<TextMeshProUGUI>().SetText("" + mco.mc[i].criador);
            temp.transform.Find("Map_branch").GetComponent<TextMeshProUGUI>().SetText("" + mco.mc[i].id_usuarios);
            temp.transform.Find("Map_date").GetComponent<TextMeshProUGUI>().SetText("" + mco.mc[i].data.Split('T')[0]);
        }

        Debug.Log("Map Loaded");
    }

    public void loadPanelMapas(MapaClassOBJ classOBJ) 
    {
        for (int i = 0; i < classOBJ.mc.Length; i++)
        {
            
            GameObject temp = Instantiate(mapPanelPrefab, mapPanelPrefab.transform.position, Quaternion.identity, panelParentMenu2.transform);

            string tempID = ""+classOBJ.mc[i].id;
            temp.name = ""+(i+lastMap);
            temp.transform.Find("Map_name").GetComponent<TextMeshProUGUI>().SetText("" + classOBJ.mc[i].nome);
            temp.transform.Find("Map_id").GetComponent<TextMeshProUGUI>().SetText("#" + tempID);
            temp.transform.Find("Map_creator").GetComponent<TextMeshProUGUI>().SetText("" + classOBJ.mc[i].criador);
            temp.transform.Find("Map_branch").GetComponent<TextMeshProUGUI>().SetText("Criador #" + mco.mc[i].id_usuarios);
            temp.transform.Find("Map_date").GetComponent<TextMeshProUGUI>().SetText("" + mco.mc[i].data.Split('T')[0]);
        }
        
    }

    public void loadLevelEditor()
    {
        int i = int.Parse("" + gameObject.name);
        staticLoadedMap.loadedMap = mco.mc[i];
        SceneManager.LoadScene("Editor");
    }

    public void loadBlankEditor()
    {
        MapaClass tempMapaClass = new MapaClass();
        tempMapaClass.nome = "new map";
        tempMapaClass.criador = "new creator";
        tempMapaClass.json = "{}";
        tempMapaClass.id_usuarios = staticLoadedMap.contaID;
        staticLoadedMap.loadedMap = tempMapaClass;
        SceneManager.LoadScene("Editor");
    }


}
