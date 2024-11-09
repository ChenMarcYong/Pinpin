using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    // Start is called before the first frame update


    public LayerMask ennemiMask;
    public float attackRange = 0.5f;
    public Transform attackPoint;
    public Collider2D attackCollider;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnAttack() 
    {
        UnityEngine.Debug.Log("Attack");
        Collider2D[] hitEnnemies = Physics2D.OverlapCircleAll(attackCollider.transform.position, attackRange, ennemiMask);

        foreach(Collider2D ennmi in hitEnnemies) 
        {
            UnityEngine.Debug.Log("We hit" + ennmi.name);
        }
        //attackCollider.enabled = false;
    }



    void OnDrawGizmosSelected() 
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
