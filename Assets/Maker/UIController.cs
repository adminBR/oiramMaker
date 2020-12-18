using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Text;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public MakerScript mS;
    //string URL = "http://localhost:5000/api/mapas";

    //ui tile buttons
    public List<Button> tileButtons;
    int selectedButton = 0;

    //popup
    public GameObject PopupSave;
    public TMP_InputField nome;
    public TMP_InputField criador;
    public TextMeshProUGUI data;
    public TextMeshProUGUI branch;
    void Start()
    {
        ChangeSelectedButton(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeSelectedButton(int id)
    {
        tileButtons[selectedButton].interactable = true;
        tileButtons[id].interactable = false;
        selectedButton = id;
    }

    public void openPopup()
    {
        if (mS.startTile == null || mS.endTile == null)
            return;
        data.SetText(System.DateTime.Now.Year + "-" + System.DateTime.Now.Month + "-" + System.DateTime.Now.Day);
        if(staticLoadedMap.modo == "atualizar")
        {
            PopupSave.transform.Find("Salvar").transform.Find("Text").GetComponent<TextMeshProUGUI>().SetText("Atualizar mapa");
            PopupSave.transform.Find("Nome").GetComponent<TMP_InputField>().text = mS.mapaInfo.nome;
        }
        else {
            PopupSave.transform.Find("Salvar").transform.Find("Text").GetComponent<TextMeshProUGUI>().SetText("Salvar mapa");
        }
        PopupSave.SetActive(true);
    }
    public void closePopup()
    {
        PopupSave.SetActive(false);
    }



    public void saveMap()
    {
        TileList tempTileList = new TileList();
        foreach (Transform child in mS.parentMap.transform)
        {
            Tile temp = new Tile(new Vector2Int(Mathf.RoundToInt(child.transform.position.x), Mathf.RoundToInt(child.transform.position.y)),
                (int)child.rotation.z, int.Parse("" + child.name));
            tempTileList.tList.Add(temp);
        }

        mS.mapaInfo.nome = "" + nome.text;
        mS.mapaInfo.criador = "" + staticLoadedMap.contaNome;
        mS.mapaInfo.data = "" + data.text;
        mS.mapaInfo.json = JsonUtility.ToJson(tempTileList);
        mS.mapJson = JsonUtility.ToJson(mS.mapaInfo);
        mS.mapaInfo.id_usuarios = staticLoadedMap.contaID;

        if(staticLoadedMap.modo == "atualizar")
        {
            StartCoroutine(postRequest(mS.mapJson, mS.mapaInfo.id));
        }
        else
        {
            StartCoroutine(postRequest(mS.mapJson,-1));
        }
        
    }

    IEnumerator postRequest(string map,int id)
    {
        string url = staticLoadedMap.APIURL + "/mapas";
        if (id != -1) {
            url = staticLoadedMap.APIURL + "/mapas/" + id;
        }

        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(map);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode);
        Debug.Log("Map Saved");

        SceneManager.LoadScene("Menu");
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.LogError(request.error);
            yield break;
        }
    }

    public void loadSceneFunction(string nome)
    {
        SceneManager.LoadScene(nome);
    }

}
