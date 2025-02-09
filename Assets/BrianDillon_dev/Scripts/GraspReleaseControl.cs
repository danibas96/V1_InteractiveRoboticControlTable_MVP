using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GraspReleaseControl : MonoBehaviour
{
    public bool isGrasping = true;

    public void TurnOnGrasping()
    {
        isGrasping = true;
    }
    
    public void TurnOffGrasping()
    {
        isGrasping = false;
    }
}
