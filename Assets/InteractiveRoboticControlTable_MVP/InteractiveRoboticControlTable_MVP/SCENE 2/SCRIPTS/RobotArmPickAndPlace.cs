using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotArmPickAndPlace : MonoBehaviour
{

    [Header("Robot & Movement Settings")]
    public Transform roboticArm; // The robotic arm
    public Transform spawnGrab; // Offset position for holding the object
    public Transform placePosition; // Final drop location
    public float speed = 1.5f; // Movement speed

    private Transform detectedObject = null; // The current object to pick up
    private Queue<Transform> objectQueue = new Queue<Transform>(); // Queue for multiple objects
    private HashSet<Transform> placedObjects = new HashSet<Transform>(); // Stores placed objects
    private bool isCarryingObject = false; // Prevents multiple pickups
    private Rigidbody objectRigidbody; // Reference to the target's Rigidbody
    private Vector3 originalScale; // Stores the world scale of the object

    [Header("Detection Zone Collider (Not part of the Robot)")]
    public Collider detectionZone; // External collider to detect objects

    private void Start()
    {
        detectedObject = null;
        isCarryingObject = false;
    }

    // 🔹 Detect objects that enter the detection zone
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target") && !placedObjects.Contains(other.transform))
        {
            Debug.Log($"🎯 New target detected: {other.name}");

            if (!objectQueue.Contains(other.transform))
            {
                objectQueue.Enqueue(other.transform);
            }

            if (!isCarryingObject)
            {
                StartCoroutine(PickAndPlaceLoop());
            }
        }
    }

    // 🔹 Pick and Place Loop: Moves the robot to each detected object and places it
    private IEnumerator PickAndPlaceLoop()
    {
        while (objectQueue.Count > 0)
        {
            isCarryingObject = true;
            detectedObject = objectQueue.Dequeue();

            // 🏁 Move robotic arm to the detected object's position
            yield return MoveToPosition(roboticArm, detectedObject.position);

            // 🖐️ Grab the object (Attach without affecting scale)
            GrabObject(detectedObject);

            // 🚚 Move the robotic arm to the placement position while carrying the object
            yield return MoveToPosition(roboticArm, placePosition.position);

            // 🎯 Release the object at the final position
            ReleaseObject();

            Debug.Log("✅ Object placed successfully.");
            isCarryingObject = false;

            // ❗ Ensure the robot does not pick this object again
            placedObjects.Add(detectedObject);

            // ❌ Remove the placed object from the detection queue
            RemovePlacedObjectFromQueue(detectedObject);

            detectedObject = null;

            // 🗑️ Remove already placed objects so only new objects are detected
            ClearPlacedObjectsFromQueue();
        }
    }

    // ✅ Move robotic arm smoothly to a target position
    private IEnumerator MoveToPosition(Transform obj, Vector3 targetPosition)
    {
        while (Vector3.Distance(obj.position, targetPosition) > 0.05f)
        {
            obj.position = Vector3.Lerp(obj.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }
    }

    // ✅ Grab and attach object to the robot without affecting scale
    private void GrabObject(Transform obj)
    {
        objectRigidbody = obj.GetComponent<Rigidbody>();

        if (objectRigidbody != null)
        {
            objectRigidbody.isKinematic = true; // Disable physics while being carried
            objectRigidbody.useGravity = false;
        }

        // Store the original world scale before parenting
        originalScale = obj.lossyScale;

        // Attach the object to the robot's grab point
        obj.SetParent(spawnGrab, true);

        // Restore the original world scale
        obj.localScale = Vector3.one;
        obj.localScale = new Vector3(
            originalScale.x / spawnGrab.lossyScale.x,
            originalScale.y / spawnGrab.lossyScale.y,
            originalScale.z / spawnGrab.lossyScale.z
        );

        Debug.Log("🔵 Object grabbed and moving with the robotic arm.");
    }

    // ✅ Release object at the final position and re-enable physics
    private void ReleaseObject()
    {
        if (detectedObject == null) return;

        detectedObject.SetParent(null); // Detach from the robotic arm
        detectedObject.position = placePosition.position; // Ensure final placement

        // Restore the original world scale
        detectedObject.localScale = originalScale;

        if (objectRigidbody != null)
        {
            objectRigidbody.isKinematic = false; // Reactivate physics
            objectRigidbody.useGravity = true;
        }

        Debug.Log("🔴 Object released and physics restored.");
    }

    // ✅ Remove the placed object from the detection queue
    private void RemovePlacedObjectFromQueue(Transform obj)
    {
        if (objectQueue.Contains(obj))
        {
            Queue<Transform> newQueue = new Queue<Transform>();

            foreach (Transform queuedObj in objectQueue)
            {
                if (queuedObj != obj)
                {
                    newQueue.Enqueue(queuedObj);
                }
            }

            objectQueue = newQueue;
            Debug.Log($"❌ Removed {obj.name} from queue after placement.");
        }
    }

    // ✅ Removes placed objects from the queue to prevent re-selection
    private void ClearPlacedObjectsFromQueue()
    {
        Queue<Transform> newQueue = new Queue<Transform>();

        foreach (Transform obj in objectQueue)
        {
            if (!placedObjects.Contains(obj)) // Keep only unplaced objects
            {
                newQueue.Enqueue(obj);
            }
        }

        objectQueue = newQueue;
    }
}