using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarManager : Singleton<HealthbarManager>
{
    public List<RectTransform> rt = new List<RectTransform>();
    public List<Transform> tracking = new List<Transform>();
    public RectTransform container;
    public Slider prefab;
    private int failedTry = 0;
    private Queue<DeleteInfo> deleteBar;
    private struct DeleteInfo
    {
        public RectTransform rt;
        public Transform tf;

        public DeleteInfo(RectTransform rectTransform,Transform transform)
        {
            this.rt = rectTransform;
            this.tf = transform;
        }
    }
    private void Start()
    {
        deleteBar = new Queue<DeleteInfo>();
    }

    private void FixedUpdate()
    {
        try
        {
            for (int i = 0; i < tracking.Count; i++)
            {
                Vector3 screenPoint = Camera.main.WorldToViewportPoint(tracking[i].position);
                bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
                rt[i].gameObject.SetActive(onScreen);
                if (onScreen)
                {
                    rt[i].anchoredPosition = Camera.main.ViewportToScreenPoint(screenPoint);
                }
            }
        }
        catch (Exception e)
        {
            failedTry++;
            if (failedTry > tracking.Count)
            {
                rt.Clear();
                tracking.Clear();
                failedTry = 0;
            }
            throw;
        }
        if(deleteBar.Count > 0)
        {
            for (int i = 0; i < deleteBar.Count; i++)
            {
                DeleteInfo info = deleteBar.Dequeue();
                rt.Remove(info.rt);
                tracking.Remove(info.tf);
                Destroy(info.rt.gameObject);
            }
        }
    }

    public void AddHealth(Transform target,CharacterCombat combat)
    {
        Slider slider = Instantiate(prefab,container);
        combat.OnHealthUpdate += delegate (float value) { slider.value = value; };
        tracking.Add(target);
        rt.Add(slider.GetComponent<RectTransform>());
    }

    public void Remove(Transform tf)
    {
        int index = 0;
        for (int i = 0; i < tracking.Count; i++)
        {
            if(tf == tracking[i])
            {
                index = i;
                break;
            }
        }
        deleteBar.Enqueue(new DeleteInfo(rt[index], tf));
    }

    public void ClearAll()
    {
        tracking.Clear();
        rt.Clear();
        deleteBar.Clear();
    }
}
