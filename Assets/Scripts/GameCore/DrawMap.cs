using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Sirenix.OdinInspector;
using System;

public class DrawMap : MonoBehaviour
{
    [SerializeField] private GameObject gridParent;
    [SerializeField] private MapData data;
    [SerializeField] private float buildSpeed = 0.2f;
    [SerializeField] private TileBase emptyBase;
    [SerializeField] private Tilemap emptyTile;
    public Action OnDrawMap;

    private void Start()
    {
        GameManager.GetInstant().BuildManager.OnBuildCharacter += DrawEmptyTile;
    }
    public void SetUp(MapData mapData)
    {
        data = mapData;
    }

    [ButtonGroup()]
    public void CreateTileInstant()
    {
        StartCoroutine(DrawTile());
    }
    [ButtonGroup()]
    public void ResetTile()
    {
        for (int i = 0; i < gridParent.transform.childCount; i++)
        {
            Destroy(gridParent.transform.GetChild(i).gameObject);
        }
    }

    public void DrawEmptyTile(Vector3 position, CharacterData _)
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
                if (data.name == "CantBuildArea")
                {
                    continue;
                }
                yield return new WaitForSeconds(buildSpeed);
            }
            if (data.name == "CantBuildArea") emptyTile = tile;
        }
        OnDrawMap?.Invoke();
    }
}
