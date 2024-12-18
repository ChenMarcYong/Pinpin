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

        int projectileLayer = LayerMask.NameToLayer("EnnemiProjectile");
        int enemyLayer = LayerMask.NameToLayer("Ennemi");
        Physics2D.IgnoreLayerCollision(projectileLayer, enemyLayer);
        Physics2D.IgnoreLayerCollision(projectileLayer, projectileLayer);
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

        else if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && shooter.layer != collision.gameObject.layer)
        {
            collision.gameObject.GetComponent<PlayerStatus>().DamageTaken(damage);
            //UnityEngine.Debug.Log($"Player touché : " + damage);
        }




        if (collision.gameObject != shooter && collision.gameObject.layer != shooter.layer)
        {
            Destroy(gameObject);
        }

        else
        {
            // Ignorer les collisions entre le projectile et le shooter
            Collider2D shooterCollider = gameObject.GetComponent<Collider2D>();
            Collider2D projectileCollider = collision.gameObject.GetComponent<Collider2D>();
            
            if (shooterCollider != null && projectileCollider != null)
            {
                UnityEngine.Debug.Log("aieeee" + shooterCollider + projectileCollider);
                Physics2D.IgnoreCollision(projectileCollider, shooterCollider);
            }
        }


    }

    private void ConditionToDeleteGameObject() 
    {
       if (transform.position.y > heightThreshold) Destroy(gameObject);
    }
}
