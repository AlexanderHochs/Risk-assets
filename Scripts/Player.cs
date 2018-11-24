using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private string ID;
    private Color color;
    public int numberOfBattleBattalions { get; set; }
    public int numberOfBattalionsToDeploy { get; set; }
    private List<Country> countriesOwned { get; set; }
    public List<Region> regionsOwned { get; set; }

    private SortedDictionary<string, Country> CountriesOwnedMap;

    public Player()
    {
        RiskUtil riskUtil = new RiskUtil();
        ID = riskUtil.GenerateID();
        numberOfBattleBattalions = 0;
        numberOfBattalionsToDeploy = 0;
        countriesOwned = new List<Country>();
        regionsOwned = new List<Region>();
        CountriesOwnedMap = new SortedDictionary<string, Country>();
    }

    public string GetID()
    {
        return ID;
    }

    public Color GetColor()
    {
        return color;
    }

    public void SetColor(Color color)
    {
        this.color = color;
    }

    public void AddCountry(Country country)
    {
        countriesOwned.Add(country);
        CountriesOwnedMap.Add(country.GetID(), country);
    }

    public void RemoveCountry(Country country)
    {
        countriesOwned.Remove(country);
        CountriesOwnedMap.Remove(country.GetID());
    }

    public int GetCountryCount()
    {
        return countriesOwned.Count;
    }

    public bool CountryOwned(Country country)
    {
        return CountriesOwnedMap.ContainsKey(country.GetID());
    }

    public void CalculateDeploymentNumber()
    {
        numberOfBattalionsToDeploy = 3;
        int additionalCountriesNumber = countriesOwned.Count - 10;
        if ( additionalCountriesNumber > 0 )
        {
            int counter = additionalCountriesNumber;
            while( counter > 0 )
            {
                numberOfBattalionsToDeploy++;
                counter -= 3;
            }
        }

        foreach( Region region in regionsOwned )
        {
            numberOfBattalionsToDeploy += region.reinforcementBonus;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             
        }
    }

}
