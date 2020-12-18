using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Text;

public class LoginCadastro : MonoBehaviour
{
    public TMP_InputField nome;
    public TMP_InputField senha;

    public TextMeshProUGUI errorTxt;
    public TMP_InputField apiField;

    UsuarioClass temp = new UsuarioClass();

    void Start()
    {
        apiField.text = staticLoadedMap.APIURL;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void saveAPI()
    {
        staticLoadedMap.APIURL = apiField.text;
    }

    public void cadastrar()
    {

        errorTxt.text = "";
        if (nome.text.ToLower() != "" && senha.text.ToLower() != "")
        {
            temp.nome = "" + nome.text.ToLower();
            temp.senha = "" + senha.text.ToLower();
            string usuarioJson = JsonUtility.ToJson(temp);
            StartCoroutine(postCadastroRequest(usuarioJson));
        }
    }

    public void login()
    {

        errorTxt.text = "";
        if (nome.text.ToLower() != "" && senha.text.ToLower() != "")
        {
            temp.nome = "" + nome.text.ToLower();
            temp.senha = "" + senha.text.ToLower();
            string usuarioJson = JsonUtility.ToJson(temp);
            StartCoroutine(postLoginRequest(usuarioJson));
        }
    }

    IEnumerator postLoginRequest(string usuario)
    {
        var request = new UnityWebRequest(staticLoadedMap.APIURL + "/usuarios/login", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(usuario);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.LogError(request.error);
            errorTxt.text = "API incorreta ou servidor inacessivel";
            yield break;
        }

        string textReq = request.downloadHandler.text;
        if (textReq.Length < 10)
        {
            Debug.Log("Login usuario: #" + request.downloadHandler.text);
            staticLoadedMap.contaID = int.Parse(request.downloadHandler.text);
            staticLoadedMap.contaNome = temp.nome;
            SceneManager.LoadScene("Menu");
        }
        else
        {
            errorTxt.text = textReq;
        }
    }
    IEnumerator postCadastroRequest(string usuario)
    {
        var request = new UnityWebRequest(staticLoadedMap.APIURL + "/usuarios/cadastro", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(usuario);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            errorTxt.text = "API incorreta ou servidor inacessivel";
            Debug.LogError(request.error);
            yield break;
        }

        string textReq = request.downloadHandler.text;
        if (textReq.Length < 10)
        {
            Debug.Log("Cadastrado usuario: #" + request.downloadHandler.text);
            staticLoadedMap.contaID = int.Parse(request.downloadHandler.text);
            staticLoadedMap.contaNome = temp.nome;
            SceneManager.LoadScene("Menu");
        }
        else
        {

        }

    }

}
