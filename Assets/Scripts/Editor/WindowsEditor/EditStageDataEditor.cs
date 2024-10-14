using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

public class EditStageDataEditor : OdinMenuEditorWindow
{
    [MenuItem("Tools/EditData/Stage Data")]
    private static void OpenWindow()
    {
        GetWindow<EditStageDataEditor>().Show();
    }
    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree
        {
            {"Create New Data ",new CreateNewStageData()}
        };
        tree.AddAllAssetsAtPath("StageData", "Assets/TableObject", typeof(StageData), true, true);
        return tree;
    }

    public class CreateNewStageData
    {
        [InlineEditor(objectFieldMode: InlineEditorObjectFieldModes.Hidden)]
        public StageData data;
        public CreateNewStageData()
        {
            data = CreateInstance<StageData>();
            data.StageName = "New StageData";
        }

        [Button("Add New Map Data")]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(data, "Assets/TableObject/Stage/" + data.StageName + ".asset");
            AssetDatabase.SaveAssets();
            data = CreateInstance<StageData>();
            data.StageName = "New Map Data";
        }
    }
}
