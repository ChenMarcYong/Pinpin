using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowross_combat : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject snowball;
    [SerializeField] private Transform snowballSpawnPoint_up;
    [SerializeField] private Transform snowballSpawnPoint_right;
    [SerializeField] private Transform snowballSpawnPoint_down;
    [SerializeField] private Transform snowballSpawnPoint_left;

    private GameObject snowballInst_up;
    private GameObject snowballInst_right;
    private GameObject snowballInst_down;
    private GameObject snowballInst_left;


    void Start()
    {
        StartCoroutine(ShootRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void shoot() 
    {
        if (snowball != null) 
        {
            
            snowballInst_up = Instantiate(snowball, snowballSpawnPoint_up.position, Quaternion.Euler(0, 0, -90));
            snowballInst_down = Instantiate(snowball, snowballSpawnPoint_down.position, Quaternion.Euler(0, 0, 90));

            snowballInst_left = Instantiate(snowball, snowballSpawnPoint_left.position, Quaternion.Euler(0, 0, 0));
            snowballInst_right = Instantiate(snowball, snowballSpawnPoint_right.position, Quaternion.Euler(0, 180, 0));
        }
    }

    private IEnumerator ShootRoutine()
    {
        while (true) // Boucle infinie pour r�p�ter l'action
        {
            shoot(); // Appel de la m�thode shoot
            yield return new WaitForSeconds(0.5f); // Attente de 5 secondes
        }
    }
}
