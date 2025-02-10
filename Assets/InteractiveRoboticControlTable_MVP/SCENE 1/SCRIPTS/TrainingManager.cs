using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TrainingManager : MonoBehaviour
{
    public enum ArmState { None, Idle, FollowingPath }
    private ArmState currentState = ArmState.None; // ✅ Starts in a neutral state

    [Header("Robot and Objects")]
    public Transform roboticArm; // Robotic arm  

    [Header("Movement Settings")]
    public float speed = 1.5f;
    private List<Vector3> spawnPoints = new List<Vector3>(); // List of saved positions  
    private List<GameObject> positionMarkers = new List<GameObject>(); // Stores the spheres with text
    private bool isFollowingPath = false;

    [Header("UI Elements")]
    public GameObject StartMotion; // UI objects

    [Header("Position Marker Prefab")]
    public GameObject positionMarkerPrefab; // Prefab for position markers (Sphere + Text)

    private void Start()
    {
        Debug.Log("⚡ Ready to Start! Select a mode.");
    }

    // ✅ 1️⃣ Activate Idle Mode (Connected to "Idle" button in Meta SDK When Select)
    public void ActivateIdleMode()
    {
        if (currentState == ArmState.Idle) return;

        currentState = ArmState.Idle;
        Debug.Log("🚀 Switched to Idle Mode - Move freely & save positions.");

        // Show UI elements
        StartMotion.SetActive(true);
    }

    // ✅ 2️⃣ Save the robot's current position as a waypoint (Connected to "Record" button)
    public void RecordSpawnPoint()
    {
        Vector3 position = roboticArm.position;
        spawnPoints.Add(position);
        Debug.Log("📍 Saved Position: " + position);

        // Instantiate Position Marker (Sphere with Text)
        GameObject marker = Instantiate(positionMarkerPrefab, position, Quaternion.identity);
        positionMarkers.Add(marker);

        // Update the text
        TMP_Text markerText = marker.GetComponentInChildren<TMP_Text>();
        if (markerText != null)
        {
            markerText.text = "P" + spawnPoints.Count;
        }
    }

    // ✅ 3️⃣ Delete All Recorded Waypoints (Connected to "Delete Trajectory" button)
    public void DeleteTrajectory()
    {
        spawnPoints.Clear();
        Debug.Log("🗑️ All saved positions cleared.");

        // Destroy all markers
        foreach (GameObject marker in positionMarkers)
        {
            Destroy(marker);
        }
        positionMarkers.Clear();
    }

    // ✅ 4️⃣ Follow Path Mode: Move robot along the recorded trajectory (Connected to "Following Path" button)
    public void StartFollowingPath()
    {
        if (spawnPoints.Count == 0) return;

        currentState = ArmState.FollowingPath;
        Debug.Log("🚶‍♂️ Following recorded path.");

        // Hide UI elements while following the path
        StartMotion.SetActive(false);

        // 🟡 Hide all position markers while moving
        foreach (GameObject marker in positionMarkers)
        {
            marker.SetActive(false);
        }

        isFollowingPath = true;
        StartCoroutine(FollowPath());
    }

    private IEnumerator FollowPath()
    {
        foreach (Vector3 targetPosition in spawnPoints)
        {
            yield return MoveToPosition(roboticArm, targetPosition);
        }

        isFollowingPath = false;

        // 🟢 Re-enable position markers after movement is complete
        foreach (GameObject marker in positionMarkers)
        {
            marker.SetActive(true);
        }

        Debug.Log("✔️ Path completed.");
    }

    // ✅ Move object/robot smoothly to target positions
    private IEnumerator MoveToPosition(Transform obj, Vector3 targetPosition)
    {
        while (Vector3.Distance(obj.position, targetPosition) > 0.05f)
        {
            obj.position = Vector3.Lerp(obj.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }
    }
}