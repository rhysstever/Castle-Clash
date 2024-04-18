using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Targetable
{
    [SerializeField]
    internal float moveSpeed, moveDirection;
    [SerializeField]
    internal int spawnCost;
    public int SpawnCost { get { return spawnCost; } }
    [SerializeField]
    internal Rigidbody2D rb;
    [SerializeField]
    private GameObject deathObject;

    // Start is called before the first frame update
    internal override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        if(team == Team.LeftTeam)
            moveDirection = 1.0f;
        else
            moveDirection = -1.0f;
    }

    // Update is called once per frame
    internal override void Update()
    {
        base.Update();
    }

    internal override void FixedUpdate()
    {
        base.FixedUpdate();
    }

	public override void TakeDamage(float damage)
	{
		base.TakeDamage(damage);
        if(health <= 0)
            Die();
	}

	internal virtual bool CanMove()
	{
        return GameManager.instance.CurrentMenuState == MenuState.Game;
	}

	internal virtual void Move()
    {

    }

    internal void Die()
	{
        GameObject deathObj = Instantiate(deathObject, gameObject.transform.position, Quaternion.identity, transform.parent);
        deathObj.name = gameObject.name + deathObject.name;
        if(team == Team.RightTeam)
            deathObj.GetComponent<SpriteRenderer>().flipX = true;
        Destroy(gameObject);
    }
}
