using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : Building
{
    [SerializeField]
    private GameObject spawnPrefab, spawnObjectParent;
    [SerializeField]
    private GameObject spawnLocationPointer;
    [SerializeField]
    private KeyCode moveLaneUpInputKey, moveLaneDownInputKey, spawnInputKey;

    private int currentSpawnLane;
    private float spawnLocationPointerPosXOffset;
    private int spawnCount;

    // Start is called before the first frame update
    internal override void Start()
    {
        health = GameManager.instance.BaseMaxHealth;
        currentSpawnLane = 1;
        spawnLocationPointerPosXOffset = 2.75f;
        if(team == Team.RightTeam)
            spawnLocationPointerPosXOffset *= -1;
        spawnCount = 0;
        UpdateSpawnLocationPointer();
    }

    // Update is called once per frame
    internal override void Update()
    {
        ChangeSpawnLocation();

        if(Input.GetKeyDown(spawnInputKey) && CanSpawn())
            Spawn();
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

    private bool CanSpawn()
    {
        return GameManager.instance.CurrentMenuState == MenuState.Game
            && !isDestroyed;
    }

    private void Spawn()
    {
        GameObject newUnit = Instantiate(
            spawnPrefab, 
            spawnLocationToPos(), 
            Quaternion.identity, 
            spawnObjectParent.transform
            );
        newUnit.name = spawnPrefab.name + spawnCount;

        spawnCount++;
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
}
