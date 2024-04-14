using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : Building
{
    [SerializeField]
    private GameObject spawnLocationPointer;
    [SerializeField]
    private KeyCode moveLaneUpInputKey, moveLaneDownInputKey;
    [SerializeField]
    private List<KeyCode> spawnKeys;
    [SerializeField]
    private List<GameObject> spawnPrefabs;

    private int currentSpawnLane;
    private float spawnLocationPointerPosXOffset;
    private int attackerSpawnCount, workerSpawnCount;

    // Start is called before the first frame update
    internal override void Start()
    {
        base.Start();
        health = GameManager.instance.BaseMaxHealth;

        spawnLocationPointerPosXOffset = 2.75f;
        if(team == Team.RightTeam)
            spawnLocationPointerPosXOffset *= -1;
        
        currentSpawnLane = 1;
        attackerSpawnCount = 0;
        workerSpawnCount = 0;

        UpdateSpawnLocationPointer();
    }

    // Update is called once per frame
    internal override void Update()
    {
        base.Update();
        ChangeSpawnLocation();

        for(int i = 0; i < spawnKeys.Count; i++)
        {
            if(Input.GetKeyDown(spawnKeys[i]) 
                && CanSpawn(spawnPrefabs[i]))
                Spawn(spawnPrefabs[i]);
        }
    }

    /// <summary>
    /// Changes the selected spawn location (lane) with 'W' and 'S' keys
    /// </summary>
    private void ChangeSpawnLocation()
    {
        int spawnLocation = currentSpawnLane;
        if(Input.GetKeyDown(moveLaneUpInputKey))
        {
            spawnLocation--;
            currentSpawnLane = Mathf.Clamp(spawnLocation, 0, 2);
            UpdateSpawnLocationPointer();
        }
        else if(Input.GetKeyDown(moveLaneDownInputKey))
        {
            spawnLocation++;
            currentSpawnLane = Mathf.Clamp(spawnLocation, 0, 2);
            UpdateSpawnLocationPointer();
        }
    }

    /// <summary>
    /// Updates the location of the spawn pointer based on the selected spawn lane
    /// </summary>
    private void UpdateSpawnLocationPointer()
    {
        float xPos = spawnLocationPointerPosXOffset;
        float yPos = currentSpawnLane * -0.7f;
        Vector2 newPos = new Vector2(xPos, yPos);
        
        spawnLocationPointer.transform.localPosition = newPos;
    }

    private bool CanSpawn(GameObject spawnPrefab)
    {
        return GameManager.instance.CurrentMenuState == MenuState.Game
            && !isDestroyed
            && GameManager.instance.GetTeamGold(team) >= spawnPrefab.GetComponent<Unit>().SpawnCost;
    }

    private void Spawn(GameObject spawnPrefab, bool free = false)
    {
        if(!free)
            GameManager.instance.SpendGold(team, spawnPrefab.GetComponent<Unit>().SpawnCost);

        if(spawnPrefab.GetComponent<Worker>() != null)
		{
            Vector2 workerSpawnLocation = gameObject.transform.position;
            workerSpawnLocation.y += 0.5f;
            
            SpawnUnit(spawnPrefab, workerSpawnLocation, workerSpawnCount);
            workerSpawnCount++;
        }
        else
		{
            SpawnUnit(spawnPrefab, spawnLocationToPos(), attackerSpawnCount);
            attackerSpawnCount++;
        } 
    }

    private void SpawnUnit(GameObject spawnPrefab, Vector2 spawnPos, int currentCount) 
    {
        GameObject parent = GameManager.instance.GetTeamUnitParent(team);

        GameObject newUnit = Instantiate(
            spawnPrefab,
            spawnPos,
            Quaternion.identity,
            parent.transform
            );
        newUnit.name = spawnPrefab.name + currentCount;
    }

    public override void TakeDamage(float damage)
	{
		base.TakeDamage(damage);
        GameManager.instance.UpdateBaseHealth();
	}

	/// <summary>
	/// Uses the selected spawn lane to get the spawn location
	/// </summary>
	/// <returns>A Vector2 of where a unit will spawn</returns>
	private Vector2 spawnLocationToPos()
    {
        Vector2 pos = spawnLocationPointer.transform.position;
        if(team == Team.LeftTeam)
            pos.x += -2.0f;
        else
            pos.x += 2.0f;
        pos.y -= 1.1f;
        return pos;
    }

    public void SpawnFirstBuilder()
    {
        // Spawn a worker for free
        Spawn(spawnPrefabs[2], true);
    }

	public override void Reset(float health)
	{
		base.Reset(health);
        currentSpawnLane = 1;
        attackerSpawnCount = 0;
        workerSpawnCount = 0;

        UpdateSpawnLocationPointer();
    }
}
