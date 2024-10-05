using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapData))]
public class MapDataEditor : Editor
{
    bool deleteSidel = false;
    MapData mapData;
    string newMapName = "";
    public override void OnInspectorGUI()
    {
        mapData = target as MapData;
        base.OnInspectorGUI();
        EditorGUILayout.BeginHorizontal("box");
        if (GUILayout.Button("Edit", EditorStyles.toolbarButton))
        {
            deleteSidel = false;
            newMapName = mapData.mapName;
        }
        if (GUILayout.Button("Delete", EditorStyles.toolbarButton))
        {
            deleteSidel = true;
        }
        EditorGUILayout.EndHorizontal();
        if (!deleteSidel)
        {
            DrawEditor();
        }
        else
        {
            DrawDelete();
        }
    }

    private void DrawEditor()
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(10);
        EditorGUILayout.Foldout(true, "Edit");
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Invoke"))
        {
            mapData.EditName(newMapName);
            newMapName = "";
            AssetDatabase.SaveAssets();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Name", GUILayout.Width(40));
        newMapName = EditorGUILayout.TextField(newMapName);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndVertical();

    }

    private void DrawDelete()
    {
        EditorGUILayout.BeginVertical("box");
        if (GUILayout.Button("Delete"))
        {
            string assetPath = AssetDatabase.GetAssetPath(mapData);
            Undo.DestroyObjectImmediate(mapData);
            AssetDatabase.DeleteAsset(assetPath);
            AssetDatabase.SaveAssets();
        }
        EditorGUILayout.EndVertical();
    }
}