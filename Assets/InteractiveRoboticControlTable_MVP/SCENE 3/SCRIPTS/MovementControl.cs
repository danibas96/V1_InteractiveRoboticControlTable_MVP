using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementControl : MonoBehaviour
{
    [SerializeField] public List<GameObject> _movingObjects;
    [SerializeField] private Vector3 _movementVector;
    private float _speed = 1.0f;

    private void Update()
    {
        foreach (GameObject obj in _movingObjects)
        {
            obj.transform.Translate(_movementVector * Time.deltaTime * _speed);
        }
    }
}