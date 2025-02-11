using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FollowPath : MonoBehaviour
{
    public Transform roboticArm; // Reference to the robotic arm
    public List<Transform> waypoints; // List of objects in space that define the trajectory
    public float speed = 1.5f; // Movement speed
    private int currentWaypointIndex = 0;
    private bool isFollowingPath = false;

    public Button startButton; // Button to start following the path
    public Button resetButton; // Button to reset the trajectory

    private void Start()
    {
        startButton.onClick.AddListener(StartFollowingPath);
        resetButton.onClick.AddListener(ResetTrajectory);
    }

    public void StartFollowingPath()
    {
        if (waypoints.Count > 0 && !isFollowingPath)
        {
            isFollowingPath = true;
            StartCoroutine(MoveAlongPath());
        }
    }

    private IEnumerator MoveAlongPath()
    {
        while (currentWaypointIndex < waypoints.Count)
        {
            Transform target = waypoints[currentWaypointIndex];

            while (Vector3.Distance(roboticArm.position, target.position) > 0.05f)
            {
                roboticArm.position = Vector3.Lerp(roboticArm.position, target.position, speed * Time.deltaTime);
                yield return null;
            }

            currentWaypointIndex++;
        }

        isFollowingPath = false;
        currentWaypointIndex = 0; // Reset to the start position
    }

    public void ResetTrajectory()
    {
        isFollowingPath = false;
        currentWaypointIndex = 0;
        StopAllCoroutines();
        roboticArm.position = waypoints[0].position; // Move arm back to the first point
    }
}