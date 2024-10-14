using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine.Tilemaps;
using System.Linq;

public class EditMapDataEditor : OdinMenuEditorWindow
{
    [MenuItem("Tools/EditData/Map Data")]
    private static void OpenWindow()
    {
        GetWindow<EditMapDataEditor>().Show();
    }
    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree
        {
            { "Create New Data", new CreateNewMapData() },
            { "Edit Map Data", new EditMapData() }
        };
        tree.AddAllAssetsAtPath("Map Data", "Assets/TableObject", typeof(MapData), true, true);
        return tree;
    }

    public class EditMapData
    {
        [InlineEditor(objectFieldMode: InlineEditorObjectFieldModes.Boxed), AssetsOnly]
        public MapData mapData;
        [ShowIf("@mapData != null")]
        public Grid grid;
        [ShowIf("@mapData != null")]
        public GameWaypoints gameWaypoints;
        [ShowIf("@mapData != null"), ButtonGroup()]
        private void LoadDataToMap()
        {
            var gridObject = grid.gameObject;
            var i = gridObject.GetComponentsInChildren<Tilemap>();
            Debug.Log(i.Count());
            foreach (var item in i)
            {
                string name = item.gameObject.name;
                int renderrer = item.gameObject.GetComponent<TilemapRenderer>().sortingOrder;
                Debug.Log(renderrer);
                TileDataContainer data = new TileDataContainer();
                data.name = name;
                data.sortOrder = renderrer;
                GetTileData(item, data);
                mapData.tileDataContainers.Add(data);
            }
            GetPathData(gameWaypoints.Waypoints);
            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(mapData);
        }
        [ShowIf("@mapData != null"), ButtonGroup()]
        private void ClearData()
        {
            mapData.tileDataContainers.Clear();
            mapData.Path.Clear();
            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(mapData);
        }

        [ShowIf("@mapData != null"), Button()]
        private void LogData()
        {
            foreach (var item in mapData.tileDataContainers)
            {
                Debug.Log(item.objectDatas.Count);
            }
        }

        private void GetPathData(Vector3[] paths)
        {
            foreach (var item in paths)
            {
                mapData.Path.Add(item);
            }
        }

        private void GetTileData(Tilemap tilemap, TileDataContainer data)
        {
            BoundsInt boundsInt = tilemap.cellBounds;
            for (int x = 0; x < boundsInt.size.x; x++)
            {
                for (int y = boundsInt.size.y; y >= 0; y--)
                {
                    Vector3Int position = new Vector3Int(x + boundsInt.x, y + boundsInt.y, 0);
                    TileBase tileBase = tilemap.GetTile(position);
                    Debug.Log(tileBase);
                    if (tileBase != null)
                    {
                        data.tileData.Add(tileBase);
                        data.positionData.Add(position);
                    }
                }
            }
            if (tilemap.gameObject.transform.childCount > 0)
            {
                for (int i = 0; i < tilemap.gameObject.transform.childCount; i++)
                {
                    var item = tilemap.gameObject.transform.GetChild(i).gameObject;
                    var prefabRoot = PrefabUtility.GetOutermostPrefabInstanceRoot(item);
                    var prefabAsset = PrefabUtility.GetCorrespondingObjectFromSource(prefabRoot);
                    Debug.Log(prefabRoot);
                    Debug.Log(prefabAsset);

                    if (prefabAsset != null)
                    {
                        var index = data.objectDatas.FindIndex(j => j.gameObject == prefabAsset);
                        if (index != -1)
                        {
                            data.objectDatas[index].positions.Add(item.transform.position);
                        }
                        else
                        {
                            ObjectPositionData objectPositionData = new ObjectPositionData();
                            objectPositionData.gameObject = prefabAsset;
                            objectPositionData.positions.Add(item.transform.position);
                            data.objectDatas.Add(objectPositionData);
                        }
                    }

                }
                data.haveGameobjet = true;
            }
        }

    }

    public class CreateNewMapData
    {
        [InlineEditor(objectFieldMode: InlineEditorObjectFieldModes.Hidden)]
        public MapData mapData;
        public CreateNewMapData()
        {
            mapData = CreateInstance<MapData>();
            mapData.mapName = "New Map Data";
        }

        [Button("Add New Map Data")]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(mapData, "Assets/TableObject/MapData/" + mapData.mapName + ".asset");
            AssetDatabase.SaveAssets();
            mapData = CreateInstance<MapData>();
            mapData.mapName = "New Map Data";
        }
    }
}
