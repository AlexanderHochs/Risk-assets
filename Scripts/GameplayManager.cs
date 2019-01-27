using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//TODO: implement deploy mode for just initilizing i.e. a new round type and finish initializing
public class GameplayManager : MonoBehaviour
{

    private const string PLAYER = "player: ";
    private const string END_INVASION = "End Invasion Round";
    private const string END_REDEPLOYMENT = "End Redployment Round";

    // if public change to private after debug
    public bool startDeploy = false, startBattle = false, startRedeploy = false;

    public bool isStart = true;
    public bool isInitial = true;
    public GameObject transitionDialog;
    public GameObject transitionBtn;
    public Region[] regions;
    //public GameObject[] regionsRef;
    public Text transitionButtonText;
    public Text playerField;

    // properties
    public bool validCountryClick { get; set; }
    public Player curPlayer { get; set; }

    // TODO: fjnd more efficient way, maybe two cameras and switching
    private bool menuActive = false;

    private float timer = 0.5f;
    private Country countryReference;
    private int countryReferenceCount = 0;
    private int initialArmyCount;
    private int roundType = 2; // 0: deploy, 1: attack, 2: redeploy, 3: waiting/initial state
    private List<Country> countryList = new List<Country>();

    // variables eventually to be put in a static script
    // TODO: player references

