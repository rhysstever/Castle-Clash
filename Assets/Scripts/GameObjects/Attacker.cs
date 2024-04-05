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

        this.Move();

        target = FindTarget();
        if(CanAttack())
            Attack();
    }

	internal override bool CanMove()
    {
        return base.CanMove()
            && target == null;
	}

	internal override void Move()
	{
        // Do Not call base.Move()! The Attack moves differently

        if(CanMove())
            rb.velocity = new Vector2(moveDirection * moveSpeed * Time.deltaTime, 0);
        else
            rb.velocity = Vector2.zero;
    }

    private GameObject FindTarget()
	{
        Vector2 direction = GetTargetDirection(team);
        LayerMask enemyLayerMask = GetLayerMask(team);
        LayerMask onlyEnemyLayerMask = 1 << enemyLayerMask;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, range, onlyEnemyLayerMask);

        if(hit.collider != null)
            return hit.collider.gameObject;
        else
            return null;
    }

    private bool CanAttack()
    {
        if(GameManager.instance.CurrentMenuState == MenuState.Game)
            attackTimer += Time.deltaTime;

        if(attackTimer < attackSpeed)
            return false;

        return target != null;
    }

    private void Attack()
    {
        Debug.Log(target.gameObject.name + " hit!");
        target.GetComponent<Targetable>().TakeDamage(damage);
        attackTimer = 0f;
    }

    private Vector2 GetTargetDirection(Team team)
	{
        if(team == Team.LeftTeam)
            return Vector2.right;
        else
            return Vector2.left;
	}

    private LayerMask GetLayerMask(Team team)
    {
        if(team == Team.LeftTeam)
            return LayerMask.NameToLayer("RightTeam");
        else
            return LayerMask.NameToLayer("LeftTeam");
    }
}
