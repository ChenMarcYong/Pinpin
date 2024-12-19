using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStatus : MonoBehaviour
{
    private Animator animator;

    [Header("Health Settings")]
    public float currentHealthPoint;
    public float MaxHealthPoint = 5f;

    private bool isAlreadyDead = false;

    public float attackDammage = 1f;
    

    [Header("Recovery Settings")]
    public float recoveryTime = 1.0f; // Temps de r�cup�ration en secondes
    private bool canTakeDamage = true;

    [Header("Player Settings")]
    [SerializeField] private PlayerInput playerInput;

    // Start is called before the first frame update
    void Start()
    {
        currentHealthPoint = MaxHealthPoint;
        animator = GetComponent<Animator>();


    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealthPoint <= 0)
        {
            Die();
        }
    }

    public void DamageTaken(float damage)
    {
        if (canTakeDamage && !isAlreadyDead)
        {
            animator.SetTrigger("Hurt");
            currentHealthPoint -= damage;

            StartCoroutine(RecoveryCooldown());
        }
    }

    private IEnumerator RecoveryCooldown()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(recoveryTime);
        canTakeDamage = true;
    }

    private void Die()
    {
        isAlreadyDead = true;
        animator.SetTrigger("Hurt");
        DisablePlayerInput();
        //UnityEngine.Debug.Log("Dead");
        DestroyAllProjectiles();
        DisableAllEnemyScripts();
        
        FindObjectOfType<CameraZoomOnDeath>().TriggerZoomOnDeath();

    }

    private void DestroyAllEnemies()
    {
        int enemyLayer = LayerMask.NameToLayer("Ennemi");

        // Trouver tous les objets dans la sc�ne
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // V�rifier si l'objet est sur le layer "Ennemi"
            if (obj.layer == enemyLayer)
            {
                Destroy(obj);
            }
        }
    }

    private void DisableAllEnemyScripts()
    {
        int enemyLayer = LayerMask.NameToLayer("Ennemi");

        // Trouver tous les objets dans la sc�ne
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // V�rifier si l'objet est sur le layer "Ennemi"
            if (obj.layer == enemyLayer)
            {
                // D�sactiver tous les scripts attach�s � l'objet
                MonoBehaviour[] scripts = obj.GetComponents<MonoBehaviour>();
                foreach (MonoBehaviour script in scripts)
                {
                    script.enabled = false;
                }

            }
        }

    }

    private void DestroyAllProjectiles()
    {
        int enemyProjectileLayer = LayerMask.NameToLayer("EnnemiProjectile");

        // Trouver tous les objets dans la sc�ne
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // V�rifier si l'objet est sur le layer "Ennemi"
            if (obj.layer == enemyProjectileLayer)
            {
                Destroy(obj);
            }
        }
    }

    private void DisablePlayerInput()
    {
        if (playerInput != null)
        {
            playerInput.DeactivateInput(); // D�sactive toutes les entr�es
        }
    }
}
