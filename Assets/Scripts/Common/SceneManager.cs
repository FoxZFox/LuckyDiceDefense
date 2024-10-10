using System;
using System.Collections.Generic;
using UnityEngine;
using us = UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instant;
    public enum sceneName
    {
        login,
        mainmenu,
        gacha,
        gameplay
    }
    [SerializeField] private SceneStruct[] sceneStructs;
    private Dictionary<sceneName, string> sceneDic;
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
        sceneDic = new Dictionary<sceneName, string>();
        foreach (var i in sceneStructs)
        {
            sceneDic.Add(i.sceneName, i.asset);
        }

    }

    public void LoadScene(sceneName name)
    {
        us.SceneManager.LoadScene(sceneDic[name]);
    }

    public void LoadSceneAsync(sceneName name)
    {
        us.SceneManager.LoadSceneAsync(sceneDic[name], us.LoadSceneMode.Additive);
    }
}

[Serializable]
public struct SceneStruct
{
    public SceneManager.sceneName sceneName;
    public string asset;
}
