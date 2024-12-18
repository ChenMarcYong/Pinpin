using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class SnowballBehaviour : MonoBehaviour
{
    [SerializeField] private float normalSnowballSpeed = 15f;
    [SerializeField] private float heightThreshold = 100f;
    private Rigidbody2D rb;

    private float damage = 0.1f;

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

    public void SetShooter(GameObject shooterObject, float shooterDamage)
    {
        shooter = shooterObject;
        damage = shooterDamage;
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ennemi") && shooter.layer != collision.gameObject.layer) 
        {
            collision.gameObject.GetComponent<EnnemiStatus>().DamageTaken(damage);
        }

        else if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && shooter != collision.gameObject)
        {
            collision.gameObject.GetComponent<PlayerStatus>().DamageTaken(damage);
            //UnityEngine.Debug.Log($"Player touch� : " + damage);
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