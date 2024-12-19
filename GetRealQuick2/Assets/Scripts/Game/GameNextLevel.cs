using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameNextLevel : MonoBehaviour
{

    void Update()
    {
        if (!AreEnemiesPresent())
        {
            LoadNextLevel();
        }
    }

    private bool AreEnemiesPresent()
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.layer == LayerMask.NameToLayer("Ennemi"))
            {
                return true; 
            }
        }

        return false; 
    }

   
    private void LoadNextLevel()
    {

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (SceneManager.sceneCountInBuildSettings > currentSceneIndex + 1)
        {
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
        else
        {
            UnityEngine.Debug.Log("Pas de niveau suivant disponible.");
        }
    }
}
