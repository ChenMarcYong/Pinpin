using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using UnityEngine.UIElements.Experimental;
using System;
using System.Diagnostics;
using System.Collections.Specialized;
using System.Security.Cryptography;

public class PlayerController : MonoBehaviour
{

    public static PlayerController singleton;


    [Header("Camera")]
    [SerializeField] private GameObject _cameraFollowOB;


    [SerializeField] private GameObject dashParticlesPrefab;

    private float detectionRadius = 0.5f;

    private ParticleSystem dashParticlesInstance;
    public bool hasFireTrail = true;

    public float speedX = 6f;
    public float speedY = 6f;
    public int maxJumpCount = 2;
    public float jumpForce = 10f;
    private int jumpCount;
    private bool isGrounded = false;


    public float scale = 1f;

    public float dashForce = 25f;
    public float dashDuration = 0.12f;
    public float dashCooldown = 2f;
    public float dashDamageFireTrail = 2.5f;
    private bool isDashing = false;
    private float lastDashTime;

    public float vy;

    //public bool isShieldActive = false;

    public float shieldDuration = 2f; // Durée du bouclier actif en secondes
    public float shieldCooldown = 5f; // Cooldown du bouclier en secondes
    private float lastShieldTime = -Mathf.Infinity; // Dernière activation du bouclier
    private bool isShielding = false; // Indique si le bouclier est actif

    public float fallMultiplier = 2.5f;   // Multiplicateur pour accélérer la chute
    public float lowJumpMultiplier = 2f;  // Pour accélérer les sauts courts quand le joueur relâche le bouton

    public Collider2D attackCollider;
    public Transform attackPoint;
    public LayerMask ennemiMask;
    public float attackRange = 0.5f;

    private Animator animator;
    private Rigidbody2D playerRigidbody;
    private Vector2 inputValue; 
    private SpriteRenderer spriteRenderer;

    public GameObject shieldLeft;           // Sprite bouclier à gauche
    public GameObject shieldRight;          // Sprite bouclier à droite

    public int direction = -1;



    public bool IsFacingRight;

    private CameraFollowObject _cameraFollowObject;
    private float _fallSpeedYDampingChangeThreshold;

    private void Awake()
    {
        // Assurez-vous qu'il n'existe qu'une seule instance de ce script
        if (singleton == null) singleton = this;
        else Destroy(gameObject);
    }


    void Start()
    {
        vy = 0;
        playerRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        attackCollider.enabled = false;

        shieldLeft.SetActive(false);
        shieldRight.SetActive(false);

        _cameraFollowObject = _cameraFollowOB.GetComponent<CameraFollowObject>();

        IsFacingRight = true;

        _fallSpeedYDampingChangeThreshold = CameraManager.instance._fallSpeedYDampingChangeThreshold;

        jumpCount = maxJumpCount;

        if (dashParticlesPrefab != null)
        {
            // Instancier le prefab dans la scène à la position du GameObject principal
            GameObject particleObject = Instantiate(dashParticlesPrefab, transform.position, Quaternion.identity);
            particleObject.transform.SetParent(transform);
            dashParticlesInstance = particleObject.GetComponent<ParticleSystem>();
            

            if (dashParticlesInstance != null)
            {
                dashParticlesInstance.gameObject.SetActive(true);
            }
            else
            {
                UnityEngine.Debug.LogError("Le Prefab n'a pas de ParticleSystem attaché !");
            }
        }
        else
        {
            UnityEngine.Debug.LogError("dashParticlesPrefab n'est pas assigné !");
        }
    }

    private void TurnCheck()
    {
        if(inputValue.x > 0 && !IsFacingRight)
        {
            Turn();
        }

        else if (inputValue.x < 0 && IsFacingRight)
        {
            Turn();
        }

    }

    private void Turn()
    {
        if(IsFacingRight)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;
            _cameraFollowObject.CallTurn();
            direction = -1;
        }

