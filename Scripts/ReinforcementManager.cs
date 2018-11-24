using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReinforcementManager : MonoBehaviour {

    //TODO: implement dialog where one can add battalions to a country by selected the country and have a visual as to which countries have deployments

    //TODO: implement dialog where one can add battalions from one country to another

    public GameObject gameplayManager;

    private Country countryToReinforce;
    private Country countryToTakeAway;
    private int amountToExchange = 0;

    //Redeployment dialog
    public GameObject redeploymentDialog;
    public Text countryToReinforceAmount;
    public Text countryToTakeAwayAmount;
    public Text input;

    //Deployment variables
    // if public then change to private after debugging
    private Player curPlayer;
    
    public int numberOfDeployments;
    public List<Country> countriesToDeploy = new List<Country>();
    //

    public GameObject deploymentDialog;
    public Text numberOfDeploymentsLabel;

    void Start () {
        redeploymentDialog.SetActive(false);
        deploymentDialog.SetActive(false);
    }

    //TODO: decide where the dialogs should go
    //Redeployment dialog
    public void StartRedeploymentDialog()
    {
        int amount;

        curPlayer = gameObject.GetComponent<GameplayManager>().curPlayer;
        numberOfDeployments = curPlayer.numberOfBattalionsToDeploy;

        redeploymentDialog.SetActive(true);

        //disable the countries so you can't click them through the dialog
        countryToTakeAway.GetNextStateCountry().SetEnabled(false);

        countryToReinforce.GetNextStateCountry().SetEnabled(false);

        amount = countryToTakeAway.GetNumberOfBattalionsOccupying() - amountToExchange;
        countryToTakeAwayAmount.text = amount.ToString();

        amount = countryToReinforce.GetNumberOfBattalionsOccupying() + amountToExchange;
        countryToReinforceAmount.text = amount.ToString();
    }

    public void SetExchangeTexts()
    {
        int amount;
        int inputAmount;
        //TODO: add exception for nonInt strings
        if (System.Int32.TryParse(input.text, out inputAmount) && ((
            countryToTakeAway.GetNumberOfBattalionsOccupying() - amountToExchange) > 0))
        {
            Debug.Log(" input: " + inputAmount);
            SetAmountToExchange(inputAmount);
        }

        amount = countryToTakeAway.GetNumberOfBattalionsOccupying() - amountToExchange;
        countryToTakeAwayAmount.text = amount.ToString();

        amount = countryToReinforce.GetNumberOfBattalionsOccupying() + amountToExchange;
        countryToReinforceAmount.text = amount.ToString();
    }

    public void OnRedeploymentDialogSubmit()
    {
        BattalionDeployment(true);
        gameObject.GetComponent<GameplayManager>().StartTransitionDialog();
    }

    // end redeployment dialog
    //deployment
    public void StartDeploymentDialog()
    {
        curPlayer = gameObject.GetComponent<GameplayManager>().curPlayer;
        numberOfDeployments = curPlayer.numberOfBattalionsToDeploy;
        deploymentDialog.SetActive(true);
        SetNumberOfDeploymentsLabel();
    }
    public void SetNumberOfDeployments(int numberOfDeployments)
    {
        this.numberOfDeployments = numberOfDeployments;
    }

    public void SetNumberOfDeploymentsLabel()
    {
        numberOfDeploymentsLabel.text = (numberOfDeployments - countriesToDeploy.Count).ToString();
    }

    public void AddCountryToDeployList(Country country)
    {
        if (curPlayer.CountryOwned(country))
        {
            country.SetEnabled(false);
            country.GetNextStateCountry().gameObjectRef.SetActive(true);
            country.GetNextStateCountry().toggleTrigger(false);
            countriesToDeploy.Add(country);
            SetNumberOfDeploymentsLabel();

            if ((numberOfDeployments - countriesToDeploy.Count) < 1)
            {
                DoDeployments();
            }
        }
    }

    private void DoDeployments()
    {
        deploymentDialog.SetActive(false);
        for (int i = countriesToDeploy.Count - 1; i > -1; i--)
        {
            SetCountryToReinforce(countriesToDeploy[i]);
            BattalionDeployment(false);
            countriesToDeploy.RemoveAt(i);
            curPlayer.numberOfBattalionsToDeploy--;
        }
        if (gameObject.GetComponent<GameplayManager>().isInitial)
        {
            gameObject.GetComponent<GameplayManager>().InitializeGameCallback();
        }
        else
        {
            gameObject.GetComponent<GameplayManager>().StartTransitionDialog();
        }
    }
    //end deployment

    public void SetAmountToExchange(int amountToExchange)
    {
        this.amountToExchange = amountToExchange;
    }

    public void SetCountryToReinforce(Country countryToReinforce)
    {
        this.countryToReinforce = countryToReinforce;
    }

    public Country GetCountryToTakeAway()
    {
        return countryToTakeAway;
    }

    public void SetCountryToTakeAway(Country countryToTakeAway)
    {
        if(countryToTakeAway.GetNumberOfBattalionsOccupying() < 2)
        {
            Debug.Log("A country must have a minimum of 1 battalion in a country");
            return;
        }
        this.countryToTakeAway = countryToTakeAway;
    }

    private void BattalionDeployment(bool isRedeployment)
    {
        if (isRedeployment)
        {
            countryToTakeAway.SetNumberOfBattalionsOccupying(
                countryToTakeAway.GetNumberOfBattalionsOccupying() - amountToExchange);
            Debug.Log("here");
            Debug.Log(amountToExchange + " " + countryToReinforce.GetNumberOfBattalionsOccupying() + " => " + countryToTakeAway.GetNumberOfBattalionsOccupying() + " redployment complete");

            countryToReinforce.SetNumberOfBattalionsOccupying(
                    countryToReinforce.GetNumberOfBattalionsOccupying() + amountToExchange);

            redeploymentDialog.SetActive(false);
            Debug.Log(amountToExchange + " " + countryToReinforce.GetNumberOfBattalionsOccupying() + " => " + countryToTakeAway.GetNumberOfBattalionsOccupying() + " redployment complete");

            countryToTakeAway.GetNextStateCountry().SetEnabled(true);
            countryToTakeAway.GetNextStateCountry().SwitchCountryView();

            countryToReinforce.GetNextStateCountry().SetEnabled(true);
            countryToReinforce.GetNextStateCountry().SwitchCountryView();
        }
        else
        {
            countryToReinforce.SetNumberOfBattalionsOccupying(
                    countryToReinforce.GetNumberOfBattalionsOccupying() + 1);

            countryToReinforce.SetEnabled(true);
            countryToReinforce.toggleTrigger(true);
            countryToReinforce.GetNextStateCountry().toggleTrigger(true);
            countryToReinforce.GetNextStateCountry().SwitchCountryView();
        }
    }

}
