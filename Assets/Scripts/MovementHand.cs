using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementHand : MonoBehaviour
{
    [SerializeField] private Transform position1; // Primera posici�n
    [SerializeField] private Transform position2; // Segunda posici�n
    [SerializeField] private float moveSpeed = 2f; // Velocidad de movimiento del objeto
    private Transform targetPosition; // Posici�n objetivo actual
    private bool isMoving = false; // Indica si el objeto se est� moviendo

    private void Start()
    {
        if (position1 == null || position2 == null)
        {
            Debug.LogError("Las posiciones no est�n configuradas en el Inspector.");
            return;
        }

        // Establecemos la posici�n inicial como Position1
        targetPosition = position1;
    }

    private void Update()
    {
        if (isMoving)
        {
            // Mover el objeto hacia la posici�n objetivo
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, moveSpeed * Time.deltaTime);

            // Verificar si el objeto alcanz� la posici�n objetivo
            if (Vector3.Distance(transform.position, targetPosition.position) < 0.01f)
            {
                isMoving = false; // Detener el movimiento
            }
        }
    }

    // Llamado desde el bot�n al presionar
    public void TogglePosition()
    {
        if (targetPosition == position1)
        {
            targetPosition = position2; // Cambiar a la segunda posici�n
        }
        else
        {
            targetPosition = position1; // Cambiar a la primera posici�n
        }

        isMoving = true; // Activar el movimiento
    }
}