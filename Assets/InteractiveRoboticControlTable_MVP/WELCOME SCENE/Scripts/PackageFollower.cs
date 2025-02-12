using UnityEngine;

public class PackageFollower : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The target empty object that the robot uses as its IK/constraint target.")]
    public Transform targetEmpty;

    [Header("Follow Settings")]
    [Tooltip("How fast the target follows the package (smaller means slower).")]
    public float followSpeed = 2f;

    private Transform packageTransform;

    // To enable or disable detection from the big collider.
    private bool isDetectionEnabled = true;

    private void OnTriggerEnter(Collider other)
    {
        if (isDetectionEnabled && other.CompareTag("Package"))
        {
            packageTransform = other.transform;
            Debug.Log($"Package entered the detection zone: {other.name}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isDetectionEnabled && other.CompareTag("Package") && other.transform == packageTransform)
        {
            packageTransform = null;
            Debug.Log($"Package exited the detection zone: {other.name}");
        }
    }

    private void Update()
    {
        if (isDetectionEnabled && packageTransform != null && targetEmpty != null)
        {
            targetEmpty.position = Vector3.Lerp(targetEmpty.position, packageTransform.position, followSpeed * Time.deltaTime);
            Debug.DrawLine(targetEmpty.position, packageTransform.position, Color.green);
        }
    }

    // Public method to toggle detection (can be called by other scripts).
    public void SetDetectionEnabled(bool enabled)
    {
        isDetectionEnabled = enabled;
        if (!enabled)
        {
            packageTransform = null; // Clear the target if detection is disabled.
            Debug.Log("Big collider detection disabled.");
        }
        else
        {
            Debug.Log("Big collider detection enabled.");
        }
    }
}