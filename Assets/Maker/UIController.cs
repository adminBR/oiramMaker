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
        data.SetText("2222-10-22");
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
        mS.mapaInfo.criador = "" + criador.text;
        mS.mapaInfo.data = "" + data.text;
        mS.mapaInfo.json = JsonUtility.ToJson(tempTileList);
        mS.mapJson = JsonUtility.ToJson(mS.mapaInfo);
        mS.mapaInfo.id_usuarios = staticLoadedMap.contaID;

        StartCoroutine(postRequest(mS.mapJson));
        
    }

    IEnumerator postRequest(string map)
    {
        var request = new UnityWebRequest(staticLoadedMap.APIURL + "/mapas", "POST");
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
