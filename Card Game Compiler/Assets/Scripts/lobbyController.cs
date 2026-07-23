using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;

public class lobbyController : MonoBehaviour
{
    public GameObject UserObj;
    public Transform UserParent;
    public TMP_Text lobbyCodeText;
    public TMP_Text playerCountText;
    public TMP_Text gameSelectionText;
    public TMP_Text gameDescriptionText;
    public List<GameObject> userList;
    public GameObject setGameButton;
    public GameObject startGameButton;
    public int players;
    public bool isHost = false;
    public string currentUser;
    public TabNavigation tN;
    public void updateLobbyInfo(LobbyInfo lobby, string username)
    {
        user host = lobby.host;
        user[] users = lobby.players;
        string code = lobby.code;
        string game = lobby.game;
        currentUser = username;
        if(currentUser != host.getName())
        {
            isHost = false;
            setGameButton.SetActive(false);
            startGameButton.SetActive(false);
            tN.swapFields("lobbyUser");
        }
        userList.Clear();
        updatePlayerList(users);
        players = users.Length;
        lobbyCodeText.text = "Lobby Code - " + code;
        gameSelectionText.text = game;
        gameDescriptionText.text = lobby.gameDescription;
        playerCountText.text = "Current Players " + players + "/32";
        if(currentUser == host.getName())
        {
            updateToHost(users,game);
        }
    }

    public void updatePlayerList(user[] users)
    {
        foreach (Transform child in UserParent) 
        {
            Destroy(child.gameObject);
        }
        for(int i = 0; i < users.Length; i++)
        {
            float f = i*-0.5f;
            GameObject y = Instantiate(UserObj, UserParent.position + new Vector3(0,f,0), UserParent.rotation, UserParent);
            playerInfo PI = y.GetComponent<playerInfo>();
            userList.Add(y);
            PI.Init(users[i].getDisplay(),users[i].getName(),i == 0,isHost,users[i].color);
        }
    }

    public void updateToHost(user[] users,string game)
    {
        isHost = true;
        for(int i = 0; i < userList.Count; i++)
        {
            userList[i].GetComponent<playerInfo>().Init(users[i].getDisplay(),users[i].getName(),i == 0,isHost,users[i].color);
        }
        setGameButton.SetActive(true);
        if(game != "No Game Selected")
        {
            startGameButton.SetActive(true);
            tN.swapFields("lobbyHostGame");
        }
        else
        {
            startGameButton.SetActive(false);
            tN.swapFields("lobbyHost");
        }
    }
}
