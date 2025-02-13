using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayAudioOnPositionChange : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The GameObject whose movement will trigger the audio.")]
    public Transform targetObject;

    [Header("Audio Settings")]
    [Tooltip("Threshold distance to detect movement (to avoid triggering for very small movements).")]
    public float movementThreshold = 0.01f;

    private Vector3 lastPosition;
    private AudioSource audioSource;

    private void Start()
    {
        // Cache the initial position and AudioSource component.
        if (targetObject == null)
        {
            Debug.LogError("No target object assigned! Please assign a target object in the Inspector.");
            enabled = false;
            return;
        }

        lastPosition = targetObject.position;
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true; // Ensure the audio loops while the object is moving.
    }

    private void Update()
    {
        if (targetObject == null) return;

        // Calculate the distance moved since the last frame.
        float distanceMoved = Vector3.Distance(targetObject.position, lastPosition);

        // If the object has moved beyond the threshold, play the audio.
        if (distanceMoved > movementThreshold)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
                Debug.Log($"Playing audio because {targetObject.name} is moving.");
            }
        }
        else
        {
            // If the object has stopped moving, stop the audio.
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
                Debug.Log($"Stopped audio because {targetObject.name} stopped moving.");
            }
        }

        // Update the last known position.
        lastPosition = targetObject.position;

    }
}