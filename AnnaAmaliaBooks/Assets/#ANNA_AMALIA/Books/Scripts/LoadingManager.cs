using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.guidepilot.guidepilotcore;
using UnityEngine.SceneManagement;
using System;

public class LoadingManager : MonoSingleton<LoadingManager>
{
    public List<SceneId> loadedScenes { get; private set; } = new List<SceneId>();

    private void Awake()
    {
        Instance = this;
    }

    public void LoadSceneSingle(SceneId scene) => LoadScene(scene, LoadSceneMode.Single);

    public void LoadSceneAdditive(SceneId scene) => LoadScene(scene, LoadSceneMode.Additive);

    public void LoadScene(SceneId scene, LoadSceneMode loadSceneMode)
    {
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(scene.SceneName(), loadSceneMode);
        loadingOperation.completed += (x) =>
        {
            if (!loadedScenes.Contains(scene))
                loadedScenes.Add(scene);
        };
    }

    public void UnloadScene(SceneId scene)
    {
        AsyncOperation loadingOperation = SceneManager.UnloadSceneAsync(scene.SceneName());
        loadingOperation.completed += (x) =>
        {
            if (loadedScenes.Contains(scene))
                loadedScenes.Remove(scene);
        };
    }
}
