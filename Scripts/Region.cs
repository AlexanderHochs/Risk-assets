using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AssociatedCountriesRefs
{
    GameObject[] countryRefs;
}

[System.Serializable]
public class Region {

    public int reinforcementBonus;
    public string regionName; // for in editor view
    public GameObject GameplayManager;
    
    public List<Country> countries = new List<Country>();

    public void Initialize()
    {
        foreach (Country country in countries)
        {
            Country newCountry;
            newCountry = new Country(country.nextStateRef, false, GameplayManager);
            newCountry.setNextStateCountry(country);
            country.setNextStateCountry(newCountry);

            country.gameObjectRef.GetComponent<CountryUnityInterface>().InitializeCountry(country);
            newCountry.gameObjectRef.GetComponent<CountryUnityInterface>().InitializeCountry(newCountry);
        }
    }

    public int getReinforcementBonus()
    {
        return reinforcementBonus;
    }

    //public bool playerOwnRegion(Player player)
    //{
    //    for (Country in Countries)
    //}

}
