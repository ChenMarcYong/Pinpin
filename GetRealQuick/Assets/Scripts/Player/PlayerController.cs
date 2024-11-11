using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using UnityEngine.UIElements.Experimental;
using System;
using System.Diagnostics;

public class PlayerController : MonoBehaviour
{
    public float speedX = 6f;
    public float speedY = 6f;
    public float jumpForce = 10f;
    private bool isGrounded = false;


    public float scale = 1f;

    public float dashForce = 25f;
    public float dashDuration = 0.12f;
    public float dashCooldown = 2f;

    private bool isDashing = false;
    private float lastDashTime;

    public bool isShieldActive = false;

    public float fallMultiplier = 2.5f;   // Multiplicateur pour acc�l�rer la chute
    public float lowJumpMultiplier = 2f;  // Pour acc�l�rer les sauts courts quand le joueur rel�che le bouton

    public Collider2D attackCollider;
    public Transform attackPoint;
    public LayerMask ennemiMask;
    public float attackRange = 0.5f;

    private Animator animator;
    private Rigidbody2D playerRigidbody;
    private Vector2 inputValue; 
    private SpriteRenderer spriteRenderer;

    private int direction;


    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        attackCollider.enabled = false;
    }

    void MoveAround()
    {

        if (isDashing) return; // Emp�che le mouvement normal pendant le dash
        if (Mathf.Abs(inputValue.x) > 0.0f) 
        {
            if (playerRigidbody) 
            {
                playerRigidbody.velocity = new Vector2(inputValue.x * speedX, playerRigidbody.velocity.y);
                animator.SetFloat("Speed", playerRigidbody.velocity.magnitude);
            }
            if (inputValue.x > 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                //spriteRenderer.flipX = false; // Vers la droite
                //attackPoint.localPosition = new Vector3(Mathf.Abs(attackPoint.localPosition.x), attackPoint.localPosition.y, attackPoint.localPosition.z);

            }
            else if (inputValue.x < 0)
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                //spriteRenderer.flipX = true; // Vers la gauche
                //attackPoint.localPosition = new Vector3(- Mathf.Abs(attackPoint.localPosition.x), attackPoint.localPosition.y, attackPoint.localPosition.z);

            }


        }

        else 
        {
        animator.SetFloat("Speed", 0.0f);
        playerRigidbody.velocity = new Vector2(0, playerRigidbody.velocity.y);
        }
    }

    void OnMove(InputValue value) 
    {
        //UnityEngine.Debug.Log("move");
        inputValue = value.Get<Vector2>();
        
    }

    void OnAttack()
    {
        //UnityEngine.Debug.Log("Attack");

        if (!isShieldActive) 
        {
            animator.SetTrigger("Attack");

            Collider2D[] hitEnnemies = Physics2D.OverlapCircleAll(attackCollider.transform.position, attackRange, ennemiMask);

            foreach (Collider2D ennmi in hitEnnemies)
            {
                UnityEngine.Debug.Log("We hit" + ennmi.name);
            }
        }

    }    
    
    void OnJump() 
    {
        if (isGrounded) 
        {
            animator.SetTrigger("Jump");
            animator.SetBool("IsOnGround", false);
            UnityEngine.Debug.Log("Jump");
            isGrounded = false;
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, jumpForce);
        }

        // playerRigidbody.AddForce(0.0f, 5.0f, 0.0f, ForceMode.Impulse);
    }

    void OnDash()
    {
        // V�rifie si le dash est en cooldown
        if (Time.time - lastDashTime >= dashCooldown && !isDashing)
        {
            StartCoroutine(Dash());
        }
    }

    void OnShield(InputValue value)
    {
        // Active ou d�sactive le bouclier en fonction de l'�tat du bouton (enfonc� ou rel�ch�)

        //isShieldActive = value.isPressed
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        lastDashTime = Time.time;

        // Activer le param�tre "IsDashing" dans l'Animator pour d�marrer l'animation
        animator.SetBool("IsDashing", true);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Ennemi"), true);
        // Calculer la direction du dash (vers la droite ou vers la gauche en fonction de `inputValue.x`)
        float dashDirection = inputValue.x != 0 ? Mathf.Sign(inputValue.x) : (spriteRenderer.flipX ? -1 : 1);
        playerRigidbody.velocity = new Vector2(dashDirection * dashForce, playerRigidbody.velocity.y);

        // Attendre la dur�e du dash avant de r�tablir les contr�les normaux
        yield return new WaitForSeconds(dashDuration);

        // D�sactiver "IsDashing" pour arr�ter l'animation
        animator.SetBool("IsDashing", false);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Ennemi"), false);
        isDashing = false;
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        // V�rifie si le personnage est de nouveau au sol
        if (collision.contacts[0].normal.y > 0.5f && !isGrounded) // v�rifie si le contact vient du bas
        {
            animator.SetBool("IsOnGround", true);
            isGrounded = true;
        }
    }

    void Update()
    {
        MoveAround();
        //ApplyGravityMultiplier();
        isShieldActive = Keyboard.current.rKey.isPressed;  // ou Input.GetButton("Bouclier") pour l'ancien syst�me Input

        if (isShieldActive)
        {
            // Activer le bouclier
            animator.SetBool("IsShielding", true);
            //Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Ennemi"), true);
        }
        else
        {
            // D�sactiver le bouclier
            animator.SetBool("IsShielding", false);
            //Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Ennemi"), false);
        }
    }

    void ApplyGravityMultiplier()
    {
        // Si le personnage tombe (vitesse vers le bas)
        if (playerRigidbody.velocity.y < 0)
        {
            // Augmente la gravit� pour rendre la chute plus rapide
            playerRigidbody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        // Si le personnage saute et qu'on rel�che le bouton de saut, cela r�duit la hauteur de saut
        else if (playerRigidbody.velocity.y > 0 && !Keyboard.current.spaceKey.isPressed)
        {
            playerRigidbody.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
