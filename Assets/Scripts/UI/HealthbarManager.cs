using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarManager : Singleton<HealthbarManager>
{
    public List<RectTransform> hpbar = new List<RectTransform>();
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
            hpbar[i].anchoredPosition = cam.WorldToScreenPoint(tracking[i].position);
        }
        if(deleteBar.Count > 0)
        {
            for (int i = 0; i < deleteBar.Count; i++)
            {
                DeleteInfo info = deleteBar.Dequeue();
                hpbar.Remove(info.rt);
                tracking.Remove(info.tf);
                Destroy(info.rt.gameObject);
            }
        }
    }

    public void AddHealth(Transform target,EnemyCombat ec)
    {
        Slider slider = Instantiate(prefab,container);
        ec.OnHealthUpdate += delegate (float value) { slider.value = value; };
        tracking.Add(target);
        hpbar.Add(slider.GetComponent<RectTransform>());
    }

    public void Remove(Transform tf)
    {
        int index = tracking.IndexOf(tf);
        deleteBar.Enqueue(new DeleteInfo(hpbar[index], tf));
    }
}
