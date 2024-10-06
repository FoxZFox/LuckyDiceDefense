using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

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
