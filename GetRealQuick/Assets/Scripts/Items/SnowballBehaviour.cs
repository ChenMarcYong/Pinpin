using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballBehaviour : MonoBehaviour
{
    [SerializeField] private float normalSnowballSpeed = 15f;

    private Rigidbody2D rb;

    private void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
        SetStraightVelocity();
    }

    private void SetStraightVelocity() 
    {
        rb.velocity = -transform.right * normalSnowballSpeed;
    }

    private void setDestroyOnImpact() 
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        
        
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ennemi")) 
        {
            collision.gameObject.GetComponent<EnnemiStatus>().DamageTaken(5);
        }

        if (collision.gameObject.layer != LayerMask.NameToLayer("Player")) 
        {
            Destroy(gameObject);
        }


    }
}
