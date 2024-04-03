using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : Unit
{
    [SerializeField]
    internal float damage, range, attackSpeed, attackTimer, projectileSpeed;
    [SerializeField]
    internal GameObject target;

    // Start is called before the first frame update
    internal override void Start()
    {
        base.Start();
        attackTimer = attackSpeed;  // Can attack right away
        target = null;
    }

    // Update is called once per frame
    internal override void Update()
    {
        base.Update();
    }

    internal override void FixedUpdate()
    {
        base.FixedUpdate();

        // Move if there is no target
        if(target == null)
            Move();

        // Attacking
        Target();
        attackTimer += Time.deltaTime;
        if(CanAttack())
            Attack();
    }

    internal override void Move()
	{
        // Do Not call base.Move()! The Attack moves differently
        float currentMoveSpeed = moveSpeed;
        if(GameManager.instance.CurrentMenuState != MenuState.Game)
        {
            currentMoveSpeed = 0f;
        }

        rb.velocity = new Vector2(moveDirection * currentMoveSpeed * Time.deltaTime, 0);
    }

    private void Target()
	{
        Vector2 direction = Vector2.right;
        LayerMask enemyLayerMask = LayerMask.NameToLayer("RightTeam");
        enemyLayerMask = 1 << enemyLayerMask;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, range, enemyLayerMask);

        if(hit.collider != null)
        {
            target = hit.collider.gameObject;
        }
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
        Debug.Log(target.gameObject.name + " hit!");
        target.GetComponent<Targetable>().TakeDamage(damage);
        attackTimer = 0f;
    }
}
