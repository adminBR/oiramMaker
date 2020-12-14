[System.Serializable]
public class MapaClass
{
    public int id;
    public string nome;
    public string criador;
    public string json;
    public string data;
    public int id_usuarios;
}

[System.Serializable]
public class MapaClassOBJ
{
    public MapaClass[] mc;
}
[System.Serializable]
public class UsuarioClass
{
    public int id;
    public string nome;
    public string senha;
}

