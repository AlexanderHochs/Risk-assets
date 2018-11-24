using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Country
{

    // TODO: make the reference inside of the object instead of the object inside of the reference
    public GameObject gameObjectRef;

    private bool countryEnabled = false;
    private Player owner = new Player();
    private int numberOfBattalionsOccupying = 1;
    private Country nextState;
    private string ID;

    public Country myCountry;
    public bool isInitialState;
    public GameObject nextStateRef;
    public List<GameObject> associatedCountriesRefs;
    public GameObject GameplayManager;
    public int BattalionCount;

    public Country()
    {
        RiskUtil riskUtil = new RiskUtil();
        ID = riskUtil.GenerateID();
    }

    public Country(GameObject gameObjectRef, bool isInitialState, GameObject GameplayManager)
    {
        RiskUtil riskUtil = new RiskUtil();
        ID = riskUtil.GenerateID();
        this.gameObjectRef = gameObjectRef;
        this.isInitialState = isInitialState;
        this.GameplayManager = GameplayManager;
    }

    public string GetID()
    {
        return ID;
    }

    public void setNextStateCountry(Country country)
    {
        nextState = country;
    }

    public void SwitchCountryView()
    {
        nextState.gameObjectRef.SetActive(true);
        gameObjectRef.SetActive(false);
    }

    public void toggleTrigger(bool toggle)
    {
        gameObjectRef.GetComponent<PolygonCollider2D>().enabled = toggle;
    }

    public Country GetNextStateCountry()
    {
        return nextState;
    }

    public Player GetOwner()
    {
        return owner;
    }

    public void SetOwner(Player owner)
    {
        gameObjectRef.GetComponent<SpriteRenderer>().color = owner.GetColor();
        nextStateRef.GetComponent<SpriteRenderer>().color = owner.GetColor();
        this.owner = owner;
    }

    public int GetNumberOfBattalionsOccupying()
    {
        return numberOfBattalionsOccupying;
    }

    public void SetNumberOfBattalionsOccupying(int numberOfBattalionsOccupying)
    {
        this.numberOfBattalionsOccupying = numberOfBattalionsOccupying;
    }

    public void SetEnabled(bool countryEnabled)
    {
        this.countryEnabled = countryEnabled;
    }

    public bool IsEnabled()
    {
        return this.countryEnabled;
    }
}
