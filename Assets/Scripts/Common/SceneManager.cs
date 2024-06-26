using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using us = UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instant;
    public enum sceneName
    {
        login,
        mainmenu
    }

    [SerializeField] SceneStruct[] sceneStructs;
    private Dictionary<sceneName, SceneAsset> sceneDic;
    void Start()
    {
        if (Instant == null)
        {
            DontDestroyOnLoad(gameObject);
            Instant = this;
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Initialize()
    {
        sceneDic = new Dictionary<sceneName, SceneAsset>();
        foreach (var i in sceneStructs)
        {
            sceneDic.Add(i.sceneName, i.asset);
        }
    }

    public void LoadScene(sceneName name)
    {
        us.SceneManager.LoadScene(sceneDic[name].name);
    }
}

[Serializable]
public struct SceneStruct
{
    public SceneManager.sceneName sceneName;
    public SceneAsset asset;
}
