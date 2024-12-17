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



    void Start()
    {
        currentHealthPoint = MaxHealthPoint;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealthPoint <= 0 && !isAlreadyDead)
        {
            isAlreadyDead = true;
            UnityEngine.Debug.Log("im dead" + name);
            Death(); 
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
            Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Player"), true);
            
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
            
            yield return new WaitForSeconds(2f); // Attend 3 secondes
            Destroy(gameObject); // Détruit le GameObject
        }

    }

}
