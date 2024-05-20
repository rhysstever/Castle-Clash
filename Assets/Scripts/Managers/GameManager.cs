using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MenuState
{
    MainMenu,
    Setup,
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
    private GameObject leftUnitParent, rightUnitParent, leftProjectileParent, rightProjectileParent;
    [SerializeField]
    private Spawner leftSpawner, rightSpawner;
    [SerializeField]
    private Producer leftMine, rightMine;
    [SerializeField]
    private int leftTeamGold, rightTeamGold;
    [SerializeField]
    private float baseMaxHealth;
    public float BaseMaxHealth { get { return baseMaxHealth; } }
    [SerializeField]
    private MenuState currentMenuState;
    public MenuState CurrentMenuState { get { return currentMenuState; } }
    private Stack<MenuState> menuStates;

    private bool isSinglePlayer;
    public bool IsSinglePlayer { get { return isSinglePlayer; } }

    // Start is called before the first frame update
    void Start()
    {
        menuStates = new Stack<MenuState>();
        isSinglePlayer = false;

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

    public void ChangeMenuState(MenuState newMenuState, bool isGoingBack = false)
	{
        switch(newMenuState)
        {
            case MenuState.MainMenu:
                if(!isGoingBack)
                {
                    menuStates.Clear();
                    ResetAll();
                }
                break;
            case MenuState.Setup:
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
                    leftSpawner.Spawn(2, true);
                    rightSpawner.Spawn(2, true);
                }
                break;
            case MenuState.Pause:
                break;
            case MenuState.GameEnd:
                ClearAllProjectiles();
                break;
        }

        if(!isGoingBack)
            menuStates.Push(newMenuState);

        currentMenuState = menuStates.Peek();
        UIManager.instance.ChangeUIState(newMenuState);
	}

    public void PreviousMenuState()
    {
        menuStates.Pop();
        ChangeMenuState(menuStates.Peek(), true);
    }

    public void SetupGame(int playerCount)
	{
        if(playerCount == 1)
		{
            isSinglePlayer = true;
            EnemyComputerManager.instance.computerTeam = Team.RightTeam;
		} else if(playerCount == 2)
        {
            isSinglePlayer = false;
        }
        ChangeMenuState(MenuState.Game);
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

    public GameObject GetTeamUnitParent(Team team)
	{
        if(team == Team.LeftTeam)
            return leftUnitParent;
        else
            return rightUnitParent;
    }

    public GameObject GetTeamProjectileParent(Team team)
    {
        if(team == Team.LeftTeam)
            return leftProjectileParent;
        else
            return rightProjectileParent;
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

    public int GetTeamGold(Team team)
    {
        if(team == Team.LeftTeam)
            return leftTeamGold;
        else
            return rightTeamGold;
    }

    public void AddGold(Team team, int amount)
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

    public void SpendGold(Team team, int amount)
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

	private void ResetAll()
    {
        ResetGold();
        ResetBuildings();
        ClearAllUnits();
        ClearAllProjectiles();
    }

	private void ResetGold()
    {
        leftTeamGold = 0;
        rightTeamGold = 0;
        UIManager.instance.UpdateTeamGold(
            GetTeamGold(Team.LeftTeam),
            GetTeamGold(Team.RightTeam)
        );
    }

    private void ResetBuildings()
    {
        leftSpawner.Reset(baseMaxHealth);
        rightSpawner.Reset(baseMaxHealth);
        leftMine.Reset(baseMaxHealth);
        rightMine.Reset(baseMaxHealth);
        UpdateBaseHealth();
    }

    private void ClearAllUnits()
    {
        // Reset all units
        for(int i = leftUnitParent.transform.childCount - 1; i >= 0; i--)
            Destroy(leftUnitParent.transform.GetChild(i).gameObject);
        for(int i = rightUnitParent.transform.childCount - 1; i >= 0; i--)
            Destroy(rightUnitParent.transform.GetChild(i).gameObject);
    }

    private void ClearAllProjectiles()
    {
        // Reset all projectiles
        for(int i = leftProjectileParent.transform.childCount - 1; i >= 0; i--)
            Destroy(leftProjectileParent.transform.GetChild(i).gameObject);
        for(int i = rightProjectileParent.transform.childCount - 1; i >= 0; i--)
            Destroy(rightProjectileParent.transform.GetChild(i).gameObject);
    }
}
