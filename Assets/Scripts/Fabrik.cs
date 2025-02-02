using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Fabrik : MonoBehaviour
{
    [SerializeField] private Transform[] bones; 
    private float[] bonesLengths; 
    [SerializeField] private int solverIterations = 5; 
    private Transform targetPosition; 
    [SerializeField] private TextMeshProUGUI[] jointAngleTexts;
    [SerializeField] private Transform homePosition; 

    private Quaternion[] initialRotations; 
    private Vector3[] initialPositions; 

    private void Start()
    {
        bonesLengths = new float[bones.Length - 1];
        initialRotations = new Quaternion[bones.Length];
        initialPositions = new Vector3[bones.Length];

        for (int i = 0; i < bones.Length; i++)
        {
            if (i < bones.Length - 1)
            {
                bonesLengths[i] = (bones[i + 1].position - bones[i].position).magnitude;
            }
            initialRotations[i] = bones[i].rotation;
            initialPositions[i] = bones[i].position;
        }

        if (jointAngleTexts.Length != bones.Length)
        {
            Debug.LogError("La cantidad de textos no coincide con la cantidad de huesos.");
        }
    }

    private void Update()
    {
        if (targetPosition != null)
        {
            SolveIK();
        }
        else
        {
            ResetToHome();
        }

        UpdateJointAnglesUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (targetPosition == null && other.CompareTag("Target"))
        {
            targetPosition = other.transform;
            Debug.Log($"Nuevo target detectado: {targetPosition.name}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (targetPosition != null && other.transform == targetPosition)
        {
            Debug.Log($"Target salió del área: {targetPosition.name}");
            targetPosition = null;
        }
    }

    private void SolveIK()
    {
        Vector3[] finalBonesPositions = new Vector3[bones.Length];
        for (int i = 0; i < bones.Length; i++)
        {
            finalBonesPositions[i] = bones[i].position;
        }

        for (int i = 0; i < solverIterations; i++)
        {
            finalBonesPositions = SolveForwardPositions(SolveInversePositions(finalBonesPositions));
        }

        for (int i = 0; i < bones.Length; i++)
        {
            bones[i].position = finalBonesPositions[i];

            if (i < bones.Length - 1)
            {
                Vector3 direction = finalBonesPositions[i + 1] - bones[i].position;
                bones[i].rotation = Quaternion.LookRotation(direction);
            }
            else
            {
                Vector3 direction = targetPosition.position - bones[i].position;
                bones[i].rotation = Quaternion.LookRotation(direction);
            }
        }
    }

    private void ResetToHome()
    {
        for (int i = 0; i < bones.Length; i++)
        {
            bones[i].position = initialPositions[i];
            bones[i].rotation = initialRotations[i];
        }
    }

    private Vector3[] SolveInversePositions(Vector3[] forwardPositions)
    {
        Vector3[] inversePositions = new Vector3[forwardPositions.Length];
        for (int i = forwardPositions.Length - 1; i >= 0; i--)
        {
            if (i == forwardPositions.Length - 1)
            {
                inversePositions[i] = targetPosition.position;
            }
            else
            {
                Vector3 direction = (inversePositions[i + 1] - forwardPositions[i]).normalized;
                inversePositions[i] = inversePositions[i + 1] - direction * bonesLengths[i];
            }
        }
        return inversePositions;
    }

    private Vector3[] SolveForwardPositions(Vector3[] inversePositions)
    {
        Vector3[] forwardPositions = new Vector3[inversePositions.Length];
        for (int i = 0; i < inversePositions.Length; i++)
        {
            if (i == 0)
            {
                forwardPositions[i] = bones[0].position;
            }
            else
            {
                Vector3 direction = (inversePositions[i] - forwardPositions[i - 1]).normalized;
                forwardPositions[i] = forwardPositions[i - 1] + direction * bonesLengths[i - 1];
            }
        }
        return forwardPositions;
    }

    private void UpdateJointAnglesUI()
    {
        for (int i = 0; i < bones.Length; i++)
        {
            if (jointAngleTexts.Length > i && jointAngleTexts[i] != null)
            {
                Vector3 eulerAngles = bones[i].rotation.eulerAngles;
                jointAngleTexts[i].text = $"Joint {i + 1}: X={eulerAngles.x:F1}, Y={eulerAngles.y:F1}, Z={eulerAngles.z:F1}";
            }
        }
    }
}