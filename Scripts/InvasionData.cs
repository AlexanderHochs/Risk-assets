using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InvasionData{

    private static Country attackingCountry;
    private static Country defendingCountry;

    public static GameObject gameplayManager { get; set; }

    public static void SetAttackingCountry(Country country)
    {
        attackingCountry = country;
    }

    public static Country GetAttackingCountry()
    {
        return attackingCountry;
    }
    public static void SetDefendingCountry(Country country)
    {
        defendingCountry = country;
    }

    public static Country GetDefendingCountry()
    {
        return defendingCountry;
    }

}
