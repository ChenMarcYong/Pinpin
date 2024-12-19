using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class EnnemiStatus : MonoBehaviour
{
    // Start is called before the first frame update

    public float currentHealthPoint;
    public float MaxHealthPoint = 5f;

    public float attackDammage = 1f;
    private Animator animator;

    private bool isAlreadyDead = false;

    private float TimeAfterDeath = 1.5f;


    void Start()
    {
        currentHealthPoint = MaxHealthPoint * transform.localScale.x;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealthPoint <= 0 && !isAlreadyDead)
        {
            isAlreadyDead = true;
            //UnityEngine.Debug.Log("im dead" + name);
            Death(); 



        }

        if (isAlreadyDead) 
        {
            Collider2D[] enemyColliders = GetComponents<Collider2D>();
            // Ignore les collisions avec tous les autres objets sauf le sol
            foreach (Collider2D enemyCollider in enemyColliders)
            {
                // Récupère tous les colliders présents dans la scène
                Collider2D[] allColliders = FindObjectsOfType<Collider2D>();

                foreach (Collider2D otherCollider in allColliders)
                {
                    // Ignore les collisions sauf avec les objets sur la couche "Ground"
                    if (otherCollider.gameObject.layer != LayerMask.NameToLayer("Ground"))
                    {
                        Physics2D.IgnoreCollision(enemyCollider, otherCollider, true);
                    }
                }
            }
        }
        
    }

    public void DamageTaken(float damage) 
    {
        if (!isAlreadyDead) 
        {
            animator.SetTrigger("Hurt");
            currentHealthPoint -= damage;
        }

    }

    public void Death() 
    {
        if (animator != null)
        {
            
            animator.SetBool("isDead", true);
            //Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Player"), true);
            


        }
        
        //UnityEngine.Debug.Log("im dead" + name);
        
       // Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Item"), true);
        //this.enabled = false;

    }

    public void OndeathAnimation() 
    {
        StartCoroutine(DestroyAfterDelay());
    }

    private IEnumerator DestroyAfterDelay()
    {
        {
            
            yield return new WaitForSeconds(TimeAfterDeath); // Attend 3 secondes
            Destroy(gameObject); // Détruit le GameObject
        }

    }

    public bool getIsAlreadyDead() 
    {
        return isAlreadyDead;
    }

}
