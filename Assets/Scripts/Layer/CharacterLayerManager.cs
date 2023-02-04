using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLayerManager : Singleton<CharacterLayerManager>
{
    [SerializeField] private List<Transform> characters_tf = new List<Transform>();
    [SerializeField] private List<SpriteRenderer> characters_sr = new List<SpriteRenderer>();
    [SerializeField] private int characterCount = 0;

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

    public void Remove(SpriteRenderer sprite,Action callback = null)
    {
        if (characters_sr.Contains(sprite) == false) return;
        deleteQueue.Enqueue(new DeleteInfo(sprite,callback));
    }

    private void SortingByY()
    {
<<<<<<< HEAD
        if (characterCount < 2) return;
        characters_tf.Sort((t1, t2) => t2.position.y.CompareTo(t1.position.y));
        characters_sr.Sort((sr1, sr2) => characters_tf.IndexOf(sr2.transform).CompareTo(characters_tf.IndexOf(sr1.transform)));     
        /*
=======
>>>>>>> main
        if(characterCount < 2) return;
        for (int i = 0; i < characterCount; i++)
        {
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
<<<<<<< HEAD
        */

=======
        //characters_tf.Sort((a,b) => a.transform.position.y.CompareTo(b.transform.position.y));
>>>>>>> main
    }

    private void UpdateOrder()
    {
        if (characterCount < 2) return;
        for (int i = 0; i < characterCount; i++)
        {
            characters_sr[i].sortingOrder = i;
        }
    }
}
