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
    // (I think its at the very beginning of runtime)
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
    private GameObject base1, base2;
    private Spawner spawner1, spawner2;

    [SerializeField]
    private MenuState currentMenuState;
    public MenuState CurrentMenuState { get { return currentMenuState; } }

    // Start is called before the first frame update
    void Start()
    {
        spawner1 = base1.GetComponent<Spawner>();
        spawner2 = base2.GetComponent<Spawner>();
        ChangeMenuState(MenuState.Game);
    }

    // Update is called once per frame
    void Update()
    {
        if(spawner1.IsDestroyed || spawner2.IsDestroyed)
		{
            ChangeMenuState(MenuState.GameEnd);
		}
    }

    public void ChangeMenuState(MenuState newMenuState)
	{
        currentMenuState = newMenuState;
	}
}
