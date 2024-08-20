using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWaypoints : MonoBehaviour
{
    [SerializeField] private Vector3[] wayPoints;
    public Vector3[] Waypoints => wayPoints;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (wayPoints.Length > 0)
            for (int i = 0; i < wayPoints.Length; i++)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(wayPoints[i], 0.5f);
                if (i > 0)
                {
                    Gizmos.DrawLine(wayPoints[i - 1], wayPoints[i]);
                }
            }
    }
#endif

}
