using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using System.Threading.Tasks;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instance { get; private set; }
    public bool lobbyActive = false;
    public Lobby hostLobby;
    public Lobby joinLobby;
    public float heartBeatTimer;
    public string playerName;
    public float lobbyUpdateTimer;

    private async void Start()
    {
        Instance = this;
        await UnityServices.InitializeAsync();


        AuthenticationService.Instance.SignedIn += () => {
            Debug.Log($"Signed in {AuthenticationService.Instance.PlayerId}");
        };

        AuthenticationService.Instance.ClearSessionToken();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        playerName = "PlayerName" + UnityEngine.Random.Range(10, 99);
        Debug.Log($"Player name: {playerName}");
    }


    public async Task CreateLobby(int maxPlayers, string _playerName)
    {
        try
        {
            string lobbyName = "TestLobby";
            CreateLobbyOptions options = new CreateLobbyOptions
            {
                IsPrivate = false,
                Player = GetPlayer(_playerName),
                Data = new Dictionary<string, DataObject>
                {
                    { "GameStarted", new DataObject(DataObject.VisibilityOptions.Public, "false") },
                    { "PlayersNum", new DataObject(DataObject.VisibilityOptions.Public, $"{maxPlayers}") },
                    { "StartGame_RelayCode", new DataObject(DataObject.VisibilityOptions.Member, "0") }
                }
            };
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);
            hostLobby = lobby;
            joinLobby = hostLobby;
            // OnJoinedLobby?.Invoke(this, new LobbyEventArgs { lobby = joinedLobby});

            Debug.Log($"Lobby was created: max players-{lobby.MaxPlayers}, Code-{lobby.LobbyCode}");
            PrintPlayers(hostLobby);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async Task StartGame()
    {
        try
        {
            Debug.Log("Entered to StartGame()");
            string relayCode = await RelayManager.Instance.CreateRelay();
            Lobby lobby = await Lobbies.Instance.UpdateLobbyAsync(joinLobby.Id, new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject> {
                { "StartGame_RelayCode", new DataObject(DataObject.VisibilityOptions.Member, relayCode) }
            }
            });
            joinLobby = lobby;
            Debug.Log("Exiting StartGame()");
        }
        catch (LobbyServiceException ex)
        {
            Debug.Log(ex);
        }
    }

    public async void ListLobbies()
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
            {
                Count = 25,
                Filters = new List<QueryFilter> {
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT)
                },
                Order = new List<QueryOrder> {
                    new QueryOrder(false, QueryOrder.FieldOptions.Created)
                }
            };
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();
            Debug.Log($"lobbies found - { queryResponse.Results.Count}");

            foreach (Lobby lobby in queryResponse.Results)
            {
                Debug.Log($"lobby name - {lobby.Name}, max players - {lobby.MaxPlayers}, length - {lobby.Data["Length"].Value}");
            }
        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void HandleLobbyHeartBeat()
    {
        if (hostLobby != null)
        {
            heartBeatTimer -= Time.deltaTime;
            if (heartBeatTimer < 0f)
            {
                float heartBeatTimerMax = 15;
                heartBeatTimer = heartBeatTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            }
        }
    }

    public async void HandleLobbyForUpdates()
    {
        if (joinLobby != null)
        {
            lobbyUpdateTimer -= Time.deltaTime;
            if (lobbyUpdateTimer < 0f)
            {
                float lobbyUpdateTimerMax = 1.1f;
                lobbyUpdateTimer = lobbyUpdateTimerMax;

                Lobby lobby = await LobbyService.Instance.GetLobbyAsync(joinLobby.Id);
                joinLobby = lobby;
                if (joinLobby.Data["StartGame_RelayCode"].Value != "0")
                {
                    Debug.Log($"value - {joinLobby.Data["StartGame_RelayCode"].Value}");
                    if (hostLobby == null)
                    {
                        Debug.Log("trying to join relay");
                        SceneManager.LoadScene("Game");
                        RelayManager.Instance.JoinRelay(joinLobby.Data["StartGame_RelayCode"].Value);
                        Debug.Log("joined relay");
                    }
                    joinLobby = null;
                }
            }
        }
    }

    public async Task JoinLobbyByCode(string lobbyCode, string _playerName)
    {
        try
        {
            JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions 
            { 
                Player = GetPlayer(_playerName)
            };

            Lobby lobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, joinLobbyByCodeOptions);
            joinLobby = lobby;

            Debug.Log($"Joined lobby with code {lobbyCode}");
            PrintPlayers(lobby);
        }
        catch (Exception)
        {
            Debug.Log("could not join lobby");
            throw;        
        }
    }

    public async void QuickJoinLobby() // works only if the lobby is not created in private (see CreateLobby function)
    {
        try
        {
            await LobbyService.Instance.QuickJoinLobbyAsync();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public void PrintPlayers(Lobby lobby)
    {
        Debug.Log($"players in lobby {lobby.Name}, Num of players {lobby.Data["PlayersNum"].Value}");

        foreach(Player player in lobby.Players)
        {
            Debug.Log($"player id: {player.Id}, player name: {player.Data["PlayerName"].Value}");
        }
    }
    public void PrintPlayers()
    {
        PrintPlayers(joinLobby);
    }

    public Player GetPlayer(string _playerName)
    {
        if (_playerName != "")
        {
            playerName = _playerName;
        }
        return new Player
        {
            Data = new Dictionary<string, PlayerDataObject>
            {
                { "PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName) }
            }
        };
    }

    public async void UpdatePlayerName(string newPlayerName)
    {
        try
        {
            playerName = newPlayerName;
            await LobbyService.Instance.UpdatePlayerAsync(joinLobby.Id, AuthenticationService.Instance.PlayerId, new UpdatePlayerOptions
            {
                Data = new Dictionary<string, PlayerDataObject> {
                 { "PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName) }
            }
            });
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void UpdateLobbyStartGameOption(string startGameString)
    {
        try
        {
            hostLobby = await LobbyService.Instance.UpdateLobbyAsync(joinLobby.Id, new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject>
                {
                 { "GameStarted", new DataObject(DataObject.VisibilityOptions.Public, startGameString) }
            }
            });
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void LeaveLobby()
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(joinLobby.Id, AuthenticationService.Instance.PlayerId);
            UpdateLobbyStartGameOption("false");
            joinLobby = null;
            hostLobby = null;
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    /*[Command]
    private async void KickPlayer() commented, should be here if we want to implement kick player mechanisem
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(joinLobby.Id, joinLobby.Players[1].Id);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }*/
    public async void DeleteLobby()
    {
        try
        {
            await LobbyService.Instance.DeleteLobbyAsync(joinLobby.Id);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    void Update()
    {
        HandleLobbyHeartBeat();
        HandleLobbyForUpdates();
    }
}
