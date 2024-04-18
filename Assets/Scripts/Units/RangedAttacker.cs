using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttacker : Attacker
{
    [SerializeField]
    private Projectile projectile;

    // Start is called before the first frame update
    internal override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    internal override void Update()
    {
        base.Update();
    }

	internal override void Attack()
	{
		base.Attack();

        Vector2 projPos = new Vector2(
            gameObject.transform.position.x,
            gameObject.transform.position.y + 0.25f
            );
        Vector2 shotDirection = new Vector2(moveDirection, 0);

        GameObject parent = GameManager.instance.GetTeamProjectileParent(team);

        if(team == Team.LeftTeam)
            projPos.x += 0.4f;
        else
            projPos.x -= 0.4f;

        GameObject newShot = Instantiate(
            projectile.gameObject,
            projPos,
            Quaternion.identity,
            parent.transform
            );
        newShot.GetComponent<Projectile>().SetInitialValues(
            team, 
            damage,
            shotDirection,
            range + 0.25f
            );
	}
}
