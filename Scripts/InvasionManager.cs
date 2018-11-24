using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InvasionManager : MonoBehaviour {
    public GameObject GameplayManager;

    public string BattleSceneName;

    public void SetAttackingCountry(Country attackingCountry)
    {
        InvasionData.SetAttackingCountry(attackingCountry);
    }

    public void SetDefendingCountry(Country defendingCountry)
    {
        InvasionData.SetDefendingCountry(defendingCountry);
    }

    public void CommenceBattle()
    {
        InvasionData.gameplayManager = GameplayManager;
        SceneManager.LoadScene(BattleSceneName, LoadSceneMode.Additive);
    }

    public void LoadTransitionDialog()
    {
        GameplayManager.GetComponent<GameplayManager>().StartTransitionDialog();
    }

}
