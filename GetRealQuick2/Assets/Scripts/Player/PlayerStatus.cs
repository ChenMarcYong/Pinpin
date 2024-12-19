using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    private Animator animator;

    [Header("Health Settings")]
    public float currentHealthPoint;
    public float MaxHealthPoint = 5f;

    private bool isAlreadyDead = false;

    public float attackDammage = 1f;
    

    [Header("Recovery Settings")]
    public float recoveryTime = 1.0f; // Temps de récupération en secondes
    private bool canTakeDamage = true;

    // Start is called before the first frame update
    void Start()
    {
        currentHealthPoint = MaxHealthPoint;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DamageTaken(float damage)
    {
        if (canTakeDamage && !isAlreadyDead)
        {
            animator.SetTrigger("Hurt");
            currentHealthPoint -= damage;

            StartCoroutine(RecoveryCooldown());
        }
    }

    private IEnumerator RecoveryCooldown()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(recoveryTime);
        canTakeDamage = true;
    }
}
