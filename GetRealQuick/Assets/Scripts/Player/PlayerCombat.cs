using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject snowball;
    [SerializeField] private Transform snowballSpawnPoint;

    private GameObject snowballInst;


    public LayerMask ennemiMask;
    public float attackRange = 0.5f;
    public Transform attackPoint;
    public Collider2D attackCollider;



    public float attackDammage = 1;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnAttack() 
    {
       // if (!GetComponent<PlayerController>().isShieldActive)
        //{
            //UnityEngine.Debug.Log("Attack");
            Collider2D[] hitEnnemies = Physics2D.OverlapCircleAll(attackCollider.transform.position, attackRange, ennemiMask);

            foreach (Collider2D ennmi in hitEnnemies)
            {
                //UnityEngine.Debug.Log("We hit" + ennmi.name);
                ennmi.GetComponent<EnnemiStatus>().DamageTaken(attackDammage);

            }
       // }

        //attackCollider.enabled = false;
    }

    void OnShoot() 
    {
        if (PlayerController.singleton != null) 
        {
            int direction = PlayerController.singleton.direction;
            UnityEngine.Debug.Log("snowball direction : " + direction + " position : " + snowballSpawnPoint.position);
            //UnityEngine.Debug.Log("snowProjectile");
            Quaternion rotation = direction == -1 ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);
            snowballInst = Instantiate(snowball, snowballSpawnPoint.position, rotation) ;
        }


    }



    void OnDrawGizmosSelected() 
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
