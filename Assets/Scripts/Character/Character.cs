using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer m_sprite;
    public SpriteRenderer Sprite => m_sprite;

    protected virtual void Start()
    {
        CharacterLayerManager.Instance.Add(m_sprite);
    }

    protected Sprite CaptureFrame()
    {
        return m_sprite.sprite;
    }
}
