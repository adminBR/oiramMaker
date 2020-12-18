using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class staticLoadedMap : MonoBehaviour
{
    public static MapaClass loadedMap = new MapaClass();
    public static string APIURL = "http://localhost:5000/api";
    public static int contaID = 3;
    public static string contaNome = "admin";
    public static string modo = "criar";

}
