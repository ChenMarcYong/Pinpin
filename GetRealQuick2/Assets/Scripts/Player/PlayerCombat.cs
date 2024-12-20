using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject snowball;
    [SerializeField] private Transform snowballSpawnPoint;
    [SerializeField] private ParticleSystem dashParticles;

    private GameObject snowballInst;


    public LayerMask ennemiMask;
    public float attackRange = 1.25f;
    public Transform attackPoint;
    public Collider2D attackCollider;



    public float attackDammage = 1f;
    public float snowballDamage = 2f;
    public float cooldownTime = 1.0f;
    private bool canShoot = true;
    void Start()
    {
        int shield = LayerMask.NameToLayer("Shield");
        int player = LayerMask.NameToLayer("Player");
        Physics2D.IgnoreLayerCollision(shield, player);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("PlayerProjectile"), player);
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
        if (canShoot)
        {
            if (PlayerController.singleton != null)
            {
                int direction = PlayerController.singleton.direction;
                Quaternion rotation = direction == -1 ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);
                GameObject snowballInst = Instantiate(snowball, snowballSpawnPoint.position, rotation);
                snowballInst.GetComponent<SnowballBehaviour>().SetShooter(gameObject, snowballDamage);

                StartCoroutine(CooldownCoroutine());
            }
        }
    }

    private IEnumerator CooldownCoroutine()
    {
        canShoot = false;
        yield return new WaitForSeconds(cooldownTime);
        canShoot = true;
    }



    void OnDrawGizmosSelected() 
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
