using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using us = UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instant;
    public Action<sceneName> OnSceneLoaded;
    [SerializeField] private GameObject sceneTransitionContainer;
    private SceneTransition[] transitions;
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
        transitions = sceneTransitionContainer.GetComponentsInChildren<SceneTransition>();
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
    [Button()]
    public void LoadSceneWithTransition(sceneName name, TransitionType type)
    {
        StartCoroutine(LoadAsync(name, type));
    }
    public void LoadSceneAsync(sceneName name)
    {
        us.SceneManager.LoadSceneAsync(sceneDic[name], us.LoadSceneMode.Additive);
    }

    private IEnumerator LoadAsync(sceneName name, TransitionType type)
    {
        var transition = transitions.First(t => t.Type == type);

        var scene = us.SceneManager.LoadSceneAsync(sceneDic[name]);
        scene.allowSceneActivation = false;
        SoundManager.Instant.PauseBackGroundMusic();
        yield return transition.AnimationTransitionIn();

        do
        {
            yield return null;
        } while (scene.progress < 0.9f);

        scene.allowSceneActivation = true;
        yield return transition.AnimationTransitionOut();
        OnSceneLoaded?.Invoke(name);
        SoundManager.Instant.ResumeBackGroundMusic();
    }
}

[Serializable]
public struct SceneStruct
{
    public SceneManager.sceneName sceneName;
    public string asset;
}

public enum TransitionType
{
    Circle,
    CrossFade
}