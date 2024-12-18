using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class bonossBehaviour : MonoBehaviour
{
    [Header("Paramètres de l'IA")]
    public Transform player;           // Référence au joueur
    public float speed = 3f;           // Vitesse de déplacement
    public float stoppingDistance = 1f; // Distance d'arrêt autour du joueur

    private Rigidbody2D rb;            // Rigidbody de l'IA
    private bool isFacingRight = true;

    private EnnemiStatus status;
    private Animator animator;

    public Collider2D attackCollider;
    public float attackRange = 0.5f;
    public float attackDammage = 1f;
    public LayerMask playerMask;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        status = GetComponent<EnnemiStatus>();
        animator = GetComponent<Animator>();


        // Trouve le joueur automatiquement s'il n'est pas assigné dans l'inspecteur
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
        status = GetComponent<EnnemiStatus>();

        //UnityEngine.Debug.Log(player.position);
        if (player != null && !status.getIsAlreadyDead())
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            player = playerObject.transform;
            // Calcule la direction uniquement sur l'axe X
            float directionX = player.position.x - transform.position.x;

            // Vérifie la distance sur l'axe X uniquement
            OnAttack();
            if (Mathf.Abs(directionX) > stoppingDistance)
            {
                // Applique la vitesse uniquement sur l'axe X
                rb.velocity = new Vector2(Mathf.Sign(directionX) * speed, rb.velocity.y);
                RotateTowardsPlayer(directionX);
            }
            



            else
            {
                // Arrête le mouvement sur l'axe X
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }
    }

    void RotateTowardsPlayer(float directionX)
    {
        // Détermine si l'IA doit regarder à gauche ou à droite
        if (directionX > 0)
        {
            // Tourne vers la droite (0 degrés en Y)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (directionX < 0)
        {
            // Tourne vers la gauche (180 degrés en Y)
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    void FlipCharacter(float directionX)
    {
        // Vérifie si le personnage doit se retourner
        if ((directionX > 0 && !isFacingRight) || (directionX < 0 && isFacingRight))
        {
            isFacingRight = !isFacingRight; // Change l'état actuel

            // Applique une rotation en Y de 180 degrés
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }


    void OnAttack()
    {
        animator.SetTrigger("Hurt");
        Collider2D[] hitEnnemies = Physics2D.OverlapCircleAll(attackCollider.transform.position, attackRange, playerMask);

        foreach (Collider2D ennmi in hitEnnemies)
        {
            ennmi.GetComponent<PlayerStatus>().DamageTaken(attackDammage);
            //UnityEngine.Debug.Log("aieeee" + attackDammage);
        }
    }
}
