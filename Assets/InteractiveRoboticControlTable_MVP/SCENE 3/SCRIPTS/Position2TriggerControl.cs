using System.Collections;
using System.Collections.Generic;
using Meta.XR.Editor.Tags;
using Oculus.Interaction.PoseDetection;
using UnityEngine;

public class Position2TriggerControl : MonoBehaviour
{
    [SerializeField] private GameObject _conveyorBelt;
    //Required tag for packages, default is Package
    [SerializeField] private Tag _requiredTag;
    //Bool to determine if object is destroyed when it stops
    [SerializeField] private bool _destroyOnExit;
    //Delay time in seconds
    [SerializeField] private float _delayTime = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_requiredTag))
        {
            StartCoroutine(HandleTriggerExitAfterDelay(other));
        }
    }

    private IEnumerator HandleTriggerExitAfterDelay(Collider other)
    {
        // Wait for the specified delay time
        yield return new WaitForSeconds(_delayTime);

        // Remove the object from the moving list
        _conveyorBelt.GetComponent<MovementControl>()._movingObjects.Remove(other.gameObject);

        // Optionally destroy the object or deactivate it
        if (_destroyOnExit)
        {
            other.gameObject.SetActive(false);
        }
    }
}