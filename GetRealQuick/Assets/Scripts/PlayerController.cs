using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using UnityEngine.UIElements.Experimental;
using System;
using System.Diagnostics;

public class PlayerController : MonoBehaviour
{
    public float speedX = 5f;
    public float speedY = 5f;
    public float jumpForce = 5f;
    private bool isGrounded = true;

    private Animator animator;
    private Rigidbody2D rigidbody;
    private Vector2 inputValue;

    void Start()
    {
        rigidbody = FindObjectOfType<Rigidbody2D>();
        animator = FindObjectOfType<Animator>();
    }

    void MoveAround()
    {
        // if(Mathf.Abs(inputValue.x) > 0.0f) 
        // {
            if (rigidbody) 
            {
                rigidbody.velocity = new Vector2(inputValue.x * speedX, rigidbody.velocity.y);
                animator.SetFloat("Speed", rigidbody.velocity.magnitude);
            }
            else 
            {
                animator.SetFloat("Speed", 0.0f);
            }
        // }

        // else 
        // {
        //     animator.SetFloat("Speed", 0.0f);
        // }
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
            UnityEngine.Debug.Log("Jump");
            isGrounded = false;
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
        }

        // rigidbody.AddForce(0.0f, 5.0f, 0.0f, ForceMode.Impulse);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Vérifie si le personnage est de nouveau au sol
        if (collision.contacts[0].normal.y > 0.5f) // vérifie si le contact vient du bas
        {
            isGrounded = true;
        }
    }

    void Update()
    {
        MoveAround();
    }
}
