using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{

    public float currentHealthPoint;
    public float MaxHealthPoint = 5f;

    private bool isAlreadyDead = false;

    public float attackDammage = 1f;
    private Animator animator;

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
        if (!isAlreadyDead)
        {
            animator.SetTrigger("Hurt");
            currentHealthPoint -= damage;
        }

    }
}
