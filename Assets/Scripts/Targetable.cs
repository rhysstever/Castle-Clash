using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team
{
    LeftTeam,
    RightTeam,
}

public class Targetable : MonoBehaviour
{
    [SerializeField]
    internal float health;
    [SerializeField]
    internal Team team;

    // Start is called before the first frame update
    internal virtual void Start()
    {

    }

    // Update is called once per frame
    internal virtual void Update()
    {

    }

    internal virtual void FixedUpdate()
    {

    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;
    }

    public virtual void Reset(float health)
	{
		this.health = health;
	}
}
