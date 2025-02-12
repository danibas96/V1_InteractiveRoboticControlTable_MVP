using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    // List of scene names to reference in the Inspector
    [SerializeField] private List<string> sceneNames;

    /// <summary>
    /// Switches to the scene at the given index.
    /// </summary>
    /// <param name="sceneIndex">Index of the scene to load.</param>
    public void SwitchScene(int sceneIndex)
    {
        if (sceneIndex >= 0 && sceneIndex < sceneNames.Count)
        {
            string sceneToLoad = sceneNames[sceneIndex];
            Debug.Log($"Switching to scene: {sceneToLoad}");
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogError("Invalid scene index! Please ensure the index is within the range of the scene list.");
        }
    }
}