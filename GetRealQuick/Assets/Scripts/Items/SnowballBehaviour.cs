using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class SnowballBehaviour : MonoBehaviour
{
    [SerializeField] private float normalSnowballSpeed = 15f;
    [SerializeField] private float heightThreshold = 100f;
    private Rigidbody2D rb;

    private GameObject shooter;


    private void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
        SetStraightVelocity();
    }

    private void Update()
    {
        ConditionToDeleteGameObject();
    }

    private void SetStraightVelocity() 
    {
        rb.velocity = -transform.right * normalSnowballSpeed;
    }

    private void setDestroyOnImpact() 
    {
        
    }

    public void SetShooter(GameObject shooterObject)
    {
        shooter = shooterObject;
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ennemi") && shooter != collision.gameObject) 
        {
            collision.gameObject.GetComponent<EnnemiStatus>().DamageTaken(5);
        }

        else if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && shooter != collision.gameObject)
        {
            //collision.gameObject.GetComponent<PlayerStatus>()?.TakeDamage(5);
            UnityEngine.Debug.Log($"Player touché : ");
        }

        if (collision.gameObject != shooter) 
        {
            Destroy(gameObject);
        }
    }

    private void ConditionToDeleteGameObject() 
    {
       if (transform.position.y > heightThreshold) Destroy(gameObject);
    }
}
