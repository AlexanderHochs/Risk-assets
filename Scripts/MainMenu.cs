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
    private Color[] colors = new Color[4];

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
        colors[0] = Color.cyan;
        colors[1] = Color.red;
        colors[2] = Color.green;
        colors[3] = Color.magenta;
        LoadInData();

        players.Add(createPlayer(0));
        // add the second player on default
        players.Add(createPlayer(1));
    }

    private Player createPlayer(int index)
    {
        Player newPlayer = new Player();
        newPlayer.numberOfBattleBattalions = 0;
        newPlayer.numberOfBattalionsToDeploy = 3;
        newPlayer.SetColor(colors[index]);

        return newPlayer;
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
                players.Add(createPlayer(i));

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
