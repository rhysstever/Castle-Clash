using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private Dictionary<KeyCode, GameObject> unitPrefabs;
    public List<GameObject> UnitPrefabs { get { return spawnPrefabs; } }

    private int currentSpawnLane;
    private float spawnLocationPointerPosXOffset;

    // Start is called before the first frame update
    internal override void Start()
    {
        base.Start();
        health = GameManager.instance.BaseMaxHealth;

        unitPrefabs = new Dictionary<KeyCode, GameObject>();
        for(int i = 0; i < spawnKeys.Count; i++)
            unitPrefabs.Add(spawnKeys[i], spawnPrefabs[i]);

        spawnLocationPointerPosXOffset = 2.75f;
        if(team == Team.RightTeam)
            spawnLocationPointerPosXOffset *= -1;
        
        currentSpawnLane = 1;

        UpdateSpawnLocationPointer();
    }

    // Update is called once per frame
    internal override void Update()
    {
        base.Update();

        if(!GameManager.instance.IsSinglePlayer
            || (GameManager.instance.IsSinglePlayer
            && EnemyComputerManager.instance.computerTeam != team))
        {
            CheckChangeSpawnLaneInput();

            for(int i = 0; i < spawnKeys.Count; i++)
            {
                if(Input.GetKeyDown(spawnKeys[i])
                    && CanSpawn(spawnPrefabs[i]))
                    Spawn(spawnPrefabs[i]);
            }
        }
    }

    private void CheckChangeSpawnLaneInput()
    {
        if(Input.GetKeyDown(moveLaneUpInputKey))
            ChangeSpawnLane(-1);
        else if(Input.GetKeyDown(moveLaneDownInputKey))
            ChangeSpawnLane(1);
    }

    public void ChangeSpawnLane(int moveDirection)
    {
        moveDirection = Mathf.Clamp(moveDirection, -1, 1);

        int newSpawnLocation = currentSpawnLane + moveDirection;
        currentSpawnLane = Mathf.Clamp(newSpawnLocation, 0, 2);
        UpdateSpawnLocationPointer();
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

    public void Spawn(int indexOfUnit, bool isFree = false)
	{
        GameObject spawnPrefab = unitPrefabs.ElementAt(indexOfUnit).Value;
        Spawn(spawnPrefab, isFree);
	}

    private void Spawn(GameObject spawnPrefab, bool isFree = false)
    {
        if(!isFree)
            GameManager.instance.SpendGold(team, spawnPrefab.GetComponent<Unit>().SpawnCost);

        Vector2 spawnLocation = spawnLocationToPos();
        if(spawnPrefab.GetComponent<Worker>() != null)
		{
            spawnLocation = gameObject.transform.position;
            spawnLocation.y += 0.5f;
        }
        GameObject parent = GameManager.instance.GetTeamUnitParent(team);

        GameObject newUnit = Instantiate(
            spawnPrefab,
            spawnLocation,
            Quaternion.identity,
            parent.transform
        );
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

	public override void Reset(float health)
	{
		base.Reset(health);
        currentSpawnLane = 1;

        UpdateSpawnLocationPointer();
    }
}
