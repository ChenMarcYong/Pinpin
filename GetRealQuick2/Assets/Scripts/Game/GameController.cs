using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{
    public static GameController singleton;

    public GameOverScreen gameOverScreen;


    public void GameOver() 
    {
        gameOverScreen.setup();
    }


    private void Awake()
    {
        // Assurez-vous qu'il n'existe qu'une seule instance de ce script
        if (singleton == null) singleton = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
