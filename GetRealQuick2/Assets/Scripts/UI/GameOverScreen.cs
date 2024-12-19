using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public void setup() 
    {
        gameObject.SetActive(true);
    }

    public void restartButton() 
    {
        SceneManager.LoadScene("level_boss");
    }
}
