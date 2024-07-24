using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(InventoryController))]
public class InventoryManagerEditor : Editor
{
    static string itemDataPath = "Assets/TableObject/Item";
    static List<ItemData> itemDatas = new List<ItemData>();
    static List<string> names = new List<string>();
    static List<int> datas = new List<int>();
    static int itemID = 0;

    [MenuItem("DeBugger/LoadItemdata")]
    [RuntimeInitializeOnLoadMethod]
    private static void LoadItemData()
    {
        string[] assetguids = AssetDatabase.FindAssets("t:ItemData", new[] { itemDataPath });
        itemDatas.Clear();
        names.Clear();
        datas.Clear();
        for (int i = 0; i < assetguids.Length; i++)
        {
            string assetpath = AssetDatabase.GUIDToAssetPath(assetguids[i]);
            itemDatas.Add(AssetDatabase.LoadAssetAtPath<ItemData>(assetpath));
            datas.Add(itemDatas[i].ItemID);
            names.Add(itemDatas[i].ItemName);
        }
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        InventoryController ic = (InventoryController)target;
        itemID = EditorGUILayout.IntPopup("ItemID", itemID, names.ToArray(), datas.ToArray());

        if (GUILayout.Button("AddItem"))
        {
            // Debug.Log($"{itemID}");
            ic.AddItem(itemID);
        }
    }
}
