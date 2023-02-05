using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneManager : Singleton<ZoneManager>
{
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    [System.Serializable]
    public struct LimitInfo
    {
        public float MinY;
        public float MaxY;
        public LimitInfo(float min, float max)
        {
            MinY = min;
            MaxY = max;
        }
    }

    public LimitInfo GetInfo()
    {
        return new LimitInfo(minY, maxY);
    }

    public void LimitYPosition(Transform transform, float minY, float maxY)
    {
        Vector3 position = transform.position;
        position.y = Mathf.Clamp(position.y, minY, maxY);
        transform.position = position;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(0, minY, 0), 0.2f);
        Gizmos.DrawWireSphere(new Vector3(0, maxY, 0), 0.2f);
    }

}
