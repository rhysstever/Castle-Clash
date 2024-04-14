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
    private Vector3 targetPos;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
            Vector2 moveDirection = targetPos - gameObject.transform.position;   
            rb.velocity = projectileSpeed * Time.deltaTime * moveDirection.normalized;
            
            if(Vector2.Distance(gameObject.transform.position, targetPos) < 0.05f)
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
            if(team == Team.LeftTeam)
                Debug.Log(gameObject.name + " hits " + collision.gameObject.name);

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

	public void SetInitialValues(Team team, float damage, Vector2 targetPos)
	{
        this.team = team;
        this.damage = damage;
		this.targetPos = targetPos;
	}
}
