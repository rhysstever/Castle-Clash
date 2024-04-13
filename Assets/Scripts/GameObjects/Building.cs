using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Targetable
{
    [SerializeField]
    private Sprite builtSprite, destroyedSprite;
    protected bool isDestroyed;

    private SpriteRenderer spriteRenderer;

    public bool IsDestroyed { get { return isDestroyed; } }

    // Start is called before the first frame update
    internal override void Start()
    {
        base.Start();
        isDestroyed = false;
        GetComponent<SpriteRenderer>().sprite = builtSprite;
    }

    // Update is called once per frame
    internal override void Update()
    {
        base.Update();
    }

	public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if(health <= 0f)
        {
            isDestroyed = true;
            GetComponent<SpriteRenderer>().sprite = destroyedSprite;
        }
    }

    public override void Reset(float health)
	{
        base.Reset(health);
        isDestroyed = false;
        GetComponent<SpriteRenderer>().sprite = builtSprite;
    }
}
