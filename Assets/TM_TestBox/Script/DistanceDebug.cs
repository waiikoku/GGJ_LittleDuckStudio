using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceDebug : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform enemy;

    [SerializeField] private float minimumRadius = 10f;
    [SerializeField] private float chargeDistance = 30f;
    public Vector3 direction;
    public float distance;
    public float dstMinusCharge;
    public float subtractDMC;
    public bool clampDistance = false;
    public float topY;
    public float bottomY;
    private void OnDrawGizmos()
    {
        if(player != null && enemy != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(enemy.position, minimumRadius);
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(enemy.position, player.position);
            Gizmos.color = Color.cyan;
            direction = player.position - enemy.position;
            distance = direction.magnitude;
            dstMinusCharge = chargeDistance - distance;
            Vector2 temp = -direction.normalized * Mathf.Clamp(dstMinusCharge, 0, Mathf.Infinity);
            Vector2 temp2 = (Vector2)enemy.position;
            Vector2 point = FindIntersection(temp2,temp2 + temp);
            Gizmos.DrawLine(enemy.position, point);
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(point, 0.5f);
        }
    }

    private void Line()
    {
        direction = player.position - enemy.position;
        distance = direction.magnitude;
        dstMinusCharge = chargeDistance - distance;
        Vector2 temp = -direction.normalized * Mathf.Clamp(dstMinusCharge, 0, Mathf.Infinity);
        Vector2 temp2 = (Vector2)enemy.position;
        Vector2 point = FindIntersection(temp2, temp2 + temp);
    }

    Vector2 FindIntersection(Vector2 startPoint, Vector2 endPoint)
    {
        // หาค่า delta_y จากตำแหน่ง y ที่กำหนดและ y ของจุดเริ่มต้นของเส้น
        float delta_y = topY - startPoint.y;
        float delta_y2 = bottomY - startPoint.y;

        // หาค่า t โดยหาร delta_y ด้วยค่า y ของเส้น
        float t = delta_y / (endPoint.y - startPoint.y);
        float t2 = delta_y2 / (endPoint.y - startPoint.y);

        // ตรวจสอบว่าจุดตัดอยู่ในช่วงเส้นหรือไม่
        if (t >= 0f && t <= 1f)
        {
            // นำค่า t ไปคูณกับ vector ของเส้น แล้วบวกกับ vector ของจุดเริ่มต้นของเส้น
            Vector2 intersectionPoint = startPoint + t * (endPoint - startPoint);
            return intersectionPoint;
        }
        else if(t2 >= 0f && t2 <= 1f)
        {
            Vector2 intersectionPoint = startPoint + t2 * (endPoint - startPoint);
            return intersectionPoint;
        }
        else
        {
            // ไม่อยู่ในช่วงเส้น คืนจุดเริ่มต้นของเส้นแทน
            return endPoint;
        }
    }
}
