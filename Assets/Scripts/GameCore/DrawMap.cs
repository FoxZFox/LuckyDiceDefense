using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Sirenix.OdinInspector;

public class DrawMap : MonoBehaviour
{
    [SerializeField] private GameObject gridParent;
    [SerializeField] private MapData data;
    [SerializeField] private float buildSpeed = 0.2f;


    [ButtonGroup()]
    void CreateTileInstant()
    {
        StartCoroutine(DrawTile());
    }
    [ButtonGroup()]
    void ResetTile()
    {
        for (int i = 0; i < gridParent.transform.childCount; i++)
        {
            Destroy(gridParent.transform.GetChild(i).gameObject);
        }
    }

    private IEnumerator DrawTile()
    {
        foreach (var item in data.tileDataContainers)
        {
            TileDataContainer data = item;
            var instant = new GameObject();
            instant.transform.position = Vector3.zero;
            instant.name = data.name;
            instant.transform.parent = gridParent.transform;
            var tile = instant.AddComponent<Tilemap>();
            var tilerender = instant.AddComponent<TilemapRenderer>();
            tile.tileAnchor = new Vector3(0.5f, 0.5f, 0);
            tilerender.sortingOrder = data.sortOrder;
            Debug.Log("CreateMap");
            for (int i = 0; i < data.tileData.Count; i++)
            {
                tile.SetTile(data.positionData[i], data.tileData[i]);
                yield return new WaitForSeconds(buildSpeed);
            }
            if (data.haveGameobjet)
            {
                foreach (var i in data.objectDatas)
                {
                    foreach (var j in i.positions)
                    {
                        yield return new WaitForSeconds(buildSpeed);
                        var objectSpawn = Instantiate(i.gameObject, j, Quaternion.identity);
                        objectSpawn.transform.parent = instant.transform;
                    }
                }
            }
        }

    }
}
