using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Singleton Code
    // A public reference to this script
    public static UIManager instance = null;

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
    private Canvas canvas;
    // UI Parent Objects
    [SerializeField]
    private GameObject mainMenuUIParent, controlsUIParent, gameUIParent, pauseUIParent, gameEndUIParent;
    // Main Menu Elements
    [SerializeField]
    private Button playButton, controlsButton, quitGameButton;
    // Controls Elements
    [SerializeField]
    private GameObject controlsMenu1, controlsMenu2;
    [SerializeField]
    private Button controlsNextButton, controlsPreviousMenuButton;
    // Game Top UI Elements
    [SerializeField]
    private Button settingsButton;
    [SerializeField]
    private GameObject leftTeamHealthBarUI, rightTeamHealthBarUI;
    [SerializeField]
    private TMP_Text leftTeamGoldText, rightTeamGoldText;
    // Pause Menu Elements
    [SerializeField]
    private Button resumeGameButton, pauseToControlsButton, pauseToMainMenuButton;
    // Game End Elements
    [SerializeField]
    private TMP_Text gameEndTitleText;
    [SerializeField]
    private Button gameEndToMainMenuButton;

    // Start is called before the first frame update
    void Start()
    {
        SetupButtons();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha5))
            GameManager.instance.ChangeMenuState(MenuState.Pause);
    }

    private void SetupButtons()
    {
        playButton.onClick.AddListener(() => GameManager.instance.ChangeMenuState(MenuState.Game));
        controlsButton.onClick.AddListener(() => GameManager.instance.ChangeMenuState(MenuState.Controls));
        quitGameButton.onClick.AddListener(() => Application.Quit());
        controlsNextButton.onClick.AddListener(() => IncrementControlsMenu());
        controlsPreviousMenuButton.onClick.AddListener(() => GameManager.instance.PreviousMenuState());
        settingsButton.onClick.AddListener(() => GameManager.instance.ChangeMenuState(MenuState.Pause));
        resumeGameButton.onClick.AddListener(() => GameManager.instance.ChangeMenuState(MenuState.Game));
        pauseToControlsButton.onClick.AddListener(() => GameManager.instance.ChangeMenuState(MenuState.Controls));
        pauseToMainMenuButton.onClick.AddListener(() => GameManager.instance.ChangeMenuState(MenuState.MainMenu));
        gameEndToMainMenuButton.onClick.AddListener(() => GameManager.instance.ChangeMenuState(MenuState.MainMenu));
    }

    public void ChangeUIState(MenuState newMenuState)
	{
        for (int i = 0; i < canvas.transform.childCount; i++)
		{
            canvas.transform.GetChild(i).gameObject.SetActive(false);
		}

        switch(newMenuState)
        {
            case MenuState.MainMenu:
                mainMenuUIParent.SetActive(true);
                break;
            case MenuState.Controls:
                controlsUIParent.SetActive(true);
                controlsMenu1.SetActive(true);
                controlsMenu2.SetActive(false);
                break;
            case MenuState.Game:
                gameUIParent.SetActive(true);
                break;
            case MenuState.Pause:
                pauseUIParent.SetActive(true);
                break;
            case MenuState.GameEnd:
                gameEndUIParent.SetActive(true);
                if(GameManager.instance.GetTeamSpawner(Team.LeftTeam).IsDestroyed)
                    gameEndTitleText.text = "Goblins \nWin!";
                else if(GameManager.instance.GetTeamSpawner(Team.RightTeam).IsDestroyed)
                    gameEndTitleText.text = "Knights \nWin!";
                break;
        }
	}

    private void IncrementControlsMenu()
	{
        controlsMenu1.SetActive(false);
        controlsMenu2.SetActive(true);
	}

    public void UpdateBaseHealthUI(float leftTeamBaseHpPercentage, float rightTeamBaseHpPercentage)
	{
        leftTeamHealthBarUI.transform.localScale = new Vector2(
            leftTeamBaseHpPercentage,
            leftTeamHealthBarUI.transform.localScale.y);
        rightTeamHealthBarUI.transform.localScale = new Vector2(
            rightTeamBaseHpPercentage,
            rightTeamHealthBarUI.transform.localScale.y);
    }

	public void UpdateTeamGold(int leftTeamGoldAmount, int rightTeamGoldAmount)
	{
        if(leftTeamGoldAmount > 99)
			leftTeamGoldText.text = "99+";
        else 
            leftTeamGoldText.text = leftTeamGoldAmount.ToString();

        if(rightTeamGoldAmount > 99)
            rightTeamGoldText.text = "99+";
        else
            rightTeamGoldText.text = rightTeamGoldAmount.ToString();
    }
}
