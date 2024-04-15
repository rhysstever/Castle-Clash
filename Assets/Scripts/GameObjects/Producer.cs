using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Producer : Building
{
    [SerializeField]
    private GameObject producedResource;
    [SerializeField]
    private float produceRate;
    [SerializeField]
    private int produceAmountPerWorker;
    [SerializeField]
    private Vector2 minBounds, maxBounds;

    private int workerCount;
    private float produceTimer;

    // Start is called before the first frame update
    internal override void Start()
    {
        base.Start();
        workerCount = 0;
        produceTimer = 0f;
    }

    // Update is called once per frame
    internal override void Update()
    {
        base.Update();
    }

	internal override void FixedUpdate()
	{
		base.FixedUpdate();
        Produce();
	}

    private bool CanProduce()
	{
        if(GameManager.instance.CurrentMenuState != MenuState.Game
            || isDestroyed)
            return false;

        produceTimer += Time.deltaTime;

        return produceTimer >= produceRate;
	}

    private void Produce()
	{
        if(CanProduce())
		{
            int productionAmount = workerCount * produceAmountPerWorker;
            produceTimer = 0f;
            GameManager.instance.AddGold(team, productionAmount);
        }
	}

    public void AddWorker(Worker worker)
	{
        workerCount++;
	}

    public void SpawnResource()
	{
        float xDiff = maxBounds.x - minBounds.x;
        float yDiff = maxBounds.y - minBounds.y;
        Vector2 spawnPos = new Vector2(
			Random.value * xDiff - (xDiff / 2),
			Random.value * yDiff - (yDiff * 2.5f)
        );

        GameObject spawnedResource = Instantiate(
            producedResource,
            spawnPos,
            Quaternion.identity,
            gameObject.transform
        );
        spawnedResource.transform.localPosition = spawnPos;
    }

    public void DespawnGold(int amount)
	{
        for(int i = amount - 1; i >= 0; i--)
		{
            Destroy(gameObject.transform.GetChild(i).gameObject);
		}
	}

	public override void Reset(float health)
	{
		base.Reset(health);

        workerCount = 0;
        produceTimer = 0f;
        for(int i = gameObject.transform.childCount - 1; i >= 0; i--)
            Destroy(gameObject.transform.GetChild(i).gameObject);
    }
}
