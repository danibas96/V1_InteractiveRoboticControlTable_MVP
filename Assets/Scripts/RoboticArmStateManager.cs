using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.SceneManagement; 
public class RoboticArmStateManager : MonoBehaviour
{
    public enum ArmState { Idle, PickAndPlace, FollowingPath }
    private ArmState currentState = ArmState.Idle; // ✅ Start in Idle Mode

    [Header("Robot and Objects")]
    public Transform roboticArm; // Robotic arm  
    public GameObject objectPrefab; // Prefab to instantiate  
    public Transform pickupTable; // Table where objects are picked from  

    [Header("Movement Settings")]
    public float speed = 1.5f;
    private List<Vector3> spawnPoints = new List<Vector3>(); // List of saved positions  
    private bool isFollowingPath = false;
    private bool isPlacingObjects = false;

    private void Start()
    {
        ActivateIdleMode(); // ✅ Ensure we start in Idle mode
    }

    // ✅ 1️⃣ Activate Idle Mode (Connected to the "Idle" button in Meta SDK When Select)
    public void ActivateIdleMode()
    {
        if (currentState == ArmState.Idle) return;

        currentState = ArmState.Idle;
        Debug.Log("Switched to Idle Mode - Move freely & save positions.");
    }

    // ✅ 2️⃣ Save the robot's current position as a waypoint (Connected to "Record" button)
    public void RecordSpawnPoint()
    {
        spawnPoints.Add(roboticArm.position);
        Debug.Log("Saved Position: " + roboticArm.position);
    }

    // ✅ 3️⃣ Pick and Place Mode: Moves objects from the table to saved positions (Connected to "Pick & Place" button)
    public void StartPickAndPlace()
    {
        if (spawnPoints.Count == 0) return;

        currentState = ArmState.PickAndPlace;
        Debug.Log("Starting Pick and Place mode.");

        isPlacingObjects = true;
        StartCoroutine(PickAndPlaceObjects());
    }

    private IEnumerator PickAndPlaceObjects()
    {
        foreach (Vector3 targetPosition in spawnPoints)
        {
            // 🟢 Pick Object from Table
            GameObject obj = Instantiate(objectPrefab, pickupTable.position, Quaternion.identity);
            yield return MoveToPosition(roboticArm, pickupTable.position); // Move to table
            yield return AttachObjectToArm(obj); // Attach object
            yield return MoveToPosition(roboticArm, targetPosition); // Move to placement
            yield return ReleaseObject(obj); // Release object
        }

        isPlacingObjects = false;
        ActivateIdleMode();
        Debug.Log("All objects placed.");
    }

    // ✅ 4️⃣ Follow Path Mode: Move robot along the recorded trajectory (Connected to "Following Path" button)
    public void StartFollowingPath()
    {
        if (spawnPoints.Count == 0) return;

        currentState = ArmState.FollowingPath;
        Debug.Log("Following recorded path.");

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
        ActivateIdleMode();
        Debug.Log("Path completed.");
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

    // ✅ Attach object to the robotic arm (Simulated "Grab")
    private IEnumerator AttachObjectToArm(GameObject obj)
    {
        obj.transform.SetParent(roboticArm);
        Debug.Log("Object grabbed.");
        yield return new WaitForSeconds(1f);
    }

    // ✅ Release object from the robotic arm
    private IEnumerator ReleaseObject(GameObject obj)
    {
        obj.transform.SetParent(null);
        Debug.Log("Object placed.");
        yield return new WaitForSeconds(1f);
    }

    // ✅ 5️⃣ Reset Simulation (Connected to "Reset" button)
   /* public void ResetScene()
    {
        Debug.Log("🔄 Resetting scene...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }*/
}