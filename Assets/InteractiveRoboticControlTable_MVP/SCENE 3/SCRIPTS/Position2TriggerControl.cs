using System.Collections;
using System.Collections.Generic;
using Meta.XR.Editor.Tags;
using Oculus.Interaction.PoseDetection;
using UnityEngine;

public class Position2TriggerControl : MonoBehaviour
{
    [SerializeField] private GameObject _conveyorBelt;
    //Required tag for packages, default if package
    [SerializeField] private Tag _requiredTag;
    //Bool to determine if object is destroyed when it stops
    [SerializeField] private bool _destroyOnExit;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_requiredTag))
        {
            _conveyorBelt.GetComponent<MovementControl>()._movingObjects.Remove(other.gameObject);
            
            if (_destroyOnExit)
            {
                other.gameObject.SetActive(false);
            }
        }
    }
}