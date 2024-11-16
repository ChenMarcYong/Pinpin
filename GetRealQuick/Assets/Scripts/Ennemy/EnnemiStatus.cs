using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiStatus : MonoBehaviour
{
    // Start is called before the first frame update

    public float currentHealthPoint;
    public float MaxHealthPoint = 5f;

    public float attackDammage = 1f;
    private Animator animator;



    void Start()
    {
        currentHealthPoint = MaxHealthPoint;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    if (currentHealthPoint <= 0) Death();
    }

    public void DamageTaken(float damage) 
    {
        animator.SetTrigger("Hurt");
        currentHealthPoint -= damage;
    }

    public void Death() 
    {
        animator.SetBool("isDead", true);
        UnityEngine.Debug.Log("im dead" + name);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Player"), true);
        this.enabled = false;
    }

}
