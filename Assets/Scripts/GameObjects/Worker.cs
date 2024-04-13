using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : Unit
{
    private Animator animator;
    public Producer destination;
    private float arrivalDistance;

    // Start is called before the first frame update
    internal override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        destination = GameManager.instance.GetTeamMine(team);
        arrivalDistance = 2.0f;
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
    }

    internal override bool CanMove()
    {
        bool canMove = base.CanMove()
            && destination != null;
        animator.SetBool("canMove", canMove);

        return canMove;
    }

    internal override void Move()
	{
        Vector2 moveDirection = destination.transform.position;
        moveDirection.y -= 2f;
        moveDirection -= new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        Vector2 move = moveSpeed * Time.deltaTime * moveDirection.normalized;
        if(CanMove())
		{
            rb.velocity = move;
            CheckArrivalAtDestination();
		}
        else
		{
            rb.velocity = Vector2.zero;
		}
    }

    private void CheckArrivalAtDestination()
	{
        if(destination != null)
		{
            float dist = Vector2.Distance(destination.transform.position, gameObject.transform.position);
            if(dist <= arrivalDistance)
            {
                destination.AddWorker(this);
                gameObject.SetActive(false);
            }
        }
	}
}
