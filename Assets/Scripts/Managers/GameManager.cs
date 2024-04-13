using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MenuState
{
    MainMenu,
    Controls,
    Game,
    Pause,
    GameEnd
}

public class GameManager : MonoBehaviour
{
    #region Singleton Code
    // A public reference to this script
    public static GameManager instance = null;

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

    [SerializeField]
    private Spawner leftSpawner, rightSpawner;
    [SerializeField]
    private Producer leftMine, rightMine;
    [SerializeField]
    private float leftTeamGold, rightTeamGold;
    [SerializeField]
    private float baseMaxHealth;
    public float BaseMaxHealth { get { return baseMaxHealth; } }
    [SerializeField]
    private MenuState currentMenuState;
    public MenuState CurrentMenuState { get { return currentMenuState; } }

    // Start is called before the first frame update
    void Start()
    {
        ChangeMenuState(MenuState.Game);


        UIManager.instance.UpdateTeamGold(
            GetTeamGold(Team.LeftTeam),
            GetTeamGold(Team.RightTeam)
        );
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeMenuState(MenuState newMenuState)
	{
        currentMenuState = newMenuState;
        UIManager.instance.ChangeUIState(newMenuState);
	}

	public void UpdateBaseHealth()
	{
        float leftBaseHpPercentage = leftSpawner.health / baseMaxHealth;
        float rightBaseHpPercentage = rightSpawner.health / baseMaxHealth;
        UIManager.instance.UpdateBaseHealthUI(leftBaseHpPercentage, rightBaseHpPercentage);
        if(leftSpawner.IsDestroyed || rightSpawner.IsDestroyed)
        {
            ChangeMenuState(MenuState.GameEnd);
        }
    }

    public Spawner GetTeamSpawner(Team team)
	{
        if(team == Team.LeftTeam)
            return leftSpawner;
        else 
            return rightSpawner;
    }

    public Producer GetTeamMine(Team team)
    {
        if(team == Team.LeftTeam)
            return leftMine;
        else
            return rightMine;
    }

    public float GetTeamGold(Team team)
    {
        if(team == Team.LeftTeam)
            return leftTeamGold;
        else
            return rightTeamGold;
    }

    public void AddGold(Team team, float amount)
	{
        if(team == Team.LeftTeam)
		{
            leftTeamGold += amount;
            for(int i = 0; i < amount; i++)
                leftMine.SpawnResource();
		}
        else
		{
            rightTeamGold += amount;
            for(int i = 0; i < amount; i++)
                rightMine.SpawnResource();
        }

        UIManager.instance.UpdateTeamGold(
            GetTeamGold(Team.LeftTeam), 
            GetTeamGold(Team.RightTeam)
        );
    }

    public void SpendGold(Team team, float amount)
    {
        if(team == Team.LeftTeam)
            leftTeamGold -= amount;
        else
            rightTeamGold -= amount;

        UIManager.instance.UpdateTeamGold(
            GetTeamGold(Team.LeftTeam), 
            GetTeamGold(Team.RightTeam)
        );
    }
}
