using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementControl : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] public List<GameObject> _movingObjects;
    [SerializeField] private Transform _referenceObject; // Reference to the empty object.
    [SerializeField] private AxisDirection _movementAxis = AxisDirection.Forward; // Enum to select axis.
    [SerializeField] private float _speed = 1.0f;

    private Vector3 _movementVector;

    private void Update()
    {
        // Get the movement vector based on the selected axis.
        _movementVector = GetMovementVector();

        foreach (GameObject obj in _movingObjects)
        {
            // Move the object along the calculated movement vector.
            obj.transform.Translate(_movementVector * Time.deltaTime * _speed, Space.World);
        }
    }

    private Vector3 GetMovementVector()
    {
        if (_referenceObject == null)
        {
            Debug.LogWarning("Reference Object is not assigned.");
            return Vector3.zero;
        }

        // Determine the movement vector based on the selected axis.
        switch (_movementAxis)
        {
            case AxisDirection.Forward:
                return _referenceObject.forward;
            case AxisDirection.Backward:
                return -_referenceObject.forward;
            case AxisDirection.Right:
                return _referenceObject.right;
            case AxisDirection.Left:
                return -_referenceObject.right;
            case AxisDirection.Up:
                return _referenceObject.up;
            case AxisDirection.Down:
                return -_referenceObject.up;
            default:
                return Vector3.zero;
        }
    }

    // Enum to select the axis for movement.
    public enum AxisDirection
    {
        Forward,
        Backward,
        Right,
        Left,
        Up,
        Down
    }
}