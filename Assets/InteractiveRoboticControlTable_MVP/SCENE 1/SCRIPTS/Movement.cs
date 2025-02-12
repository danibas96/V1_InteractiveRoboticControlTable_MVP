using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.1f; // Incremento de movimiento
    private bool isMovingXUp, isMovingXDown;
    private bool isMovingYUp, isMovingYDown;
    private bool isMovingZUp, isMovingZDown;

    private void Update()
    {
        // Lógica para mover el objeto continuamente mientras las variables son true
        if (isMovingXUp) transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        if (isMovingXDown) transform.position -= Vector3.right * moveSpeed * Time.deltaTime;

        if (isMovingYUp) transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        if (isMovingYDown) transform.position -= Vector3.up * moveSpeed * Time.deltaTime;

        if (isMovingZUp) transform.position += Vector3.forward * moveSpeed * Time.deltaTime;
        if (isMovingZDown) transform.position -= Vector3.forward * moveSpeed * Time.deltaTime;
    }

    // Métodos para activar/desactivar el movimiento
    public void StartMoveXUp() => isMovingXUp = true;
    public void StopMoveXUp() => isMovingXUp = false;

    public void StartMoveXDown() => isMovingXDown = true;
    public void StopMoveXDown() => isMovingXDown = false;

    public void StartMoveYUp() => isMovingYUp = true;
    public void StopMoveYUp() => isMovingYUp = false;

    public void StartMoveYDown() => isMovingYDown = true;
    public void StopMoveYDown() => isMovingYDown = false;

    public void StartMoveZUp() => isMovingZUp = true;
    public void StopMoveZUp() => isMovingZUp = false;

    public void StartMoveZDown() => isMovingZDown = true;
    public void StopMoveZDown() => isMovingZDown = false;
}

