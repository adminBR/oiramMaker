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
    public GameObject panelParent;
    public TMP_InputField apiField;

    public MapaClassOBJ mco;

    int lastMap = 0;
    //string URL = "http://localhost:5000/api/mapas";
    void Start()
    {
        //apiField.text = "http://localhost:5000/api/mapas";
        apiField.text = staticLoadedMap.APIURL;

        //staticLoadedMap.APIURL = apiField.text;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void saveAPI()
    {
        staticLoadedMap.APIURL = apiField.text;
    }

    public void changeMenu(int id)
    {
        menus[lastMenuOpen].SetActive(false);
        menus[id].SetActive(true);
        lastMenuOpen = id;

        if(id == 1)
        {
            StartCoroutine(getRequest());
        }
    }
    public void refreshMaps()
    {
        StartCoroutine(getRequest());
    }

    IEnumerator getRequest()
    {
        UnityWebRequest www = UnityWebRequest.Get(staticLoadedMap.APIURL);

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

    public void loadPanelMapas(MapaClassOBJ classOBJ) 
    {
        for (int i = 0; i < 20; i++)
        {
            if(i+lastMap > classOBJ.mc.Length-1)
            {
                lastMap += i;
                break;
            }
            GameObject temp = Instantiate(mapPanelPrefab, mapPanelPrefab.transform.position, Quaternion.identity, panelParent.transform);

            string tempID = ""+classOBJ.mc[i + lastMap].id;
            temp.name = ""+(i+lastMap);
            temp.transform.Find("Map_name").GetComponent<TextMeshProUGUI>().SetText("" + classOBJ.mc[i + lastMap].nome);
            temp.transform.Find("Map_id").GetComponent<TextMeshProUGUI>().SetText("" + tempID);
            temp.transform.Find("Map_creator").GetComponent<TextMeshProUGUI>().SetText("" + classOBJ.mc[i + lastMap].criador);
            temp.transform.Find("Map_branch").GetComponent<TextMeshProUGUI>().SetText("" + classOBJ.mc[i + lastMap].branch);
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
        tempMapaClass.branch = "NEW";
        staticLoadedMap.loadedMap = tempMapaClass;
        SceneManager.LoadScene("Editor");
    }


}
