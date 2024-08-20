using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    GameWaypoints gameWaypoints;
    [SerializeField] private float speed = 1f;
    [SerializeField] private Vector3[] paths;
    [SerializeField] private int currentPathIndex = 0;
    private Vector3 currentPath;
    void Start()
    {
        gameWaypoints = FindFirstObjectByType<GameWaypoints>();
        paths = gameWaypoints.Waypoints;
        currentPath = paths[currentPathIndex];
    }
    void Update()
    {
        Move();
        CheckEndPath();
    }

    private void Move()
    {
        Vector3 nextMove = Vector3.MoveTowards(transform.position, currentPath, speed * Time.deltaTime);
        transform.position = nextMove;

    }
    private void CheckEndPath()
    {
        float magnitude = (transform.position - currentPath).magnitude;
        if (magnitude < 0.01f)
        {
            transform.position = currentPath;
            if (currentPathIndex == paths.Length - 1)
            {
                currentPathIndex = 0;
            }
            else
            {
                currentPathIndex++;
            }
            currentPath = paths[currentPathIndex];
        }
    }
}
