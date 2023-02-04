using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    [SerializeField] private string menuName;
    public Action<float> OnLoadProgress;
    [SerializeField] private float fakeDuration = 3f;
    public void LoadMenu()
    {
        LoadScene(menuName);
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public IEnumerator LoadSceneAsync(string name,Action callback = null)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(name);
        operation.allowSceneActivation = false;
        float progress;
        bool suddenly = true;
        while (operation.isDone)
        {
            if (suddenly)
            {
                suddenly = false;
            }
            progress = Mathf.Clamp01(operation.progress / 0.9f);
            OnLoadProgress?.Invoke(progress);
            if (progress >= 1f) break;
            yield return null;
        }
        if (suddenly)
        {
            float fakeTimer = 0f;
            while (true)
            {
                fakeTimer += Time.deltaTime;
                progress = fakeTimer / fakeDuration;
                OnLoadProgress?.Invoke(progress);
<<<<<<< HEAD
=======
                if (progress >= 1f) break;
                yield return null;
>>>>>>> main
            }
        }
        operation.allowSceneActivation = true;
        callback?.Invoke();
    }
}
