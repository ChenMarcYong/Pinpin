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
    public bool isCycling = false;

    public float radius = 3f;
    public int numbersOfSnowBalls = 3;
    public float swiftness = 3f;
    public float durationTime = 3f;
    public float shieldCooldown = 10f;
    private float damage = 1f;

    private EnnemiStatus status;
    bool isDead = false;
    public bool HasShield = false;
    private bool IsShielding = false;

    void Start()
    {
        radius = transform.localScale.x * 2;
        StartCoroutine(ShootRoutine());

        if (HasShield) 
        {
            
            StartCoroutine(CircularAttackRoutine());
            
        }
        status = GetComponent<EnnemiStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate() 
    {
        status = GetComponent<EnnemiStatus>();
        isDead = status.getIsAlreadyDead();
    }

    void shoot() 
    {
        if (snowball != null && !isDead && !IsShielding) 
        {
            if (shootDirection == 0f || shootDirection == 2f) 
            {
                snowballInst_up = Instantiate(snowball, snowballSpawnPoint_up.position, Quaternion.Euler(0, 0, -90));
                AdjustSnowballScale(snowballInst_up);
                snowballInst_up.GetComponent<SnowballBehaviour>().SetShooter(gameObject, damage);

                // Bas
                snowballInst_down = Instantiate(snowball, snowballSpawnPoint_down.position, Quaternion.Euler(0, 0, 90));
                AdjustSnowballScale(snowballInst_down);
                snowballInst_down.GetComponent<SnowballBehaviour>().SetShooter(gameObject, damage);

                // Gauche
                snowballInst_left = Instantiate(snowball, snowballSpawnPoint_left.position, Quaternion.Euler(0, 0, 0));
                AdjustSnowballScale(snowballInst_left);
                snowballInst_left.GetComponent<SnowballBehaviour>().SetShooter(gameObject, damage);

                // Droite
                snowballInst_right = Instantiate(snowball, snowballSpawnPoint_right.position, Quaternion.Euler(0, 180, 0));
                AdjustSnowballScale(snowballInst_right);
                snowballInst_right.GetComponent<SnowballBehaviour>().SetShooter(gameObject, damage);
            }


            if (shootDirection == 1f || shootDirection == 2f)
            {
                snowballInst_right_up_corner = Instantiate(snowball, snowballSpawnPoint_right_up_corner.position, Quaternion.Euler(0, 0, -135));
                AdjustSnowballScale(snowballInst_right_up_corner);
                snowballInst_right_up_corner.GetComponent<SnowballBehaviour>().SetShooter(gameObject, damage);

                // Haut droite
                snowballInst_right_down_corner = Instantiate(snowball, snowballSpawnPoint_right_down_corner.position, Quaternion.Euler(0, 0, 135));
                AdjustSnowballScale(snowballInst_right_down_corner);
                snowballInst_right_down_corner.GetComponent<SnowballBehaviour>().SetShooter(gameObject, damage);

                // Bas gauche
                snowballInst_left_up_corner = Instantiate(snowball, snowballSpawnPoint_left_up_corner.position, Quaternion.Euler(0, 0, -45));
                AdjustSnowballScale(snowballInst_left_up_corner);
                snowballInst_left_up_corner.GetComponent<SnowballBehaviour>().SetShooter(gameObject, damage);

                // Bas droite
                snowballInst_left_down_cornert = Instantiate(snowball, snowballSpawnPoint_left_down_corner.position, Quaternion.Euler(0, 0, 45));
                AdjustSnowballScale(snowballInst_left_down_cornert);
                snowballInst_left_down_cornert.GetComponent<SnowballBehaviour>().SetShooter(gameObject, damage);
            }

            if (isCycling) 
            {
                shootDirection = (shootDirection + 1) % 3;
            }
            


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

    private void AdjustSnowballScale(GameObject snowballInstance)
    {
        // Récupérer l'échelle du parent
        Vector3 parentScale = transform.localScale;

        // Appliquer l'échelle à la snowball
        snowballInstance.transform.localScale = new Vector3(
            snowballInstance.transform.localScale.x * parentScale.x,
            snowballInstance.transform.localScale.y * parentScale.y,
            snowballInstance.transform.localScale.z * parentScale.z
        );
    }


    private IEnumerator CircularAttack(float duration, int numProjectiles, float radius, float rotationSpeed)
    {
        IsShielding = true;
        List<GameObject> snowballs = new List<GameObject>();
        
        // Instanciation des projectiles en cercle
        for (int i = 0; i < numProjectiles; i++)
        {
            float angle = i * (360f / numProjectiles);
            Vector3 spawnPosition = transform.position + new Vector3(
                Mathf.Cos(angle * Mathf.Deg2Rad) * radius,
                Mathf.Sin(angle * Mathf.Deg2Rad) * radius,
                0
            );

            GameObject snowballInst = Instantiate(snowball, spawnPosition, Quaternion.identity);
            AdjustSnowballScale(snowballInst);
            snowballInst.GetComponent<SnowballBehaviour>().SetShooter(gameObject, damage);
            snowballs.Add(snowballInst);
        }

        // Temps total de l'attaque
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            for (int i = 0; i < snowballs.Count; i++)
            {
                if (isDead) 
                {
                    foreach (GameObject sb in snowballs)
                    {
                        if (sb != null) Destroy(sb);
                    }
                    break;
                }

                if (snowballs[i] != null)
                {
                    float angle = (elapsedTime * rotationSpeed + i * (360f / numProjectiles)) % 360f;
                    Vector3 newPos = transform.position + new Vector3(
                        Mathf.Cos(angle * Mathf.Deg2Rad) * radius,
                        Mathf.Sin(angle * Mathf.Deg2Rad) * radius,
                        0
                    );

                    snowballs[i].transform.position = newPos;
                }
            }

            elapsedTime += Time.deltaTime;
            
            yield return null;
        }
        IsShielding = false;
        // Détruire les projectiles après la fin de l'attaque
        if (!isDead) 
        {
            foreach (GameObject sb in snowballs)
            {
                if (sb != null) Destroy(sb);
            }
        }


    }

    private IEnumerator CircularAttackRoutine()
    {
        while (!isDead) // Tant que l'ennemi n'est pas mort
        {
            
            yield return new WaitForSeconds(shieldCooldown); // Attendre 10 secondes
            
            StartCoroutine(CircularAttack(durationTime, numbersOfSnowBalls, radius, swiftness)); // Lancer l'attaque circulaire
            
        }
    }

}
