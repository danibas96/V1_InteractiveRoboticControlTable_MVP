using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class FinalJointGrabber : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The designated parent that the package will be forcefully assigned to after being held for 2 seconds.")]
    public Transform designatedParent;

    [Header("Grab Settings")]
    [Tooltip("Time in seconds the package must remain inside the trigger to be grabbed.")]
    public float requiredHoldTime = 2f;

    [Header("Events")]
    [Tooltip("Event to invoke once the package is grabbed (i.e. reparented to the designatedParent).")]
    public UnityEvent OnPackageGrabbed;

    [Tooltip("Event to invoke when the package leaves the small collider (after being grabbed).")]
    public UnityEvent OnPackageRemoved;

    [Tooltip("Reference to the PackageFollower script on the big collider (to enable/disable its detection).")]
    public PackageFollower bigColliderScript;

    // Internally track the package and whether it has been grabbed.
    private GameObject currentPackage;
    private Coroutine grabCoroutine;
    private bool isPackageGrabbed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isPackageGrabbed && other.CompareTag("Package"))
        {
            // If no package is currently tracked, begin the grabbing process.
            if (currentPackage == null)
            {
                currentPackage = other.gameObject;
                Debug.Log($"Package entered the final joint trigger: {other.name}");
                grabCoroutine = StartCoroutine(GrabAfterDelay(other));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Package") && other.gameObject == currentPackage)
        {
            // If the package was successfully grabbed...
            if (isPackageGrabbed)
            {
                Debug.Log($"Package removed from the final joint trigger (grabbed): {other.name}");
                // Unparent the package (forcefully remove it from designatedParent).
                other.transform.SetParent(null);
                OnPackageRemoved?.Invoke();

                // Re-enable detection in the big collider.
                if (bigColliderScript != null)
                {
                    bigColliderScript.SetDetectionEnabled(true);
                }

                isPackageGrabbed = false;
                currentPackage = null;
            }
            else // If the package left before the required hold time...
            {
                if (grabCoroutine != null)
                {
                    StopCoroutine(grabCoroutine);
                    grabCoroutine = null;
                    Debug.Log($"Package exited before grabbing: {other.name}");
                }
                currentPackage = null;
            }
        }
    }

    private IEnumerator GrabAfterDelay(Collider packageCollider)
    {
        float timer = 0f;
        while (timer < requiredHoldTime)
        {
            timer += Time.deltaTime;
            Debug.Log($"Holding package for {timer:F2} seconds...");
            yield return null;
        }

        // After the hold time, forcefully reparent and snap the package to the designated parent's position.
        if (packageCollider != null && designatedParent != null)
        {
            // Reparent while preserving world scale/rotation.
            packageCollider.transform.SetParent(designatedParent, true);
            // Move the package to the parent's exact position.
            packageCollider.transform.localPosition = Vector3.zero;
            Debug.Log($"Package grabbed and reparented to {designatedParent.name} at its position.");

            OnPackageGrabbed?.Invoke(); // Invoke the grabbed event.

            // Disable big collider detection.
            if (bigColliderScript != null)
            {
                bigColliderScript.SetDetectionEnabled(false);
            }

            isPackageGrabbed = true;
        }
    }

}
