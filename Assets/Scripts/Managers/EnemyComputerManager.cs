using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyComputerManager : MonoBehaviour
{
    #region Singleton Code
    // A public reference to this script
    public static EnemyComputerManager instance = null;

    // Awake is called even before start
    private void Awake()
    {
        // If the reference for this script is null, assign it this script
        if(instance == null)
            instance = this;
        // If the reference is to something else (it already exists)
        // than this is not needed, thus destroy it
        else if(instance != this)
            Destroy(gameObject);
    }
    #endregion

    public Team computerTeam;
    private Spawner computerSpawner;

    private int mostExpensiveUnit;
    [SerializeField]
    private float changeLaneMinTime, changeLaneMaxTime;
    private float timeToChangeLane, currentChangeLaneTimer;

    // Start is called before the first frame update
    void Start()
    {
        computerTeam = Team.RightTeam;
        computerSpawner = GameManager.instance.GetTeamSpawner(computerTeam);

        mostExpensiveUnit = 0;
        foreach(GameObject spawnPrefab in computerSpawner.UnitPrefabs)
		{
            int spawnCost = spawnPrefab.GetComponent<Unit>().SpawnCost;
            if(spawnCost > mostExpensiveUnit)
                mostExpensiveUnit = spawnCost;
		}

        currentChangeLaneTimer = 0f;
        SetNewChangeLaneTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.CurrentMenuState == MenuState.Game)
        {
            int currentGold = GameManager.instance.GetTeamGold(computerTeam);
            if(currentGold >= mostExpensiveUnit)
                SpawnRandomUnit();
        }
    }

	private void FixedUpdate()
	{
        if(GameManager.instance.CurrentMenuState == MenuState.Game)
        {
            currentChangeLaneTimer += Time.deltaTime;
            if(currentChangeLaneTimer >= timeToChangeLane)
                ChangeToRandomSpawningLane();
        }
    }

	private void SpawnRandomUnit()
	{
        int randomUnitIndex = Random.Range(0, computerSpawner.UnitPrefabs.Count);
        computerSpawner.Spawn(randomUnitIndex);
    }

    private void SetNewChangeLaneTimer()
	{
        timeToChangeLane = Random.Range(changeLaneMinTime, changeLaneMaxTime);
    }

	private void ChangeToRandomSpawningLane()
    {
        currentChangeLaneTimer = 0f;
        computerSpawner.ChangeSpawnLane(Random.Range(-1, 2));
        SetNewChangeLaneTimer();
    }
}
