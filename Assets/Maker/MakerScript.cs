using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.Serialization;

[System.Serializable]
public class Tile
{
    public Vector2Int position;
    public int rotation;
    public int TileId;
    public Tile(Vector2Int pos, int rot, int tId)
    {
        position = pos;
        rotation = rot;
        TileId = tId;
    }
}

[System.Serializable]
public class TileList
{
    public List<Tile> tList = new List<Tile>();
}


public class MakerScript : MonoBehaviour
{
    //geral
    public GameObject parentMap;
    public GameObject phantomTile;
    public GameObject prefabTile;
    public List<Sprite> tileList;
    public int selectedTileId = 0;
    public Vector2Int aproxPos;
    public bool isMakerMode = true;
    public GameObject startTile = null;
    public GameObject endTile = null;
    

    //detection
    public bool isMouseOver = false;
    public GameObject Player;
    public GameObject EditorUI;
    public GameObject playUI;


    //serialization
    public TileList levelList = new TileList();
    public string mapJson = "";
    public MapaClass mapaInfo = new MapaClass();
    //string URL = "http://localhost:5000/api/mapas";

    void Start()
    {
        //mapaInfo = staticLoadedMap.loadedMap;
        loadFirstTime();

        phantomTile.GetComponent<SpriteRenderer>().sprite = tileList[selectedTileId];
    }

    // Update is called once per frame
    void Update()
    {
        if (isMakerMode)
        {
            MakerMode();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                loadMakerMode();
            }
        }
    }

    public void Reset()
    {
        Player.transform.position = new Vector2(startTile.transform.position.x, startTile.transform.position.y);
        Player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    }

    public void loadPlayerMode()
    {
        if (startTile == null || endTile == null)
            return;

        isMakerMode = false;
        EditorUI.SetActive(false);
        playUI.SetActive(true);
        phantomTile.SetActive(false);

        Player.SetActive(true);
        Player.transform.position = new Vector2(startTile.transform.position.x, startTile.transform.position.y);
    }
    public void loadMakerMode()
    {
        phantomTile.GetComponent<SpriteRenderer>().sprite = tileList[selectedTileId];

        isMakerMode = true;
        EditorUI.SetActive(true);
        playUI.SetActive(false);
        phantomTile.SetActive(true);

        Player.SetActive(false);
    }

    public void MakerMode()
    {
        Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        aproxPos = new Vector2Int(Mathf.RoundToInt(mousepos.x), Mathf.RoundToInt(mousepos.y));

        phantomTile.transform.position = new Vector3(aproxPos.x, aproxPos.y, phantomTile.transform.position.z);



        if (Input.GetMouseButton(0))
        {
            var eventSys = GameObject.Find("EventSystem").GetComponent<EventSystem>();
            RaycastHit2D rcCheck = Physics2D.CircleCast(aproxPos, 0.4f, Vector2.zero);
            if (rcCheck.collider == null && !eventSys.IsPointerOverGameObject())
            {
                Debug.Log("bloco instanciado");
                addTile(aproxPos, selectedTileId);
            }
        }
        if (Input.GetMouseButton(1))
        {
            RaycastHit2D rcCheck = Physics2D.CircleCast(aproxPos, 0.4f, Vector2.zero);
            if (rcCheck.collider != null)
            {
                Debug.Log("bloco removido");
                GameObject.Destroy(rcCheck.collider.gameObject);
            }
        }
    }

    public void addTile(Vector2Int position, int tileId)
    {
        GameObject tempTile = prefabTile;
        tempTile.transform.position = new Vector3(aproxPos.x, aproxPos.y, tempTile.transform.position.z);

        if (tileId == 0) //start
        {
            tempTile.GetComponent<SpriteRenderer>().sprite = tileList[0];
            if(startTile != null)
            {
                Destroy(startTile.gameObject);
                Debug.Log("destroy");
            }
            startTile = Instantiate(tempTile, new Vector3(position.x, position.y, 1), Quaternion.identity, parentMap.transform);
            startTile.GetComponent<BoxCollider2D>().isTrigger = true;
            startTile.name = "" + tileId;
            return;
        }
        else if (tileId == 1) //end
        {
            tempTile.GetComponent<SpriteRenderer>().sprite = tileList[1];
            if (endTile != null)
            {
                Destroy(endTile.gameObject);
            }
            endTile = Instantiate(tempTile, new Vector3(position.x, position.y, 1), Quaternion.identity, parentMap.transform);
            endTile.GetComponent<BoxCollider2D>().isTrigger = true;
            endTile.name = "" + tileId;
            endTile.tag = "endTile";
            return;
        }
        else if (tileId == 2) //espinho
        {
            tempTile.GetComponent<SpriteRenderer>().sprite = tileList[2];
            GameObject espinho = Instantiate(tempTile, new Vector3(position.x, position.y, 1), Quaternion.identity, parentMap.transform);
            espinho.GetComponent<BoxCollider2D>().isTrigger = true;
            espinho.GetComponent<BoxCollider2D>().size = new Vector2(0.5f, 0.5f);
            espinho.name = "" + tileId;
            espinho.tag = "espinho";
            return;
        }
        else if (tileId == 3) //padrao
        {
            tempTile.GetComponent<SpriteRenderer>().sprite = tileList[Random.Range(3, 6)];
        }
        else
        {
            tempTile.GetComponent<SpriteRenderer>().sprite = tileList[tileId];
        }

        GameObject instTile = Instantiate(tempTile, new Vector3(position.x, position.y, 1), Quaternion.identity,parentMap.transform);
        instTile.name = ""+tileId;

    }

    public void loadFirstTime()
    {
        mapaInfo = staticLoadedMap.loadedMap;
        foreach (Transform child in parentMap.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        levelList = JsonUtility.FromJson<TileList>(mapaInfo.json);

        foreach (Tile tTemp in levelList.tList)
        {
            addTile(tTemp.position, tTemp.TileId);
        }
        Debug.Log("Map Loaded");
    }

    public void loadMap()
    {
        //levelList = new List<Tile>();
        foreach (Transform child in parentMap.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        StartCoroutine(getRequest(17));

    }


    IEnumerator getRequest(int id)
    {
        UnityWebRequest www = UnityWebRequest.Get(staticLoadedMap.APIURL + "/mapas/" + id);

        yield return www.SendWebRequest();

        if(www.isNetworkError || www.isHttpError)
        {
            Debug.LogError(www.error);
            yield break;
        }

        mapJson = www.downloadHandler.text;
        mapJson = "{\"mc\":" + mapJson + "}";


        MapaClassOBJ mco = JsonUtility.FromJson<MapaClassOBJ>(mapJson);

        mapaInfo = mco.mc[0];
        levelList = JsonUtility.FromJson<TileList>(mapaInfo.json);

        foreach (Tile tTemp in levelList.tList)
        {
            addTile(tTemp.position, tTemp.TileId);
        }
        Debug.Log("Map Loaded");
    }

    /////UI Functions
    ///

    public void setActiveTile(int id)
    {
        selectedTileId = id;
        phantomTile.GetComponent<SpriteRenderer>().sprite = tileList[id];
    }

}
