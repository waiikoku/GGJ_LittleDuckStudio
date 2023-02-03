using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class CharacterLayerManager : Singleton<CharacterLayerManager>
{
    [SerializeField] private List<Transform> characters_tf = new List<Transform>();
    [SerializeField] private List<SpriteRenderer> characters_sr = new List<SpriteRenderer>();
    [SerializeField] private int characterCount = 0;

    public float topYLimit;
    public float bottomYLimit;

    [Header("Inject")]
    [SerializeField] private SpriteRenderer[] injectSR;

    //Cache
    private Vector3 position;
    private Queue<DeleteInfo> deleteQueue;

    public struct DeleteInfo
    {
        public SpriteRenderer spriteRenderer;
        public Action callback;

        public DeleteInfo(SpriteRenderer sr,Action action)
        {
            this.spriteRenderer = sr;
            this.callback = action;
        }
    }

    private void Start()
    {
        for (int i = 0; i < injectSR.Length; i++)
        {
            Add(injectSR[i]);
        }
        deleteQueue = new Queue<DeleteInfo>();
    }

    private void FixedUpdate()
    {
        if (characterCount == 0) return;
        SortingByY();
    }

    private void LateUpdate()
    {
        if (characterCount == 0) return;
        UpdateOrder();
        if (deleteQueue.Count > 0)
        {
            for (int i = 0; i < deleteQueue.Count; i++)
            {
                DeleteInfo info = deleteQueue.Dequeue();
                characters_tf.Remove(info.spriteRenderer.transform);
                characters_sr.Remove(info.spriteRenderer);
                info.callback?.Invoke();
            }
            characterCount = characters_sr.Count;
        }
    }

    public void Add(SpriteRenderer sprite)
    {
        if (characters_sr.Contains(sprite)) return;
        characters_sr.Add(sprite);
        characters_tf.Add(sprite.transform);
        characterCount = characters_sr.Count;
    }

    public void Remove(SpriteRenderer sprite,Action callback)
    {
        if (characters_sr.Contains(sprite) == false) return;
        deleteQueue.Enqueue(new DeleteInfo(sprite,callback));
    }

    private void SortingByY()
    {
        for (int i = 0; i < characterCount; i++)
        {
            position = characters_tf[i].position;
            position.y = Mathf.Clamp(position.y, bottomYLimit, topYLimit);
            characters_tf[i].position = position;
            var max = i;
            for (var j = i + 1; j < characterCount; j++)
            {
                if (characters_tf[max].position.y < characters_tf[j].position.y)
                {
                    max = j;
                }
            }

            if (max != i)
            {
                var lowerValue = characters_tf[max];
                characters_tf[max] = characters_tf[i];
                characters_tf[i] = lowerValue;
                var lowerSR = characters_sr[max];
                characters_sr[max] = characters_sr[i];
                characters_sr[i] = lowerSR;
            }
        }
        //characters_tf.Sort((a,b) => a.transform.position.y.CompareTo(b.transform.position.y));
    }

    private void UpdateOrder()
    {
        for (int i = 0; i < characterCount; i++)
        {
            characters_sr[i].sortingOrder = i;
        }
    }
}
