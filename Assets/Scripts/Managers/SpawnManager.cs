using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private int currentSpawnLocation;
    [SerializeField]
    private GameObject spawnLocationPointer;
    [SerializeField]
    private GameObject leftPlayerUnits;
    [SerializeField]
    private GameObject blueWarrior;

    // Start is called before the first frame update
    void Start()
    {
        currentSpawnLocation = 0;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeSpawnLocation();

        if(Input.GetKeyDown(KeyCode.Space) && CanSpawn())
            Spawn();
    }

    /// <summary>
    /// Changes the selected spawn location (lane) with 'W' and 'S' keys
    /// </summary>
    private void ChangeSpawnLocation()
	{
        int spawnLocation = currentSpawnLocation;
		if(Input.GetKeyDown(KeyCode.W))
		{
            spawnLocation--;
		} 
        else if(Input.GetKeyDown(KeyCode.S))
		{
            spawnLocation++;
		}
        currentSpawnLocation = Mathf.Clamp(spawnLocation, 0, 2);
        UpdateSpawnLocationPointer();
    }

    /// <summary>
    /// Updates the location of the spawn pointer based on the selected spawn lane
    /// </summary>
    private void UpdateSpawnLocationPointer()
    {
        Vector2 newPos = Vector2.zero;
        float leftTeamSpawnPosX = -6.5f;
        switch(currentSpawnLocation)
        {
            case 0:
                newPos = new Vector2(leftTeamSpawnPosX, 0.5f);
                break;
            case 1:
                newPos = new Vector2(leftTeamSpawnPosX, -0.5f);
                break;
            case 2:
                newPos = new Vector2(leftTeamSpawnPosX, -1.5f);
                break;
            default:
                newPos = Vector2.zero;
                break;
        }
        spawnLocationPointer.transform.position = newPos;
    }

    private bool CanSpawn()
	{
        return true;
	}

    private void Spawn()
	{
        GameObject newSpawn = Instantiate(blueWarrior, spawnLocationToPos(), Quaternion.identity, leftPlayerUnits.transform);
	}

    /// <summary>
    /// Uses the selected spawn lane to get the spawn location
    /// </summary>
    /// <returns>A Vector2 of where a unit will spawn</returns>
	private Vector2 spawnLocationToPos()
	{
        float leftTeamSpawnPosX = -6.5f;
        switch(currentSpawnLocation)
        {
            case 0:
                return new Vector2(leftTeamSpawnPosX, -0.3f);
            case 1:
                return new Vector2(leftTeamSpawnPosX, -1.3f);
            case 2:
                return new Vector2(leftTeamSpawnPosX, -2.3f);
            default:
                return Vector2.zero;
        }
	}
}
