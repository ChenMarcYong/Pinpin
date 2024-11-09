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
    public float jumpForce = 7f;
    private bool isGrounded = false;

    public float dashForce = 25f;
    public float dashDuration = 0.12f;
    public float dashCooldown = 2f;

    private bool isDashing = false;
    private float lastDashTime;



    private Animator animator;
    private Rigidbody2D rigidbody;
    private Vector2 inputValue; 
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void MoveAround()
    {

        if (isDashing) return; // Empêche le mouvement normal pendant le dash
        if (Mathf.Abs(inputValue.x) > 0.0f) 
        {
            if (rigidbody) 
            {
                rigidbody.velocity = new Vector2(inputValue.x * speedX, rigidbody.velocity.y);
                animator.SetFloat("Speed", rigidbody.velocity.magnitude);
            }
            if (inputValue.x > 0)
            {
                spriteRenderer.flipX = false; // Vers la droite
            }
            else if (inputValue.x < 0)
            {
                spriteRenderer.flipX = true; // Vers la gauche
            }


        }

        else 
        {
        animator.SetFloat("Speed", 0.0f);
        rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
        }
    }

    void OnMove(InputValue value) 
    {
        UnityEngine.Debug.Log("move");
        inputValue = value.Get<Vector2>();
        
    }

    void OnAttack()
    {
        UnityEngine.Debug.Log("Attack");
        animator.SetTrigger("Attack");
    }    
    
    void OnJump() 
    {
        if (isGrounded) 
        {
            animator.SetTrigger("Jump");
            animator.SetBool("IsOnGround", false);
            UnityEngine.Debug.Log("Jump");
            isGrounded = false;
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
        }

        // rigidbody.AddForce(0.0f, 5.0f, 0.0f, ForceMode.Impulse);
    }

    void OnDash()
    {
        // Vérifie si le dash est en cooldown
        if (Time.time - lastDashTime >= dashCooldown && !isDashing)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        lastDashTime = Time.time;

        // Activer le paramètre "IsDashing" dans l'Animator pour démarrer l'animation
        animator.SetBool("IsDashing", true);

        // Calculer la direction du dash (vers la droite ou vers la gauche en fonction de `inputValue.x`)
        float dashDirection = inputValue.x != 0 ? Mathf.Sign(inputValue.x) : (spriteRenderer.flipX ? -1 : 1);
        rigidbody.velocity = new Vector2(dashDirection * dashForce, rigidbody.velocity.y);

        // Attendre la durée du dash avant de rétablir les contrôles normaux
        yield return new WaitForSeconds(dashDuration);

        // Désactiver "IsDashing" pour arrêter l'animation
        animator.SetBool("IsDashing", false);
        isDashing = false;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Vérifie si le personnage est de nouveau au sol
        if (collision.contacts[0].normal.y > 0.5f && !isGrounded) // vérifie si le contact vient du bas
        {
            animator.SetBool("IsOnGround", true);
            isGrounded = true;
        }
    }

    void Update()
    {
        MoveAround();
    }
}
