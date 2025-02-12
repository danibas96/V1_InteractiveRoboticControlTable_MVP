using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class FinalJointGrabber : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The target empty object that will become the package’s parent.")]
    public Transform targetEmpty;

    [Header("Grab Settings")]
    [Tooltip("Time in seconds the package must remain inside the trigger to be grabbed.")]
    public float requiredHoldTime = 2f;

    [Header("Events")]
    [Tooltip("Event to invoke once the package is grabbed.")]
    public UnityEvent OnPackageGrabbed;

    // We keep a reference to the currently detected package.
    private GameObject currentPackage;

    // A reference to the coroutine so we can cancel it if needed.
    private Coroutine grabCoroutine;

    // Called when a collider enters the trigger.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("package") && currentPackage == null)
        {
            currentPackage = other.gameObject;
            // Start the grabbing routine.
            grabCoroutine = StartCoroutine(GrabAfterDelay(other));
        }
    }

    // Called when a collider exits the trigger.
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("package") && other.gameObject == currentPackage)
        {
            // Cancel the grab coroutine if the package leaves before the required time.
            if (grabCoroutine != null)
            {
                StopCoroutine(grabCoroutine);
                grabCoroutine = null;
            }
            currentPackage = null;
        }
    }

    // Coroutine that waits for the package to stay inside the trigger for a given time.
    private IEnumerator GrabAfterDelay(Collider packageCollider)
    {
        float timer = 0f;
        while (timer < requiredHoldTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        // If the package is still here after the delay, parent it to the target empty.
        if (packageCollider != null && targetEmpty != null)
        {
            packageCollider.transform.SetParent(targetEmpty);
            // Optionally, reset its local position if you need it to be exactly aligned:
            // packageCollider.transform.localPosition = Vector3.zero;

            // Invoke any additional event (for example, to trigger UI or other behaviors).
            OnPackageGrabbed?.Invoke();
        }
    }
}
