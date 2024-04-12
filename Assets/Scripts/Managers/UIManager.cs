using System.Collections;
using System.Collections.Generic;
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
    // Top Game UI Elements
    [SerializeField]
    private Button settingsButton;
    [SerializeField]
    private GameObject leftTeamHealthBarUI, rightTeamHealthBarUI;

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
        settingsButton.onClick.AddListener(() => GameManager.instance.ChangeMenuState(MenuState.Pause));
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
                break;
            case MenuState.Game:
                gameUIParent.SetActive(true);
                break;
            case MenuState.Pause:
                pauseUIParent.SetActive(true);
                break;
            case MenuState.GameEnd:
                gameEndUIParent.SetActive(true);
                break;
        }
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
}
