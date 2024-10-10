using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public class GameRunner : MonoBehaviour
{
    static GameRunner()
    {
        string path = "Assets/Scenes/LoginScene.unity";
        EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
    }
}
