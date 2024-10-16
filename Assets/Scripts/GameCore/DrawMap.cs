using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Sirenix.OdinInspector;
using System;
using System.Threading.Tasks;

public class DrawMap : MonoBehaviour
{
    [SerializeField] private GameObject gridParent;
    [SerializeField] private MapData data;
    [SerializeField] private float buildSpeed = 0;
    [SerializeField] private TileBase emptyBase;
    [SerializeField] private Tilemap emptyTile;
    public Action<Tilemap> OnDrawMap;
    private float timer;

    private void Start()
    {
        GameManager.GetInstant().BuildManager.OnBuildCharacter += DrawEmptyTile;
    }
    public void SetUp(MapData mapData)
    {
        data = mapData;
        timer = buildSpeed;
    }

    [ButtonGroup()]
    public async void CreateTileInstant()
    {
        await DrawTile();
    }
    [ButtonGroup()]
    public void ResetTile()
    {
        for (int i = 0; i < gridParent.transform.childCount; i++)
        {
            Destroy(gridParent.transform.GetChild(i).gameObject);
        }
    }

    public void DrawEmptyTile(Vector3 position, InventoryCharacter _)
    {
        var pos = emptyTile.WorldToCell(position);
        var tile = emptyTile.GetTile(pos);
        if (tile == null)
        {
            emptyTile.SetTile(pos, emptyBase);
        }
    }

    public void RemoveEmptyTile(Character data)
    {
        var pos = emptyTile.WorldToCell(data.transform.position);
        var tile = emptyTile.GetTile(pos);
        if (tile == emptyBase)
        {
            emptyTile.SetTile(pos, null);
        }
    }

    // private IEnumerator DrawTile()
    // {
    //     foreach (var item in data.tileDataContainers)
    //     {
    //         TileDataContainer data = item;
    //         var instant = new GameObject();
    //         instant.transform.position = Vector3.zero;
    //         instant.name = data.name;
    //         instant.transform.parent = gridParent.transform;
    //         var tile = instant.AddComponent<Tilemap>();
    //         var tilerender = instant.AddComponent<TilemapRenderer>();
    //         tile.tileAnchor = new Vector3(0.5f, 0.5f, 0);
    //         tilerender.sortingOrder = data.sortOrder;
    //         Debug.Log("CreateMap");
    //         for (int i = 0; i < data.tileData.Count; i++)
    //         {
    //             tile.SetTile(data.positionData[i], data.tileData[i]);
    //             if (data.name == "CantBuildArea")
    //             {
    //                 continue;
    //             }
    //             yield return null;
    //         }
    //         if (data.name == "CantBuildArea") emptyTile = tile;
    //     }
    //     OnDrawMap?.Invoke(emptyTile);
    // }
    private async Task DrawTile()
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
                if (data.name == "CantBuildArea")
                {
                    continue;
                }
                await Task.Delay((int)(buildSpeed * 1000));
            }
            if (data.name == "CantBuildArea") emptyTile = tile;
        }
        OnDrawMap?.Invoke(emptyTile);
    }
}

