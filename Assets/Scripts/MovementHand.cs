using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementHand : MonoBehaviour
{
    [SerializeField] private Transform position1; // Primera posición
    [SerializeField] private Transform position2; // Segunda posición
    [SerializeField] private float moveSpeed = 2f; // Velocidad de movimiento del objeto
    private Transform targetPosition; // Posición objetivo actual
    private bool isMoving = false; // Indica si el objeto se está moviendo

    private void Start()
    {
        if (position1 == null || position2 == null)
        {
            Debug.LogError("Las posiciones no están configuradas en el Inspector.");
            return;
        }

        // Establecemos la posición inicial como Position1
        targetPosition = position1;
    }

    private void Update()
    {
        if (isMoving)
        {
            // Mover el objeto hacia la posición objetivo
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, moveSpeed * Time.deltaTime);

            // Verificar si el objeto alcanzó la posición objetivo
            if (Vector3.Distance(transform.position, targetPosition.position) < 0.01f)
            {
                isMoving = false; // Detener el movimiento
            }
        }
    }

    // Llamado desde el botón al presionar
    public void TogglePosition()
    {
        if (targetPosition == position1)
        {
            targetPosition = position2; // Cambiar a la segunda posición
        }
        else
        {
            targetPosition = position1; // Cambiar a la primera posición
        }

        isMoving = true; // Activar el movimiento
    }
}