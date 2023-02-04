using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawnManager : MonoBehaviour
{
    public Character prefab;

    public Transform container;
    public CharacterLayerManager clm;

    public float minY;
    public float maxY;

    private void Start()
    {
        Spawn(6);
    }

    public void Spawn(int amout)
    {
        for (int i = 0; i < amout; i++)
        {
            Character go = Instantiate(prefab, container);
            go.gameObject.name = $"Clone({i})";
            clm.Add(go.Sprite);
            RandomColor(go.Sprite);
            RandomY(go.transform);
        }
    }

    private void RandomColor(SpriteRenderer sr)
    {
        sr.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }

    private void RandomY(Transform tf)
    {
        tf.position = new Vector3(0, Random.Range(minY, maxY), 0);
    }
}
