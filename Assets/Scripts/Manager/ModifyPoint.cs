using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteAlways]
public class ModifyPoint : MonoBehaviour
{
    public bool activate = false;
    public Transform[] blockers;
    public Transform[] focus;

    public Vector2 startPointA;
    public Vector2 startPointB;
    public Vector2 offset;
    private void Update()
    {
        if(activate)
        {
            activate = false;
            for (int i = 0; i < blockers.Length; i++)
            {
                blockers[i].position = startPointA + (offset * i);
            }

            for (int i = 0; i < focus.Length; i++)
            {
                focus[i].position = startPointB + (offset * i);
            }
        }
    }
}
