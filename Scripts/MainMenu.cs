using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    // Unity Object Refs
    public Dropdown mapSelect;
    public Dropdown numOfPlayersSelect;

    private List<Player> players = new List<Player>();
    private string levelName;

    // Use this for initialization
    void Start () {
        InitializeGameOptions();
    }

    // Update is called once per frame
    void Update () {
		
	}

    void LoadInData()
    {
        LoadInMapData();
        LoadInPlayerData();
    }

    private void LoadInPlayerData()
    {
        //// TODO: load in player data
        //Player newLocalPlayer = new Player(0);
        //newLocalPlayer.numberOfBattleBattalions = 0;
        //newLocalPlayer.numberOfBattalionsToDeploy = 3;
        //players.Add(newLocalPlayer);
        //// add the second player on default
        //Player newPlayer = new Player(1);
        //newPlayer.numberOfBattleBattalions = 0;
        //newPlayer.numberOfBattalionsToDeploy = 3;
        //players.Add(newPlayer);
    }

    private void InitializeGameOptions()
    {
        LoadInData();
        Player newLocalPlayer = new Player();
        newLocalPlayer.numberOfBattleBattalions = 0;
        newLocalPlayer.numberOfBattalionsToDeploy = 3;
        players.Add(newLocalPlayer);
        // add the second player on default
        Player newPlayer = new Player();
        newPlayer.numberOfBattleBattalions = 0;
        newPlayer.numberOfBattalionsToDeploy = 3;
        players.Add(newPlayer);
    }

    private void LoadInMapData()
    {
        // TODO: load in map data
        levelName = "Base";
    }

    public void ChooseMap(Dropdown change)
    {
        // TODO: choose map from map data list
        // levelname = change.name etc.
        levelName = "Base";
    }

    public void IniitalizePlayers(Dropdown change)
    {
        int value = change.value + 2;
        if (value < players.Count)
        {
            for (int i = players.Count - 1; i > value; i--)
            {
                players.RemoveAt(i);
            }
        }
        else if (value > players.Count)
        {
            for (int i = players.Count; i < value; i++)
            {
                Player newPlayer = new Player();
                newPlayer.numberOfBattleBattalions = 0;
                newPlayer.numberOfBattalionsToDeploy = 3;
                players.Add(newPlayer);

            }
        }
    }

    public void OnStart()
    {
        GameData.players = players;
        SceneManager.LoadScene(levelName);
    }

    public void OnExit()
    {
        Application.Quit();
    }
}
