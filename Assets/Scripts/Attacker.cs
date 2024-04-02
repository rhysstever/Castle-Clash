using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    [SerializeField]
    private float health, damage, range, attackSpeed, attackTimer, projectileSpeed;
    [SerializeField]
    private GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        attackTimer = attackSpeed;  // Can attack right away
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        attackTimer += Time.deltaTime;
        if(CanAttack())
            Attack();
    }

    private bool CanAttack()
    {
        if(target == null)
            return false;

        if(attackTimer < attackSpeed)
            return false;

        return true;
    }

    private void Attack()
    {

    }
}