        else
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;
            _cameraFollowObject.CallTurn();
            direction = 1;

        }
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(inputValue.x) > 0.0f) TurnCheck();

    }

    void MoveAround()
    {

        if (isDashing) return; // Empêche le mouvement normal pendant le dash
        if (Mathf.Abs(inputValue.x) > 0.0f) 
        {
            if (playerRigidbody) 
            {
                playerRigidbody.velocity = new Vector2(inputValue.x * speedX, playerRigidbody.velocity.y);
                animator.SetFloat("Speed", playerRigidbody.velocity.magnitude);
            }
            if (inputValue.x > 0)
            {
                
                
                
                
                

            }
            else if (inputValue.x < 0)
            {
                

            }
        }
        
        else 
        {
        animator.SetFloat("Speed", 0.0f);
        playerRigidbody.velocity = new Vector2(0, playerRigidbody.velocity.y);
        }

        //UnityEngine.Debug.Log(IsFacingRight);
    }

    void OnMove(InputValue value) 
    {
        //UnityEngine.Debug.Log("move");
        inputValue = value.Get<Vector2>();
        
    }

    void OnAttack()
    {
        //UnityEngine.Debug.Log("Attack");

        //if (!isShieldActive) 
        //{
            animator.SetTrigger("Attack");

            Collider2D[] hitEnnemies = Physics2D.OverlapCircleAll(attackCollider.transform.position, attackRange, ennemiMask);

            /*foreach (Collider2D ennmi in hitEnnemies)
            {
                UnityEngine.Debug.Log("We hit" + ennmi.name);
            }*/
        //}

    }    

