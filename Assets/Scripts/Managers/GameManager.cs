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
    private Spawner spawner1, spawner2;
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
        float leftBaseHpPercentage = spawner1.health / baseMaxHealth;
        float rightBaseHpPercentage = spawner2.health / baseMaxHealth;
        UIManager.instance.UpdateBaseHealthUI(leftBaseHpPercentage, rightBaseHpPercentage);
        if(spawner1.IsDestroyed || spawner2.IsDestroyed)
        {
            ChangeMenuState(MenuState.GameEnd);
        }
    }
}
