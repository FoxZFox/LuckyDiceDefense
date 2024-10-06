using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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
