using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    public Rigidbody2D rb;
    [SerializeField]
    private float projectileSpeed;

    private Team team;
    private float damage;
    [SerializeField]
    private Vector3 moveDirection;
    private float range;

    private Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void FixedUpdate()
	{
        Move();
    }

    private bool CanMove()
	{
        return GameManager.instance.CurrentMenuState == MenuState.Game;
	}

	private void Move()
	{
        if(CanMove())
		{
            Vector2 move = projectileSpeed * Time.deltaTime * moveDirection.normalized;
            rb.velocity = move;

            Vector2 totalMoveVec = transform.position - startingPos;
            float totalMove = Mathf.Abs(totalMoveVec.magnitude);
            if(totalMove > range)
                Destroy(gameObject);
        }
        else
		{
            rb.velocity = Vector2.zero;
		}
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject != null)
        {
            if(collision.gameObject.GetComponent<Targetable>() != null
                && collision.gameObject.GetComponent<Targetable>().team != team
                && (collision.gameObject.GetComponent<Building>() != null
                || collision.gameObject.GetComponent<Attacker>() != null))
            {
                collision.gameObject.GetComponent<Targetable>().TakeDamage(damage);
                Destroy(gameObject);
            }
        }
	}

	public void SetInitialValues(Team team, float damage, Vector2 moveDirection, float range)
	{
        this.team = team;
        this.damage = damage;
        this.moveDirection = moveDirection;
        this.range = range;
	}
}
