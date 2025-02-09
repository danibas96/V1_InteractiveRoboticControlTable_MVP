using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPickupControl : MonoBehaviour
{
    [SerializeField] private GraspReleaseControl _graspReleaseControl;
    [SerializeField] private Transform _hand;
    private Vector3 _originalRotation;
    private Rigidbody _rb;

    private void Start()
    {
        _originalRotation = transform.localEulerAngles;
        _rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (_graspReleaseControl.isGrasping && other.gameObject == _hand.gameObject)
        {
            _rb.isKinematic = true;
            transform.localEulerAngles = _originalRotation;
            transform.position = _hand.position + new Vector3(0, -0.05f, 0);
            transform.parent = _hand;
        }
        else
        {
            _rb.isKinematic = false;
            transform.parent = null;
        }
    }
}
