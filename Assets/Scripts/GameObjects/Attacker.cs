using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : Unit
{
    [SerializeField]
    internal float damage, range, attackSpeed, attackTimer, projectileSpeed;
    [SerializeField]
    internal GameObject target;

    private Animator animator;

    // Start is called before the first frame update
    internal override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        attackTimer = 0;
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

        Move();

        target = FindTarget();
        if(CanAttack())
        {
            attackTimer += Time.deltaTime;
            if(attackTimer >= attackSpeed)
                Attack();
        }
    }

	internal override bool CanMove()
    {
        bool canMove = base.CanMove() 
            && target == null;
        animator.SetBool("canMove", canMove);

        return canMove;
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
        Vector2 origin = new Vector2(
            gameObject.transform.position.x,
            gameObject.transform.position.y + 1.5f  // Offset needed to match up with actual gameObject
            );
        Vector2 direction = GetTargetDirection(team);
        LayerMask enemyLayerMask = GetLayerMask(team);
        LayerMask onlyEnemyLayerMask = 1 << enemyLayerMask;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, range, onlyEnemyLayerMask);
        //Debug.DrawRay(origin, direction * range);

        GameObject target = GetValidTarget(hit);

        animator.SetBool("isAttacking", target != null);
        return target;
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

    private GameObject GetValidTarget(RaycastHit2D hit)
	{
        if(hit.collider != null)
        {
            if(hit.collider.gameObject != null)
            {
                GameObject hitObject = hit.collider.gameObject;

                if(hitObject.GetComponent<Building>() != null)
                {
                    // If the collision is with a building, make sure it is not already destroyed
                    if(hitObject.GetComponent<Building>().IsDestroyed)
                        return null;
                }

                return hitObject;
            }
        }

        return null;
    }

    private bool CanAttack()
    {
        return GameManager.instance.CurrentMenuState == MenuState.Game
            && target != null;
    }

    private void Attack()
    {
        Debug.Log(target.gameObject.name + " hit by " + gameObject.name);
        target.GetComponent<Targetable>().TakeDamage(damage);
        attackTimer = 0f;
        animator.SetBool("isAttacking", false);
    }
}
