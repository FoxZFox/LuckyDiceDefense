using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapData : SerializedScriptableObject
{
    public string mapName;
    public List<TileDataContainer> tileDataContainers = new List<TileDataContainer>();
#if UNITY_EDITOR
    public void Initialise(string n)
    {
        mapName = n;
        name = n;
    }

    public void EditName(string n = "", int i = 0)
    {
        if (!string.IsNullOrEmpty(n) && n != mapName)
        {
            mapName = n;
            name = n;
        }
    }
#endif


}

[System.Serializable]
public class TileDataContainer
{
    public string name;
    public int sortOrder = 0;
    public bool haveGameobjet = false;
    public List<TileBase> tileData = new List<TileBase>();
    public List<Vector3Int> positionData = new List<Vector3Int>();
    public List<ObjectPositionData> objectDatas = new List<ObjectPositionData>();
}

[System.Serializable]
public class ObjectPositionData
{
    public GameObject gameObject;
    public List<Vector3> positions = new List<Vector3>();
}
