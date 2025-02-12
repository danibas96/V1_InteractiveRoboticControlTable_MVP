using UnityEngine;

public class PackageFollower : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The target empty object that the robot uses as its IK/constraint target.")]
    public Transform targetEmpty;

    [Header("Follow Settings")]
    [Tooltip("How fast the target follows the package (smaller means slower).")]
    public float followSpeed = 2f;

    // This will hold the package’s transform once it enters the collider.
    private Transform packageTransform;

    // Called when any collider enters the trigger attached to this object.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("package"))
        {
            // Store the package's transform.
            packageTransform = other.transform;
        }
    }

    // When the package leaves, we clear the stored transform.
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("package") && other.transform == packageTransform)
        {
            packageTransform = null;
        }
    }

    private void Update()
    {
        // If there is a package in the collider, move the target gradually toward it.
        if (packageTransform != null && targetEmpty != null)
        {
            // Smoothly interpolate the targetEmpty’s position toward the package’s position.
            targetEmpty.position = Vector3.Lerp(targetEmpty.position, packageTransform.position, followSpeed * Time.deltaTime);
        }
    }
}