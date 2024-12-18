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

    [SerializeField] private Transform snowballSpawnPoint_right_up_corner;
    [SerializeField] private Transform snowballSpawnPoint_right_down_corner;
    [SerializeField] private Transform snowballSpawnPoint_left_up_corner;
    [SerializeField] private Transform snowballSpawnPoint_left_down_corner;

    private GameObject snowballInst_up;
    private GameObject snowballInst_right;
    private GameObject snowballInst_down;
    private GameObject snowballInst_left;

    private GameObject snowballInst_right_up_corner;
    private GameObject snowballInst_right_down_corner;
    private GameObject snowballInst_left_up_corner;
    private GameObject snowballInst_left_down_cornert;

    public float shootDirection = 1f;
    public float TimeBetweenShoot = 1f;

    private float damage = 1f;


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

            if (shootDirection == 0f || shootDirection == 2f) 
            {
                snowballInst_up = Instantiate(snowball, snowballSpawnPoint_up.position, Quaternion.Euler(0, 0, -90));
                snowballInst_up.GetComponent<SnowballBehaviour>().SetShooter(gameObject, damage);

                // Bas
                snowballInst_down = Instantiate(snowball, snowballSpawnPoint_down.position, Quaternion.Euler(0, 0, 90));
                snowballInst_down.GetComponent<SnowballBehaviour>().SetShooter(gameObject, damage);

                // Gauche
                snowballInst_left = Instantiate(snowball, snowballSpawnPoint_left.position, Quaternion.Euler(0, 0, 0));
                snowballInst_left.GetComponent<SnowballBehaviour>().SetShooter(gameObject, damage);

                // Droite
                snowballInst_right = Instantiate(snowball, snowballSpawnPoint_right.position, Quaternion.Euler(0, 180, 0));
                snowballInst_right.GetComponent<SnowballBehaviour>().SetShooter(gameObject, damage);
            }


            if (shootDirection == 1f || shootDirection == 2f)
            {
                snowballInst_right_up_corner = Instantiate(snowball, snowballSpawnPoint_right_up_corner.position, Quaternion.Euler(0, 0, -135));
                snowballInst_right_up_corner.GetComponent<SnowballBehaviour>().SetShooter(gameObject, damage);

                // Haut droite
                snowballInst_right_down_corner = Instantiate(snowball, snowballSpawnPoint_right_down_corner.position, Quaternion.Euler(0, 0, 135));
                snowballInst_right_down_corner.GetComponent<SnowballBehaviour>().SetShooter(gameObject, damage);

                // Bas gauche
                snowballInst_left_up_corner = Instantiate(snowball, snowballSpawnPoint_left_up_corner.position, Quaternion.Euler(0, 0, -45));
                snowballInst_left_up_corner.GetComponent<SnowballBehaviour>().SetShooter(gameObject, damage);

                // Bas droite
                snowballInst_left_down_cornert = Instantiate(snowball, snowballSpawnPoint_left_down_corner.position, Quaternion.Euler(0, 0, 45));
                snowballInst_left_down_cornert.GetComponent<SnowballBehaviour>().SetShooter(gameObject, damage);
            }


            shootDirection = (shootDirection + 1) % 3;


        }
    }

    private IEnumerator ShootRoutine()
    {
        while (true) // Boucle infinie pour répéter l'action
        {
            shoot(); // Appel de la méthode shoot
            yield return new WaitForSeconds(TimeBetweenShoot); // Attente de 5 secondes
        }
    }
}
