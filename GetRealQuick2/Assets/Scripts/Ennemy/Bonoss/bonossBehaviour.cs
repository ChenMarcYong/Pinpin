using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class bonossBehaviour : MonoBehaviour
{
    [Header("Param�tres de l'IA")]
    public Transform player;           // R�f�rence au joueur
    public float speed = 3f;           // Vitesse de d�placement
    public float stoppingDistance = 2f; // Distance d'arr�t autour du joueur

    private Rigidbody2D rb;            // Rigidbody de l'IA
    private bool isFacingRight = true;

    private EnnemiStatus status;
    private Animator animator;

    public Collider2D attackCollider;
    public float attackRange = 0.5f;
    public float attackDammage = 1f;
    public LayerMask playerMask;

    public float timeBetweenAttacks = 1f; // Temps entre deux attaques
    private float nextAttackTime = 0f;    // Timer pour g�rer les attaques

    private bool PlayerGetsDamaged = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        status = GetComponent<EnnemiStatus>();
        animator = GetComponent<Animator>();


        // Trouve le joueur automatiquement s'il n'est pas assign� dans l'inspecteur
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
            else
            {
                UnityEngine.Debug.LogError("Joueur introuvable ! Assurez-vous qu'il a le tag 'Player'.");
            }
        }
    }

    void FixedUpdate()
    {
        PlayerGetsDamaged = PlayerShield.singleton.getGetsDamaged();

        status = GetComponent<EnnemiStatus>();

        //UnityEngine.Debug.Log(player.position);
        if (player != null && !status.getIsAlreadyDead())
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            player = playerObject.transform;
            // Calcule la direction uniquement sur l'axe X
            float directionX = player.position.x - transform.position.x;

            // V�rifie la distance sur l'axe X uniquement
            
            if (Mathf.Abs(directionX) > stoppingDistance)
            {
                // Applique la vitesse uniquement sur l'axe X
                rb.velocity = new Vector2(Mathf.Sign(directionX) * speed, rb.velocity.y);
                
                RotateTowardsPlayer(directionX);
            }

            





            else
            {
                // Arr�te le mouvement sur l'axe X
                rb.velocity = new Vector2(0, rb.velocity.y);
                if (Time.time >= nextAttackTime)
                {
                    OnAttack(); // Lance l'attaque
                    nextAttackTime = Time.time + timeBetweenAttacks; // R�initialise le timer d'attaque
                }
            }
            animator.SetFloat("Speed", rb.velocity.magnitude);
        }
    }

    void RotateTowardsPlayer(float directionX)
    {
        // D�termine si l'IA doit regarder � gauche ou � droite
        if (directionX > 0)
        {
            // Tourne vers la droite (0 degr�s en Y)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (directionX < 0)
        {
            // Tourne vers la gauche (180 degr�s en Y)
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    void FlipCharacter(float directionX)
    {
        // V�rifie si le personnage doit se retourner
        if ((directionX > 0 && !isFacingRight) || (directionX < 0 && isFacingRight))
        {
            isFacingRight = !isFacingRight; // Change l'�tat actuel

            // Applique une rotation en Y de 180 degr�s
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }


    void OnAttack()
    {
        animator.SetTrigger("Attack");

        LayerMask combinedMask = LayerMask.GetMask("Player", "Shield");

        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(attackCollider.transform.position, attackRange, combinedMask);

        bool shieldDetected = false;

        foreach (Collider2D target in hitTargets)
        {
            
            if (target.gameObject.layer == LayerMask.NameToLayer("Shield"))
            {
                //UnityEngine.Debug.Log("Bouclier d�tect�, aucun d�g�t inflig� !");
                shieldDetected = true;
                break;
            }
        }

        if (!shieldDetected) 
        {
            foreach (Collider2D target in hitTargets)
            {
                PlayerStatus playerStatus = target.GetComponent<PlayerStatus>();
                if (playerStatus != null)
                {
                    playerStatus.DamageTaken(attackDammage);
                }
            }
        }
    }

    
}
