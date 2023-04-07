using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ButtonManagerScript : MonoBehaviour
{
    public ButtonManagerScript Instance; // singleton
    public Button createButton;
    public Button joinButton;
    public Button rulesButton;
    public Button backButton;
    public Button joinSpecificGameButton;
    public Button createSpecificGameButton;
    public TMP_Dropdown numberOfRounds;
    public TMP_InputField numberOfPlayers;
    public TextMeshProUGUI rulesOfTheGame;
    public TMP_InputField gameIdField;


    public static event Action<GameState> ButtonEvent;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        createButton.onClick.AddListener(CreateButtonClicked);
        joinButton.onClick.AddListener(JoinButtonClicked);
        rulesButton.onClick.AddListener(RulesButtonClicked);
        backButton.onClick.AddListener(BackButtonClicked);

    }
    private void changeMainMenuObjectsActivness(bool activity)
    {
        createButton.gameObject.SetActive(activity);
        joinButton.gameObject.SetActive(activity);
        rulesButton.gameObject.SetActive(activity);
    }

     private void changeJoinMenuObjectsActiveness(bool activity)
     {
        gameIdField.gameObject.SetActive(activity);
        backButton.gameObject.SetActive(activity);
        joinSpecificGameButton.gameObject.SetActive(activity);
     }
    private void changeCreateMenuObjectsActiveness(bool activity) 
    {
        backButton.gameObject.SetActive(activity);
        numberOfRounds.gameObject.SetActive(activity);
        numberOfPlayers.gameObject.SetActive(activity);
        createSpecificGameButton.gameObject.SetActive(activity);
    }
    private void changeRulesMenuObjectsActiveness(bool activity)
    {
        backButton.gameObject.SetActive(activity);
        rulesOfTheGame.gameObject.SetActive(activity);
    }


    private void BackButtonClicked() // every click on back button does this, only show the main menu buttons!!
    {
        Debug.Log("Back button clicked");
        changeMainMenuObjectsActivness(true);
        changeCreateMenuObjectsActiveness(false);
        changeJoinMenuObjectsActiveness(false);
        changeRulesMenuObjectsActiveness(false );
    }

    private void RulesButtonClicked()// every click on rules button does this
    {
        Debug.Log("Rules button clicked");
        changeMainMenuObjectsActivness(false);
        changeRulesMenuObjectsActiveness(true);
    }

    private void JoinButtonClicked()// every click on join button does this
    {
        Debug.Log("Join button clicked");
        changeMainMenuObjectsActivness(false);
        changeJoinMenuObjectsActiveness(true) ;
    }

    private void CreateButtonClicked() // every click on create button does this
    {
        Debug.Log("Create button clicked");
        changeMainMenuObjectsActivness(false);
        changeCreateMenuObjectsActiveness(true ); 
    }

    // Update is called once per frame
    void Update()
    {

    }
}
