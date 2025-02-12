using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IKmOVEMENT : MonoBehaviour
{
     [SerializeField] private Transform[] bones; // Bones of the arm/chain
    [SerializeField] private Transform targetPosition; // Assigned manually in the inspector
    [SerializeField] private int solverIterations = 5; // Number of iterations for solving
    [SerializeField] private Transform homePosition; // Default resting position

    private float[] bonesLengths;
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
}