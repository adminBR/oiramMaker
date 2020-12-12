[System.Serializable]
public class MapaClass
{
    public int id;
    public string nome;
    public string criador;
    public string json;
    public string data;
    public string branch;
}

[System.Serializable]
public class MapaClassOBJ
{
    public MapaClass[] mc;
}