/*    void OnShoot() 
    {
        animator.SetTrigger("Attack");
    }*/
    
    void OnJump() 
    {
        if (isGrounded || jumpCount > 0) 
        {
            animator.SetTrigger("Jump");
            animator.SetBool("IsOnGround", false);
            //UnityEngine.Debug.Log("Jump");
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, jumpForce);
            jumpCount--;
        }

        // playerRigidbody.AddForce(0.0f, 5.0f, 0.0f, ForceMode.Impulse);
    }

    void OnDash()
    {
        // Vérifie si le dash est en cooldown
        if (Time.time - lastDashTime >= dashCooldown && !isDashing)
        {
            isDashing = true;
            StartCoroutine(Dash());

            if (hasFireTrail) 
            {
                dashParticlesInstance.Play();
                DetectEnemiesAtStart();
            }

            
        }
    }

    private IEnumerator Dash()
    {
        
        lastDashTime = Time.time;

        // Activer le paramètre "IsDashing" dans l'Animator pour démarrer l'animation
        animator.SetBool("IsDashing", true);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Ennemi"), true);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("EnnemiProjectile"), true);
        // Calculer la direction du dash (vers la droite ou vers la gauche en fonction de `inputValue.x`)


        playerRigidbody.velocity = new Vector2(direction * dashForce, playerRigidbody.velocity.y);

        // Attendre la durée du dash avant de rétablir les contrôles normaux
        yield return new WaitForSeconds(dashDuration);

        // Désactiver "IsDashing" pour arrêter l'animation
        animator.SetBool("IsDashing", false);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Ennemi"), false);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("EnnemiProjectile"), false);

        isDashing = false;
        dashParticlesInstance.Stop();
        enemiesHit.Clear();
    }

    private void DetectEnemiesAtStart()
    {
        // Détecte tous les ennemis dans un rayon autour du joueur
        Vector2 detectionPosition = new Vector2(transform.position.x + direction * 1.5f, transform.position.y);
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(detectionPosition, detectionRadius, LayerMask.GetMask("Ennemi"));

        foreach (Collider2D enemy in enemiesInRange)
        {
            if (!enemiesHit.Contains(enemy))
            {
                enemiesHit.Add(enemy);
                enemy.GetComponent<EnnemiStatus>()?.DamageTaken(dashDamageFireTrail);
                //Debug.Log($"Ennemi touché au début du dash : {enemy.name}");
            }
        }
    }

    private HashSet<Collider2D> enemiesHit = new HashSet<Collider2D>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDashing && hasFireTrail)
        {
            if (collision.CompareTag("Ennemi"))
            {
                collision.GetComponent<EnnemiStatus>().DamageTaken(dashDamageFireTrail);
            }

            if (collision.CompareTag("EnnemiProjectile"))
            {
                Destroy(collision.gameObject);
            }
        }
    }

    void OnShield()
    {
        // Si assez de temps est passé depuis la dernière activation, activer le bouclier
        if (Time.time - lastShieldTime >= shieldCooldown && !isShielding)
        {
            StartCoroutine(ActivateShield());
        }
    }

    private IEnumerator ActivateShield()
    {
        // Activer le bouclier
        isShielding = true;
        lastShieldTime = Time.time;
        shieldLeft.SetActive(true);
        shieldRight.SetActive(true);

        // Garder le bouclier actif pendant la durée spécifiée
        yield return new WaitForSeconds(shieldDuration);

        // Désactiver le bouclier
        shieldLeft.SetActive(false);
        shieldRight.SetActive(false);
        isShielding = false;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Vérifie si le personnage est de nouveau au sol
        /*        if (collision.contacts[0].normal.y > 0.5f && !isGrounded) // vérifie si le contact vient du bas
                {
                    animator.SetBool("IsOnGround", true);
                    isGrounded = true;
                }*/

       // UnityEngine.Debug.Log("number of collisions : " + collision.contactCount);




        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            //UnityEngine.Debug.Log("Nombre de collision avec sol : " + collision.contactCount);
            {
                foreach (ContactPoint2D contact in collision.contacts)
                {
                    //UnityEngine.Debug.Log("contact normal " + contact.normal.y);
                    if (contact.normal.y > 0.5f)
                    {
                        isGrounded = true;
                        animator.SetBool("IsOnGround", true);
                        //UnityEngine.Debug.Log("Touch ground");
                        jumpCount = maxJumpCount;
                        //break;
                    }
                }
                
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = false;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && (collision.contacts[0].normal.y < 0.5f) && (Math.Abs(playerRigidbody.velocity.y) < 0.0001))
        {
            isGrounded = true;
            animator.SetBool("IsOnGround", true);
            jumpCount = maxJumpCount;
        }
    }

    void Update()
    {
        MoveAround();
        vy = playerRigidbody.velocity.y;

        
        //dashParticles.transform.position = transform.position;
        /*        if (playerRigidbody.velocity.y < -0.5f) isGrounded = false;
                else isGrounded = true;*/
        //if (playerRigidbody.velocity.y == 0f) isGrounded = true;
        //else isGrounded = false;
        //ApplyGravityMultiplier();
        //isShieldActive = Keyboard.current.rKey.isPressed;  // ou Input.GetButton("Bouclier") pour l'ancien système Input

        /*if (isShieldActive)
        {
            // Activer le bouclier
            animator.SetBool("IsShielding", true);
            //Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Ennemi"), true);
        }
        else
        {
            // Désactiver le bouclier
            animator.SetBool("IsShielding", false);
            //Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Ennemi"), false);
        }*/


        if (playerRigidbody.velocity.y < _fallSpeedYDampingChangeThreshold)
        {
            CameraManager.instance.LerpScreenY(true);
        }

        if(playerRigidbody.velocity.y >= _fallSpeedYDampingChangeThreshold)
        {
            CameraManager.instance.LerpScreenY(false);
        }
    }

    void ApplyGravityMultiplier()
    {
        // Si le personnage tombe (vitesse vers le bas)
        if (playerRigidbody.velocity.y < 0)
        {
            // Augmente la gravité pour rendre la chute plus rapide
            playerRigidbody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        // Si le personnage saute et qu'on relâche le bouton de saut, cela réduit la hauteur de saut
        else if (playerRigidbody.velocity.y > 0 && !Keyboard.current.spaceKey.isPressed)
        {
            playerRigidbody.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private void OnDrawGizmos()
    {
/*        // Dessine un cercle pour visualiser la portée de détection
        Gizmos.color = Color.red;
        Vector2 detectionPosition = new Vector2(transform.position.x + direction * 1.5f, transform.position.y);
        Gizmos.DrawWireSphere(detectionPosition, detectionRadius);*/
    }


    public bool getIsShielding() 
    {
        return isShielding;
    }
}
