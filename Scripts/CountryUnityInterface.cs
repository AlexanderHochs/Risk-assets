using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountryUnityInterface : MonoBehaviour {

    public bool isInitialState;
    public GameObject gameplayManager;

    private Country thisCountry;

    void Start()
    {
        gameObject.SetActive(isInitialState);
    }

    void OnMouseDown()
    {
        Debug.Log("onMouseUp " + thisCountry.isInitialState);
        gameplayManager.GetComponent<GameplayManager>().SetCountryReference(thisCountry);
        gameplayManager.GetComponent<GameplayManager>().OnCountryClick(thisCountry.isInitialState);
    }

    public void InitializeCountry(Country country)
    {
        thisCountry = country;
        thisCountry.SetEnabled(true);
    }

}
