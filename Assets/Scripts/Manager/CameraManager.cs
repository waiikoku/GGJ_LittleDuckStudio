using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    public GameObject cinemachine_Main;

    public void Active(bool active)
    {
        cinemachine_Main.SetActive(active);
    }
}
