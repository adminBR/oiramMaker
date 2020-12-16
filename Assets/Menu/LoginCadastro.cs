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

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void cadastrar()
    {
        if(nome.text != "" && senha.text != "")
        {
            UsuarioClass temp = new UsuarioClass();
            temp.nome = "" + nome.text;
            temp.senha = "" + senha.text;
            string usuarioJson = JsonUtility.ToJson(temp);
            StartCoroutine(postCadastroRequest(usuarioJson));
        }
    }

    public void login()
    {
        if (nome.text != "" && senha.text != "")
        {
            UsuarioClass temp = new UsuarioClass();
            temp.nome = "" + nome.text;
            temp.senha = "" + senha.text;
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
            yield break;
        }

        Debug.Log("Login usuario: #" + request.downloadHandler.text);
        staticLoadedMap.contaID = int.Parse(request.downloadHandler.text);
        if (request.downloadHandler.text != "")
        {
            SceneManager.LoadScene("Menu");
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
            Debug.LogError(request.error);
            yield break;
        }
        Debug.Log("Cadastrado usuario: #" + request.downloadHandler.text);

        staticLoadedMap.contaID = int.Parse(request.downloadHandler.text);
        if (request.downloadHandler.text != "[]")
        {
            SceneManager.LoadScene("Menu");
        }

    }

}
