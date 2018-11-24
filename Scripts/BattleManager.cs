using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour {

    private class Dice
    {
        bool enabled;
        GameObject reference;

        public Dice()
        {
            enabled = true;
            reference = null;
        }

        public Dice(GameObject reference, bool enabled)
        {
            this.reference = reference;
            enabled = true;
        }

        public void SetDiceEnability(bool enabled)
        {
            this.enabled = enabled;
            reference.SetActive(enabled);
        }
    }

    public GameObject[] attackDice;
    public GameObject[] defenseDice;

    public bool evalAttack = false;
    public Player[] players = new Player[2];//0 is the attacker and 1 is the defender
    public Text attackBattalionsLabel;
    public Text defenseBattalionsLabel;

    private Dice[] attackDies = new Dice[3];
    private Dice[] defenseDies = new Dice[2];

	void Start () {
        InitializeDies();
        InitializeAttackingPlayer(InvasionData.GetAttackingCountry().GetNumberOfBattalionsOccupying(),
            InvasionData.GetAttackingCountry().GetOwner());
        InitializeDefendingPlayer(InvasionData.GetDefendingCountry().GetNumberOfBattalionsOccupying(),
            InvasionData.GetDefendingCountry().GetOwner());
        UpdateDies(players[0], players[1]);
        UpdateTextFields();
    }
	
	void Update () {
        if (evalAttack && !AnyDiceRolling())
        {
            Battle(players[0], players[1]);
            UpdateDies(players[0], players[1]);
            UpdateTextFields();
            evalAttack = false;
        }
    }

    private bool AnyDiceRolling()
    {
        bool AnyDiceRolling = false;
        foreach (GameObject dice in attackDice)
        {
            if (dice.activeInHierarchy)
            {
                if (dice.GetComponent<DiceRoll>().IsDiceRolling())
                {
                    AnyDiceRolling = true;
                }
            }
        }
        foreach (GameObject dice in defenseDice)
        {
            if (dice.GetComponent<DiceRoll>().IsDiceRolling())
            {
                AnyDiceRolling = true;
            }
        }
        return AnyDiceRolling;
    }

    private void Battle(Player Attacker, Player Defender)
    {
        bool[] won;

        won = AttackSuccesfulEvaluator(TwoHighestDice(defenseDice), TwoHighestDice(attackDice));
        int i = 0;
        foreach( bool signal in won)
        {
            if(!defenseDice[i].activeInHierarchy || !attackDice[i].activeInHierarchy)
            {
                Debug.Log("no dice to compare");
                continue;
            }
            if (signal)
            {
                Defender.numberOfBattleBattalions = (Defender.numberOfBattleBattalions - 1);
                Debug.Log("AttackSuccess");
            }
            else
            {
                Debug.Log("AttackFailed");
                Attacker.numberOfBattleBattalions = (Attacker.numberOfBattleBattalions - 1);
            }
            Debug.Log("Attacker Baqttalions left:" + Attacker.numberOfBattleBattalions + " Defender Battalions left:" + Defender.numberOfBattleBattalions);
            i++;
        }
        if(Attacker.numberOfBattleBattalions == 0 || Defender.numberOfBattleBattalions == 0)
        {
            if (Defender.numberOfBattleBattalions == 0)
            {
                GameData.players[GameData.players.IndexOf(players[1])].RemoveCountry(InvasionData.GetDefendingCountry());
                GameData.players[GameData.players.IndexOf(players[0])].AddCountry(InvasionData.GetDefendingCountry());
            }
            EndBattle();
        }
    }

    //helper functions
    int[] TwoHighestDice(GameObject[] dies)
    {
        int[] highestDies = {0,0};
        
        foreach (GameObject dice in dies)
        {
            if (!dice.activeInHierarchy)
            {
                Debug.Log("dice with value " + dice.GetComponent<DiceRoll>().GetDiceRoll() + " not active");
                continue;
            }
            if (dice.GetComponent<DiceRoll>().GetDiceRoll() > highestDies[0])
            {
                highestDies[1] = highestDies[0];
                highestDies[0] = dice.GetComponent<DiceRoll>().GetDiceRoll();
            }
            else if (dice.GetComponent<DiceRoll>().GetDiceRoll() > highestDies[1])
            {
                highestDies[1] = dice.GetComponent<DiceRoll>().GetDiceRoll();
            }
        }
        return highestDies;
    }

    bool[] AttackSuccesfulEvaluator(int[] highestDefenseDies, int[] highestAttackDies)
    {
        bool[] won = { false, false };
        for (int i = 0; i < 2; i++)
        {
            if (highestDefenseDies[i] >= highestAttackDies[i])
            {
                won[i] = false;
            }
            else
            {
                won[i] = true;
            }
        }

        return won;
    }

    public void InitializeAttackingPlayer(int numberOfBattalions, Player owner)
    {
        // TODO: take data from static script
        players[0] = owner;
        players[0].numberOfBattleBattalions = numberOfBattalions;
    }

    public void InitializeDefendingPlayer(int numberOfBattalions, Player owner)
    {
        // TODO: take data from static script
        players[1] = owner;
        players[1].numberOfBattleBattalions = numberOfBattalions;
    }

    private void InitializeDies()
    {
        for(int i = 0; i < 2; i++)
        {
            defenseDies[i] = new Dice(defenseDice[i], true);
            attackDies[i] = new Dice(attackDice[i], true);
        }
        //explicitly declared to save on resources
        attackDies[2] = new Dice(attackDice[2], true);
    }

    private void UpdateDies(Player attacker, Player defender)
    {
        if(defender.numberOfBattleBattalions < 2)
        {
            defenseDies[1].SetDiceEnability(false);
            if(defender.numberOfBattleBattalions < 1)
            {
                defenseDies[0].SetDiceEnability(false);
            }
        }

        if (attacker.numberOfBattleBattalions < 3)
        {
            attackDies[2].SetDiceEnability(false);
            if (attacker.numberOfBattleBattalions < 2)
            {
                attackDies[1].SetDiceEnability(false);
                if (attacker.numberOfBattleBattalions < 1)
                {
                    attackDies[0].SetDiceEnability(false);
                }
            }
        }
    }

    //UI methods
    public void RollDice()
    {
        startDiceRollVisual();
        evalAttack = true;
    }

    private void UpdateTextFields()
    {
        attackBattalionsLabel.text = players[0].numberOfBattleBattalions.ToString();
        defenseBattalionsLabel.text = players[1].numberOfBattleBattalions.ToString();
    }

    private void startDiceRollVisual()
    {
        foreach (GameObject dice in attackDice)
        {
            if (dice.activeInHierarchy)
            {
                dice.GetComponent<DiceRoll>().RollDice();
            }
        }
        foreach (GameObject dice in defenseDice)
        {
            if (dice.activeInHierarchy)
            {
                dice.GetComponent<DiceRoll>().RollDice();
            }
        }
    }


    public void EndBattle()
    {
        InvasionData.GetAttackingCountry().SetNumberOfBattalionsOccupying(players[0].numberOfBattleBattalions);
        InvasionData.GetDefendingCountry().SetNumberOfBattalionsOccupying(players[1].numberOfBattleBattalions);
        InvasionData.GetAttackingCountry().GetNextStateCountry().SwitchCountryView();
        InvasionData.GetDefendingCountry().GetNextStateCountry().SwitchCountryView();
        SceneManager.UnloadSceneAsync("Battle");
    }
}
