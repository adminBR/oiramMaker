using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class staticLoadedMap : MonoBehaviour
{
    public static MapaClass loadedMap = new MapaClass();
    public static string APIURL = "http://localhost:5000/api/mapas";

}
