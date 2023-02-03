using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class CharacterLayerManager : MonoBehaviour
{
    [SerializeField] private List<Transform> characters_tf = new List<Transform>();
    [SerializeField] private List<SpriteRenderer> characters_sr = new List<SpriteRenderer>();
    [SerializeField] private List<int> characterOrder = new List<int>();
    [SerializeField] private int characterCount = 0;

    public float topYLimit;
    public float bottomYLimit;

    //Cache
    private int height;
    private float lowerValue;
    private Vector3 position;
    private void FixedUpdate()
    {
        if (characterCount == 0) return;
        SortingByY();
    }

    private void LateUpdate()
    {
        if (characterCount == 0) return;
        UpdateOrder();
    }

    public void Add(SpriteRenderer sprite)
    {
        if (characters_sr.Contains(sprite)) return;
        characters_sr.Add(sprite);
        int calculate = 0;
        characters_tf.Add(sprite.transform);
        characterOrder.Add(calculate);
        characterCount = characters_sr.Count;
    }

    public void Remove(SpriteRenderer sprite)
    {
        if (characters_sr.Contains(sprite) == false) return;
        characters_tf.Add(sprite.transform);
        characters_sr.Remove(sprite);
        characterCount = characters_sr.Count;
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
