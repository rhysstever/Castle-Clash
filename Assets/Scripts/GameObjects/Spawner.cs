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
    private KeyCode moveLaneUpInput, moveLaneDownInput, spawnInput;

    private int currentSpawnLane;
    private float spawnLocationPointerPosXOffset;
    private int spawnCount;

    // Start is called before the first frame update
    internal override void Start()
    {
        currentSpawnLane = 0;
        spawnLocationPointerPosXOffset = 3;
        if(team == Team.RightTeam)
            spawnLocationPointerPosXOffset *= -1;
        spawnCount = 0;
    }

    // Update is called once per frame
    internal override void Update()
    {
        ChangeSpawnLocation();

        if(Input.GetKeyDown(spawnInput) && CanSpawn())
            Spawn();
    }

    /// <summary>
    /// Changes the selected spawn location (lane) with 'W' and 'S' keys
    /// </summary>
    private void ChangeSpawnLocation()
    {
        int spawnLocation = currentSpawnLane;
        if(Input.GetKeyDown(moveLaneUpInput))
        {
            spawnLocation--;
            currentSpawnLane = Mathf.Clamp(spawnLocation, 0, 2);
            UpdateSpawnLocationPointer();
        }
        else if(Input.GetKeyDown(moveLaneDownInput))
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
        float yPos = 0.5f - currentSpawnLane * 1.0f;
        Vector2 newPos = new Vector2(xPos, yPos);
        
        spawnLocationPointer.transform.localPosition = newPos;
    }

    private bool CanSpawn()
    {
        return true;
    }

    private void Spawn()
    {
        GameObject newUnit = Instantiate(
            spawnPrefab, 
            spawnLocationToPos(), 
            Quaternion.identity, 
            spawnObjectParent.transform
            );
        newUnit.name = "unit" + spawnCount;

        spawnCount++;
    }

    /// <summary>
    /// Uses the selected spawn lane to get the spawn location
    /// </summary>
    /// <returns>A Vector2 of where a unit will spawn</returns>
    private Vector2 spawnLocationToPos()
    {
        Vector2 pos = spawnLocationPointer.transform.position;
        pos.y -= 1.1f;
        return pos;
    }
}
