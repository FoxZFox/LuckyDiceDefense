using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameWaypoints))]
public class GameWaypointsEditor : Editor
{
    GameWaypoints gameWaypoints => target as GameWaypoints;
    private void OnSceneGUI()
    {
        Handles.color = Color.blue;
        Vector3 snap = new Vector3(0.1f, 0.1f, 0);
        for (int i = 0; i < gameWaypoints.Waypoints.Length; i++)
        {
            EditorGUI.BeginChangeCheck();
            Vector3 currentWaypoint = gameWaypoints.Waypoints[i];
            Vector3 newWayPoint = Handles.FreeMoveHandle(currentWaypoint, 0.7f, snap, Handles.SphereHandleCap);
            // Debug.Log(newWayPoint);
            newWayPoint.x = Mathf.Round(newWayPoint.x * 2f) / 2f;
            newWayPoint.y = Mathf.Round(newWayPoint.y * 2f) / 2f;
            // newWayPoint.x = Mathf.Round(newWayPoint.x);
            // newWayPoint.y = Mathf.Round(newWayPoint.y);
            GUIStyle wayPoinIndex = new GUIStyle { fontStyle = FontStyle.Bold, fontSize = 14 };
            wayPoinIndex.normal.textColor = Color.green;
            Vector3 textPosition = (Vector3.up * 0.8f) + (Vector3.right * 0.45f);
            Handles.Label(currentWaypoint + textPosition, $"{i + 1}", wayPoinIndex);
            GUIStyle wayPointPosition = new GUIStyle { fontStyle = FontStyle.Bold, fontSize = 14 };
            wayPointPosition.normal.textColor = Color.green;
            textPosition = (Vector3.down * 0.5f) + (Vector3.left * 0.8f);
            Handles.Label(currentWaypoint + textPosition, $"(X:{currentWaypoint.x},Y:{currentWaypoint.y})", wayPointPosition);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Change Waypoint Position");
                gameWaypoints.Waypoints[i] = newWayPoint;
            }
        }
    }
}