    void Start()
    {
        transitionDialog.SetActive(false);
        transitionBtn.SetActive(false);
        InitializeGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < 0.5f && !menuActive)
        {
            timer += Time.deltaTime;
            if (timer > 0.5f)
            {
                toggleCountryEnability(true);
            }
        }
        if (isInitial && startDeploy)
        {
            roundType = 0;
            gameObject.GetComponent<ReinforcementManager>().StartDeploymentDialog();
            startDeploy = false;
        }
        else if (startDeploy)
        {
            startDeploy = false;
            roundType = 0;
            gameObject.GetComponent<ReinforcementManager>().StartDeploymentDialog();
        }
        else if (startBattle)
        {
            startBattle = false;
            roundType = 1;
        }
        else if (startRedeploy)
        {
            startRedeploy = false;
            roundType = 2;
        }
    }

    void InitializeGame()
    {
        foreach (Region region in regions)
        {
            region.Initialize();
            foreach (Country country in region.countries)
            {
                countryList.Add(country);
                country.SetNumberOfBattalionsOccupying(1);
            }
        }
        InitializeCountryOwnerShip();
        foreach (Player player in GameData.players)
        {
            setRegionOwnership(player);
        }
        // debug code
        initialArmyCount = 1;//(int)System.Math.Round((double)(120 / numberOfPlayers));

        //TODO: choose player by choice i.e. random
        curPlayer = GameData.players[0];
        playerField.text = PLAYER + 0;
        playerField.color = curPlayer.GetColor();
        isInitial = true;
        curPlayer.numberOfBattalionsToDeploy = AmountOfDeployments();
        startDeploy = true;
    }

    private void InitializeCountryOwnerShip()
    {
        Random rnd = new Random();

        int index = countryList.Count - 1;
        int playerIndex = 0;
        for (int i = index; i > -1; i--)
        {
            int countryIndex = Random.Range(0, i);
            Country country = countryList[countryIndex];
            //TODO: option in main menu to randomize or let people pick which country is theirs
            country.SetOwner(GameData.players[playerIndex]);
            GameData.players[playerIndex].AddCountry(country);
            countryList.Remove(country);

            playerIndex = (playerIndex + 1) % GameData.players.Count;
        }
    }

    public void InitializeGameCallback()
    {
        int index = GameData.players.IndexOf(curPlayer);
        curPlayer = GameData.players[(GameData.players.IndexOf(curPlayer) + 1) % GameData.players.Count];
        playerField.text = PLAYER + GameData.players.IndexOf(curPlayer);
        playerField.color = curPlayer.GetColor();
        curPlayer.numberOfBattalionsToDeploy = AmountOfDeployments();
        Debug.Log("player " + index + " added deployment to " + countryReference.gameObjectRef.name);
        if (index == GameData.players.Count - 1)
        {
            initialArmyCount--;
        }

        if (initialArmyCount > 0)
        {
            startDeploy = true;
        }
        else
        {
            isInitial = false;
            roundType = 3;
            RunTransitionDialog();
        }
    }

    void SendInvasionData()
    {
    }

    public int GetRoundType()
    {
        return roundType;
    }

    public int GetCountryReferenceCount()
    {
        return countryReferenceCount;
    }

    public void SetCountryReferenceCount(int count)
    {
        countryReferenceCount = count;
    }

    public Country GetCountryReference()
    {
        return countryReference;
    }

    public void SetCountryReference(Country countryReference)
    {
        this.countryReference = countryReference;
        Debug.Log("setting country reference");
    }

    public void OnCountryClick(bool isInitialState)
    {
        Debug.Log("here" + isInitialState);
        if (isInitialState)
        {
            // TODO: once again find better way than just a boolean
            switch (roundType)
            {
                case 0:
                    if (curPlayer.CountryOwned(countryReference))
                    {
                        gameObject.GetComponent<ReinforcementManager>().AddCountryToDeployList(countryReference);
                    }
                    break;
                case 1:
                    if (countryReferenceCount == 0 && curPlayer.CountryOwned(countryReference))
                    {
                        countryReferenceCount++;
                        SwitchCountryView();
                        gameObject.GetComponent<InvasionManager>().SetAttackingCountry(countryReference);
                    }
                    else if (!curPlayer.CountryOwned(countryReference) && ValidCountry())
                    {
                        countryReferenceCount++;
                        SwitchCountryView();
                        gameObject.GetComponent<InvasionManager>().SetDefendingCountry(countryReference);
                        gameObject.GetComponent<InvasionManager>().CommenceBattle();
                        countryReferenceCount = 0;
                    }
                    break;
                case 2:
                    if (curPlayer.CountryOwned(countryReference))
                    {
                        if (countryReferenceCount == 0)
                        {
                            countryReferenceCount++;
                            SwitchCountryView();
                            gameObject.GetComponent<ReinforcementManager>().SetCountryToTakeAway(countryReference);
                        }
                        else if (ValidCountry())
                        {
                            countryReferenceCount++;
                            SwitchCountryView();
                            gameObject.GetComponent<ReinforcementManager>().SetCountryToReinforce(countryReference);
                            gameObject.GetComponent<ReinforcementManager>().StartRedeploymentDialog();
                            countryReferenceCount = 0;
                        }
                    }
                    break;
                default:
                    Debug.Log("Round Type error ...");
                    break;
            }
        }
        else
        {
            SwitchCountryView();
            countryReferenceCount--;
        }
    }

    private void SwitchCountryView()
    {
        if (countryReference.IsEnabled())
        {
            countryReference.SwitchCountryView();
        }
    }

    private bool ValidCountry()
    {
        switch (roundType)
        {
            case 1:
                foreach (GameObject country in InvasionData.GetAttackingCountry().associatedCountriesRefs)
                {
                    if (countryReference.gameObjectRef == country)
                    {
                        return true;
                    }
                }
                return false;
            case 2:
                foreach (GameObject country in gameObject.GetComponent<ReinforcementManager>().GetCountryToTakeAway().associatedCountriesRefs)
                {
                    if (countryReference.gameObjectRef == country)
                    {
                        return true;
                    }
                }
                return false;
            default:
                return false;
        }
    }

    // Round managing functions
    public void StartTransitionDialog()
    {
        switch (roundType)
        {
            case 0:
                transitionButtonText.text = "End Deploy";
                break;
            case 1:
                transitionBtn.SetActive(true);

                transitionButtonText.text = "End Invasion";
                break;
            case 2:
                transitionButtonText.text = "End Round";
                break;
            default:
                Debug.Log("Round Type error ...");
                break;
        }
        transitionDialog.SetActive(true);
        menuActive = true;
        toggleCountryEnability(false);
        timer = 0;
    }

    public void RunTransitionDialog()
    {
        curPlayer.numberOfBattalionsToDeploy = AmountOfDeployments();
        switch (roundType)
        {
            case 0:
                transitionBtn.SetActive(true);
                transitionButtonText.text = END_INVASION;
                startBattle = true;
                break;
            case 1:
                transitionButtonText.text = END_REDEPLOYMENT;
                startRedeploy = true;
                break;
            case 2:
                curPlayer = GameData.players[(GameData.players.IndexOf(curPlayer) + 1) % GameData.players.Count];
                playerField.text = PLAYER + GameData.players.IndexOf(curPlayer).ToString();
                playerField.color = curPlayer.GetColor();
                newRound();
                break;
            case 3:
                newRound();
                break;
            default:
                Debug.Log("Round Type error ...");
                break;
        }
        menuActive = transitionDialog.activeInHierarchy;
    }

    void setRegionOwnership(Player player)
    {
        foreach (Region region in regions)
        {
            // add region recently obtained
            if (player.regionsOwned.IndexOf(region) < 0 && PlayerOwnsRegion(region, player))
            {
                player.regionsOwned.Add(region);
            }
            // remove region recently lost
            else if (player.regionsOwned.IndexOf(region) > -1 && !PlayerOwnsRegion(region, player))
            {
                player.regionsOwned.Remove(region);
            }
        }

    }

    bool PlayerOwnsRegion(Region region, Player player)
    {
        foreach (Country country in region.countries)
        {
            if (country.GetOwner() != player)
            {
                return false;
            }
        }
        return true;
    }

    void toggleCountryEnability(bool toggle)
    {
        // TODO: fix after regions are implemented
        foreach (Region region in regions)
        {
            foreach (Country country in region.countries)
            {
                country.toggleTrigger(toggle);
            }
        }
    }

    // TODO: this function
    void newRound()
    {
        startDeploy = true;
        setRegionOwnership(curPlayer);
        curPlayer.CalculateDeploymentNumber();
        transitionBtn.SetActive(false);
        if (transitionDialog.activeInHierarchy)
        {
            transitionDialog.SetActive(false);
            menuActive = false;
        }
    }

    int AmountOfDeployments()
    {
        if (isInitial)
        {
            return 1;
        }
        else
        {
            return 3;
        }
    }
}
