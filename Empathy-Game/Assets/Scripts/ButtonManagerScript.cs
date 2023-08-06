using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Lobbies.Models;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class ButtonManagerScript : NetworkBehaviour
{
    public ButtonManagerScript Instance; // singleton
    [SerializeField] private Button createButton; // create button at the main menu
    [SerializeField] private Button joinButton; // join button at the main menu
    [SerializeField] private Button rulesButton; // rules button at the main menu
    [SerializeField] private Button backButton; // back button
    [SerializeField] private Button joinSpecificGameButton; // join specific game button, at the join menu
    [SerializeField] private Button createSpecificGameButton; // create specific game button, at the create menu
    [SerializeField] private Button startGameButton; // start game button, at the lobby
    [SerializeField] private GameObject Lobby; // static game lobby
    [SerializeField] private TMP_Dropdown numberOfRounds; // number of rounds in the game, at the create menu
    [SerializeField] private TMP_InputField numberOfPlayers; // number of players in the game, at the create menu
    [SerializeField] private TextMeshProUGUI rulesOfTheGame; // rules of the game, at the rules menu
    [SerializeField] private TMP_InputField gameIdField; // game id field, at the join menu
    [SerializeField] private TextMeshProUGUI listOfPlayers;




    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        createButton.onClick.AddListener(CreateButtonClicked);
        joinButton.onClick.AddListener(JoinButtonClicked);
        rulesButton.onClick.AddListener(RulesButtonClicked);
        backButton.onClick.AddListener(BackButtonClicked);
        createSpecificGameButton.onClick.AddListener(CreateSpecificGameButtonClicked);
        joinSpecificGameButton.onClick.AddListener(JoinSpecificGameButtonClicked);
        startGameButton.onClick.AddListener (StartGameButtonClicked);
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

    private async void JoinSpecificGameButtonClicked() // every click on join specific game button does this
    {
        Debug.Log("Join specific game button clicked");
        Debug.Log($"trying to join lobby with code {gameIdField.text}");
        try
        {
            await LobbyManager.Instance.JoinLobbyByCode(gameIdField.text);
            Debug.Log("did not throw exception");
        }
        catch (Exception)
        {
            Debug.Log($"can not use {gameIdField.text} to join");
            return;
        }
        changeJoinMenuObjectsActiveness(false);
        backButton.gameObject.SetActive(true); // back button is should be active at the lobby

        LobbyManager.Instance.lobbyActive = true;
        Lobby.SetActive(true);
    }

    private void CreateSpecificGameButtonClicked() // every click on create specific game button does this
    {
        try
        {
            int max_players = int.Parse(numberOfPlayers.text);
            if (max_players < 2 || max_players > 4) // only max 4 players for now
            {
                throw new Exception("Number of players must be between 2 and 4");
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            return;
        }

        Debug.Log("Create specific game button clicked");
        changeCreateMenuObjectsActiveness(false);
        backButton.gameObject.SetActive(true); // back button is should be active at the lobby

        LobbyManager.Instance.CreateLobby(int.Parse(numberOfPlayers.text), numberOfRounds.options[numberOfRounds.value].text);
        LobbyManager.Instance.lobbyActive = true;
        Lobby.SetActive(true);
    }

    private void StartGameButtonClicked()
    {
        Debug.Log("Start game button clicked");
        LobbyManager.Instance.UpdateLobbyStartGameOption("true");
        SceneManager.LoadScene("Game");
    }
    private void BackButtonClicked() // every click on back button does this, only show the main menu buttons!!
    {
        Debug.Log("Back button clicked");
        if (Lobby.activeSelf) // leaving the lobby
        {
            Debug.Log("Leaving lobby");
            LobbyManager.Instance.LeaveLobby();
        }
        changeMainMenuObjectsActivness(true);
        changeCreateMenuObjectsActiveness(false);
        changeJoinMenuObjectsActiveness(false);
        changeRulesMenuObjectsActiveness(false);
        Lobby.SetActive(false);
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
        changeJoinMenuObjectsActiveness(true);
    }

    private void CreateButtonClicked() // every click on create button does this
    {
        Debug.Log("Create button clicked");
        changeMainMenuObjectsActivness(false);
        changeCreateMenuObjectsActiveness(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (LobbyManager.Instance.lobbyActive && LobbyManager.Instance.joinLobby != null)
        {
            listOfPlayers.text = "";
            foreach (Player player in LobbyManager.Instance.joinLobby.Players)
            {
                listOfPlayers.text = listOfPlayers.text + player.Data["PlayerName"].Value + "\n";
            }
            if (LobbyManager.Instance.joinLobby.Data["GameStarted"].Value == "true")
            {
                SceneManager.LoadScene("Game");
            }
        }
        
    }
}
