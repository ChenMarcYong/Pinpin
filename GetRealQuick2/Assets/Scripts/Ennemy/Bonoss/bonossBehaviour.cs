using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class bonossBehaviour : MonoBehaviour
{
    [Header("Param�tres de l'IA")]
    public Transform player;           // R�f�rence au joueur
    public float speed = 3f;           // Vitesse de d�placement
    public float stoppingDistance = 1f; // Distance d'arr�t autour du joueur

    private Rigidbody2D rb;            // Rigidbody de l'IA
    private bool isFacingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

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
        UnityEngine.Debug.Log(player.position);
        if (player != null)
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
                FlipCharacter(directionX);
            }

            

            else
            {
                // Arr�te le mouvement sur l'axe X
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }
    }

    void RotateTowardsPlayer(Vector2 direction)
    {
        // Calcule l'angle vers le joueur
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Applique la rotation uniquement sur l'axe Z
        transform.rotation = Quaternion.Euler(0, angle, 0);
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
}
