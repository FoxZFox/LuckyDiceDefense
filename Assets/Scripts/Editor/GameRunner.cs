using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;

// [InitializeOnLoad]
public class GameRunner : Editor
{
    static GameRunner()
    {
        string path = "Assets/Scenes/LoginScene.unity";
        EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
    }
}
