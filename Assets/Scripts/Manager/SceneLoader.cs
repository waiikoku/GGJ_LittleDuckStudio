using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    [SerializeField] private string menuName;
    public void LoadMenu()
    {
        LoadScene(menuName);
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    private IEnumerator LoadSceneAsync(string name,Action callback = null)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(name);
        operation.allowSceneActivation = false;
        float progress;
        while (true)
        {
            progress = Mathf.Clamp01(operation.progress / 0.9f);
            if (progress >= 1f) break;
            yield return null;
        }
        operation.allowSceneActivation = true;
        callback?.Invoke();
    }
}
