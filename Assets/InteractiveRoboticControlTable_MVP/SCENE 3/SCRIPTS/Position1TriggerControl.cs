using System.Collections;
using System.Collections.Generic;
using Meta.XR.Editor.Tags;
using UnityEngine;

public class Position1TriggerControl : MonoBehaviour
{
    [SerializeField] private GameObject _conveyorBelt;
    //Required Tag for packages, default is Package
    [SerializeField] private Tag _requiredTag;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_requiredTag))
        {
            _conveyorBelt.GetComponent<MovementControl>()._movingObjects.Add(other.gameObject);
        }
    }
}
