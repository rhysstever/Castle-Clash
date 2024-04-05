using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Targetable
{
    [SerializeField]
    internal float moveSpeed, moveDirection, spawnCost;
    [SerializeField]
    public Rigidbody2D rb;

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

    internal virtual bool CanMove()
	{
        return GameManager.instance.CurrentMenuState == MenuState.Game;
	}

	internal virtual void Move()
    {

    }
}
