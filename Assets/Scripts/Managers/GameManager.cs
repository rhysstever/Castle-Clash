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
    private Stack<MenuState> menuStates;

    // Start is called before the first frame update
    void Start()
    {
        menuStates = new Stack<MenuState>();
        ChangeMenuState(MenuState.MainMenu);

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
        switch(newMenuState)
        {
            case MenuState.MainMenu:
                menuStates.Clear();
                Reset();
                break;
            case MenuState.Controls:
                break;
            case MenuState.Game:
                if(currentMenuState == MenuState.Pause)
				{
                    menuStates.Pop();
				}
                else
				{
                    leftSpawner.SpawnFirstBuilder();
                    rightSpawner.SpawnFirstBuilder();
                }
                break;
            case MenuState.Pause:
                break;
            case MenuState.GameEnd:
                break;
        }

        menuStates.Push(newMenuState);
        currentMenuState = menuStates.Peek();
        UIManager.instance.ChangeUIState(newMenuState);
	}

    public void PreviousMenuState()
	{
        menuStates.Pop();
        ChangeMenuState(menuStates.Peek());
    }

	public void Reset()
    {
        leftTeamGold = 0;
        rightTeamGold = 0;

        leftSpawner.Reset(baseMaxHealth);
        rightSpawner.Reset(baseMaxHealth);
        leftMine.Reset(baseMaxHealth);
        rightMine.Reset(baseMaxHealth);

        UpdateBaseHealth();
        UIManager.instance.UpdateTeamGold(
            GetTeamGold(Team.LeftTeam),
            GetTeamGold(Team.RightTeam)
        );
    }

	public void UpdateBaseHealth()
	{
        float leftBaseHpPercentage = leftSpawner.health / baseMaxHealth;
        float rightBaseHpPercentage = rightSpawner.health / baseMaxHealth;
        UIManager.instance.UpdateBaseHealthUI(leftBaseHpPercentage, rightBaseHpPercentage);
        if(leftSpawner.IsDestroyed)
        {
            leftMine.TakeDamage(1000);
            ChangeMenuState(MenuState.GameEnd);
        } else if(rightSpawner.IsDestroyed)
        {
            rightMine.TakeDamage(1000);
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
		{
            leftTeamGold -= amount;
            leftMine.DespawnGold((int)amount);
        }
        else
		{
            rightTeamGold -= amount;
            rightMine.DespawnGold((int)amount);
        }

        UIManager.instance.UpdateTeamGold(
            GetTeamGold(Team.LeftTeam), 
            GetTeamGold(Team.RightTeam)
        );
    }
}
