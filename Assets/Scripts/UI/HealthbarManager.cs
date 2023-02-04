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
    private Camera cam;
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
        cam = Camera.main;
        deleteBar = new Queue<DeleteInfo>();
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < tracking.Count; i++)
        {
            Vector3 screenPoint = cam.WorldToViewportPoint(tracking[i].position);
            bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
            rt[i].gameObject.SetActive(onScreen);
            if (onScreen)
            {
                rt[i].anchoredPosition = cam.ViewportToScreenPoint(screenPoint);
            }
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

<<<<<<< HEAD
=======
    /*
>>>>>>> main
    public void AddHealth(Transform target,CharacterCombat combat)
    {
        Slider slider = Instantiate(prefab,container);
        combat.OnHealthUpdate += delegate (float value) { slider.value = value; };
        tracking.Add(target);
        rt.Add(slider.GetComponent<RectTransform>());
    }
<<<<<<< HEAD

    public void AddHealth(Transform target, Action<float> onHealthUpdate)
    {
        Slider slider = Instantiate(prefab, container);
        onHealthUpdate += delegate (float value) { slider.value = value; };
        tracking.Add(target);
        rt.Add(slider.GetComponent<RectTransform>());
    }
=======
    */
>>>>>>> main

    public void Add(Transform target,RectTransform rect)
    {
        tracking.Add(target);
        rt.Add(rect);
    }
    public void Remove(Transform tf)
    {
        int index = tracking.IndexOf(tf);
        deleteBar.Enqueue(new DeleteInfo(rt[index], tf));
    }
}
