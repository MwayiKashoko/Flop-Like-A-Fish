using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;

public class KenoGame : MonoBehaviour {
    private List<bool> buttonsSelected;
    private List<bool> grid;
    private int numButtonSelected = 0;
    private int kenoSquaresHit = 0;

    private Regex findNum = new Regex(@"\d+");

    private string state = "Betting";

    private string risk = "Classic";

    private Button BackButton;
    private TMP_InputField BetInput;
    private TMP_Dropdown RiskDropdown;
    private Button BetButton;
    private Button ClearTableButton;
    private TMP_Text MoneyText;
    private Toggle BoosterToggle;
    private Button HelpButton;

    public Sprite gem;
    public Sprite buttonImage;

    public string gameMode;

    private int buttonCount;

    private bool startedCoroutine = false;

    //Payouts based on the number of spots selected Classic Risk
    /*private float[] oneSpotPayoutsClassic = {0, 3.96f};
    private float[] twoSpotPayoutsClassic = {0, 1.9f, 4.5f};
    private float[] threeSpotPayoutsClassic = {0, 1f, 3.1f, 10.4f};
    private float[] fourSpotPayoutsClassic = {0, 0.8f, 1.8f, 5f, 22.5f};
    private float[] fiveSpotPayoutsClassic = {0, 0.25f, 1.4f, 4.1f, 16.5f, 36f};
    private float[] sixSpotPayoutsClassic = {0, 0, 1f, 3.68f, 7f, 16.5f, 40f};
    private float[] sevenSpotPayoutsClassic = {0, 0, 0.47f, 3f, 4.5f, 14f, 31f, 60f};
    private float[] eightSpotPayoutsClassic = {0, 0, 0, 2.2f, 4f, 13f, 22f, 55f, 70f};
    private float[] nineSpotPayoutsClassic = {0, 0, 0, 1.55f, 3f, 8f, 15f, 44f, 60f, 85f};
    private float[] tenSpotPayoutsClassic = {0, 0, 0, 1.4f, 2.25f, 4.5f, 8f, 17f, 50f, 80f, 100f};*/

    private float[,] payoutsClassic = { {0, 3.96f, 0, 0, 0, 0, 0, 0, 0, 0, 0}, 
                                        {0, 1.9f, 4.5f, 0, 0, 0, 0, 0, 0, 0, 0},
                                        {0, 1f, 3.1f, 10.4f, 0, 0, 0, 0, 0, 0, 0},
                                        {0, 0.8f, 1.8f, 5f, 22.5f, 0, 0, 0, 0, 0, 0},
                                        {0, 0.25f, 1.4f, 4.1f, 16.5f, 36f, 0, 0, 0, 0, 0},
                                        {0, 0, 1f, 3.68f, 7f, 16.5f, 40f, 0, 0, 0, 0},
                                        {0, 0, 0.47f, 3f, 4.5f, 14f, 31f, 60f, 0, 0, 0},
                                        {0, 0, 0, 2.2f, 4f, 13f, 22f, 55f, 70f, 0, 0},
                                        {0, 0, 0, 1.55f, 3f, 8f, 15f, 44f, 60f, 85f, 0},
                                        {0, 0, 0, 1.4f, 2.25f, 4.5f, 8f, 17f, 50f, 80f, 100f}};

    //Payouts based on the number of spots selected Low Risk
    /*private float[] oneSpotPayoutsLow = {0.7f, 3.96f};
    private float[] twoSpotPayoutsLow = {0, 2f, 3.8f};
    private float[] threeSpotPayoutsLow = {0, 1.1f, 1.38f, 26f};
    private float[] fourSpotPayoutsLow = {0, 0, 2.2f, 7.9f, 90f};
    private float[] fiveSpotPayoutsLow = {0, 0, 1.5f, 4.2f, 13f, 300f};
    private float[] sixSpotPayoutsLow = {0, 0, 1.1f, 2f, 6.2f, 100f, 700f};
    private float[] sevenSpotPayoutsLow = {0, 0, 1.1f, 1.6f, 3.5f, 15f, 225f, 700f};
    private float[] eightSpotPayoutsLow = {0, 0, 1.1f, 1.5f, 2f, 5.5f, 39f, 100f, 800f};
    private float[] nineSpotPayoutsLow = {0, 0, 1.1f, 1.3f, 1.7f, 2.5f, 7.5f, 50f, 250f, 1000f};
    private float[] tenSpotPayoutsLow = {0, 0, 1.1f, 1.2f, 1.3f, 1.8f, 3.5f, 13f, 50f, 250f, 1000f};*/

    private float[,] payoutsLow = { {0.7f, 3.96f, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                    {0, 2f, 3.8f, 0, 0, 0, 0, 0, 0, 0, 0},
                                    {0, 1.1f, 1.38f, 26f, 0, 0, 0, 0, 0, 0, 0},
                                    {0, 0, 2.2f, 7.9f, 90f, 0, 0, 0, 0, 0, 0},
                                    {0, 0, 1.5f, 4.2f, 13f, 300f, 0, 0, 0, 0, 0},
                                    {0, 0, 1.1f, 2f, 6.2f, 100f, 700f, 0, 0, 0, 0},
                                    {0, 0, 1.1f, 1.6f, 3.5f, 15f, 225f, 700f, 0, 0, 0},
                                    {0, 0, 1.1f, 1.5f, 2f, 5.5f, 39f, 100f, 800f, 0, 0},
                                    {0, 0, 1.1f, 1.3f, 1.7f, 2.5f, 7.5f, 50f, 250f, 1000f, 0},
                                    {0, 0, 1.1f, 1.2f, 1.3f, 1.8f, 3.5f, 13f, 50f, 250f, 1000f}};

    //Payouts based on the number of spots selected Medium Risk
    /*private float[] oneSpotPayoutsMedium = {0.4f, 2.75f};
    private float[] twoSpotPayoutsMedium = {0, 1.8f, 5.1f};
    private float[] threeSpotPayoutsMedium = {0, 0, 2.8f, 50f};
    private float[] fourSpotPayoutsMedium = {0, 0, 1.7f, 10f, 100f};
    private float[] fiveSpotPayoutsMedium = {0, 0, 1.4f, 4f, 14f, 390f};
    private float[] sixSpotPayoutsMedium = {0, 0, 0, 3f, 9f, 180f, 710f};
    private float[] sevenSpotPayoutsMedium = {0, 0, 0, 2f, 7f, 30f, 400f, 800f};
    private float[] eightSpotPayoutsMedium = {0, 0, 0, 2f, 4f, 11f, 67f, 400f, 900f};
    private float[] nineSpotPayoutsMedium = {0, 0, 0, 2f, 2.5f, 5f, 15f, 100f, 500f, 1000f};
    private float[] tenSpotPayoutsMedium = {0, 0, 0, 1.6f, 2f, 4f, 7f, 26f, 100f, 500f, 1000f};*/

    private float[,] payoutsMedium = {  {0.4f, 2.75f, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                        {0, 1.8f, 5.1f, 0, 0, 0, 0, 0, 0, 0, 0},
                                        {0, 0, 2.8f, 50f, 0, 0, 0, 0, 0, 0, 0},
                                        {0, 0, 1.7f, 10f, 100f, 0, 0, 0, 0, 0, 0},
                                        {0, 0, 1.4f, 4f, 14f, 390f, 0, 0, 0, 0, 0},
                                        {0, 0, 0, 3f, 9f, 180f, 710f, 0, 0, 0, 0},
                                        {0, 0, 0, 2f, 7f, 30f, 400f, 800f, 0, 0, 0},
                                        {0, 0, 0, 2f, 4f, 11f, 67f, 400f, 900f, 0, 0},
                                        {0, 0, 0, 2f, 2.5f, 5f, 15f, 100f, 500f, 1000f, 0},
                                        {0, 0, 0, 1.6f, 2f, 4f, 7f, 26f, 100f, 500f, 1000f}};

    //Payouts based on the number of spots selected High Risk
    /*private float[] oneSpotPayoutsHigh = {0, 3.96f};
    private float[] twoSpotPayoutsHigh = {0, 0, 17f};
    private float[] threeSpotPayoutsHigh = {0, 0, 0, 81.5f};
    private float[] fourSpotPayoutsHigh = {0, 0, 0, 10f, 259f};
    private float[] fiveSpotPayoutsHigh = {0, 0, 0, 4.5f, 48f, 450f};
    private float[] sixSpotPayoutsHigh = {0, 0, 0, 0, 11f, 350f, 710f};
    private float[] sevenSpotPayoutsHigh = {0, 0, 0, 0, 7f, 90f, 400f, 800f};
    private float[] eightSpotPayoutsHigh = {0, 0, 0, 0, 5f, 20f, 270f, 600f, 900f};
    private float[] nineSpotPayoutsHigh = {0, 0, 0, 0, 4f, 11f, 56f, 500f, 800f, 1000f};
    private float[] tenSpotPayoutsHigh = {0, 0, 0, 0, 3.5f, 8f, 13f, 63f, 500f, 800f, 1000f};*/

    private float[,] payoutsHigh = {{0, 3.96f, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                    {0, 0, 17f, 0, 0, 0, 0, 0, 0, 0, 0},
                                    {0, 0, 0, 81.5f, 0, 0, 0, 0, 0, 0, 0},
                                    {0, 0, 0, 10f, 259f, 0, 0, 0, 0, 0, 0},
                                    {0, 0, 0, 4.5f, 48f, 450f, 0, 0, 0, 0, 0},
                                    {0, 0, 0, 0, 11f, 350f, 710f, 0, 0, 0, 0},
                                    {0, 0, 0, 0, 7f, 90f, 400f, 800f, 0, 0, 0},
                                    {0, 0, 0, 0, 5f, 20f, 270f, 600f, 900f, 0, 0},
                                    {0, 0, 0, 0, 4f, 11f, 56f, 500f, 800f, 1000f, 0},
                                    {0, 0, 0, 0, 3.5f, 8f, 13f, 63f, 500f, 800f, 1000f}};

    //Payouts based on the number of spots selected (Ohio Keno)
    private float[,] payoutsOhio = {{0, 2f, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                    {0, 0, 11f, 0, 0, 0, 0, 0, 0, 0, 0},
                                    {0, 0, 2f, 27f, 0, 0, 0, 0, 0, 0, 0},
                                    {0, 0, 1f, 5f, 72f, 0, 0, 0, 0, 0, 0},
                                    {0, 0, 0, 2f, 18f, 410f, 0, 0, 0, 0, 0},
                                    {0, 0, 0, 1f, 7f, 57f, 1100f, 0, 0, 0, 0},
                                    {0, 0, 0, 1f, 5f, 11f, 100f, 2000f, 0, 0, 0},
                                    {0, 0, 0, 0, 2f, 15f, 50f, 300f, 10000f, 0, 0},
                                    {0, 0, 0, 0, 2f, 5f, 20f, 100f, 2000f, 25000f, 0},
                                    {5f, 0, 0, 0, 0, 2f, 10f, 50f, 500f, 5000f, 100000f}};

    private decimal[] boosterMultipliers = {1M, 2M, 3M, 4M, 5M, 10M};

    // Start is called before the first frame update
    void Start() {
        buttonsSelected = new List<bool>();
        grid = new List<bool>();

        BackButton = GameObject.Find("BackButton").GetComponent<Button>();
        BetInput = GameObject.Find("BetInput").GetComponent<TMP_InputField>();
        BetButton = GameObject.Find("BetButton").GetComponent<Button>();
        ClearTableButton = GameObject.Find("ClearTableButton").GetComponent<Button>();
        MoneyText = GameObject.Find("MoneyText").GetComponent<TMP_Text>();
        HelpButton = GameObject.Find("HelpButton").GetComponent<Button>();

        if (gameMode == "Regular") {
            buttonCount = 40;

            RiskDropdown = GameObject.Find("RiskDropdown").GetComponent<TMP_Dropdown>();
        } else {
            buttonCount = 80;

            BoosterToggle = GameObject.Find("BoosterToggle").GetComponent<Toggle>();
        }

        for (int i = 1; i <= buttonCount; i++) {
            buttonsSelected.Add(false);
            grid.Add(false);

            GameObject.Find($"KenoButton ({i})").GetComponent<Image>().color = new Color32(171, 171, 171, 226);
        }

        MoneyText.text = $"Money: ${GlobalScript.money}";
    }

    public void changeValue(GameObject button) {
        Image img = button.GetComponent<Image>();

        if ((img.color.Equals(new Color32(171, 171, 171, 226)) || img.color.Equals(new Color32(178, 0, 0, 161))) && numButtonSelected < 10) {
            img.color = new Color32(180, 69, 233, 255);

            numButtonSelected++;

            buttonsSelected[Convert.ToInt32(findNum.Match((button.name)).Groups[0].Value)-1] = true;
        } else {
            if (img.color.Equals(new Color32(180, 69, 233, 255))) {
                numButtonSelected--;
            }

            img.color = new Color32(171, 171, 171, 226);

            buttonsSelected[Convert.ToInt32(findNum.Match((button.name)).Groups[0].Value)-1] = false;
        }
    }

    public void ClearTable() {
        for (int i = 0; i < buttonsSelected.Count; i++) {
            buttonsSelected[i] = false;

            GameObject.Find($"KenoButton ({i+1})").GetComponent<Image>().color = new Color32(171, 171, 171, 226);

            numButtonSelected = 0;
        }
    }

    public void Bet() {
        if (numButtonSelected > 0) {
            BackButton.interactable = false;
            BetInput.interactable = false;
            BetButton.interactable = false;
            ClearTableButton.interactable = false;
            HelpButton.interactable = false;

            for (int i = 1; i <= buttonCount; i++) {
                GameObject.Find($"KenoButton ({i})").GetComponent<Button>().interactable = false;
            }

            for (int i = 1; i <= buttonsSelected.Count; i++) {
                GameObject.Find($"KenoButton ({i})").GetComponent<Image>().sprite = buttonImage;
                GameObject.Find($"KenoButton ({i})").transform.GetChild(0).GetComponent<TMP_Text>().color = new Color32(50, 50, 50, 255);

                if (GameObject.Find($"KenoButton ({i})").GetComponent<Image>().color == new Color32(178, 0, 0, 161)) {
                    GameObject.Find($"KenoButton ({i})").GetComponent<Image>().color = new Color32(171, 171, 171, 226);
                }
            }

            GlobalScript.RoundInput(BetInput);

            GlobalScript.UpdateMoney("minus", GlobalScript.ReturnNum(BetInput.text));

            if (gameMode == "Ohio" && BoosterToggle.isOn) {
                GlobalScript.UpdateMoney("minus", GlobalScript.ReturnNum(BetInput.text));
                BoosterToggle.interactable = false;
            } else if (gameMode == "Regular") {
                RiskDropdown.interactable = false;
                risk = RiskDropdown.options[RiskDropdown.value].text;
            }

            MoneyText.text = $"Money: ${GlobalScript.money}";

            state = "Playing";
        } else {
            //Display message on screen saying to click buttons to play
        }
    }

    public void ResetGame() {
        BackButton.interactable = true;
        BetInput.interactable = true;
        BetButton.interactable = true;
        ClearTableButton.interactable = true;
        HelpButton.interactable = true;

        startedCoroutine = false;

        kenoSquaresHit = 0;

        for (int i = 1; i <= buttonCount; i++) {
            GameObject.Find($"KenoButton ({i})").GetComponent<Button>().interactable = true;

            grid[i-1] = false;
        }

        if (gameMode == "Regular") {
            RiskDropdown.interactable = true;
        } else {
            BoosterToggle.interactable = true;
        }

        state = "Betting";
    }

    public IEnumerator Wait() {
        startedCoroutine = true;

        for (int i = 0; i < grid.Count; i++) {
            if (buttonsSelected[i] == true && grid[i] == true) {
                GameObject.Find($"KenoButton ({i+1})").GetComponent<Image>().sprite = gem;
                GameObject.Find($"KenoButton ({i+1})").transform.GetChild(0).GetComponent<TMP_Text>().color = new Color32(255, 255, 255, 255);
                kenoSquaresHit++;

                yield return new WaitForSecondsRealtime(0.03f);
            } else if (grid[i] == true) {
                GameObject.Find($"KenoButton ({i+1})").GetComponent<Image>().color = new Color32(178, 0, 0, 161);

                yield return new WaitForSecondsRealtime(0.03f);
            }
        }

        state = "DeterminePayout";        
    }

    // Update is called once per frame
    void Update() {
        switch(state) {
            case "Betting":
                GlobalScript.ValidateInput(BetInput, BetButton);

                if (gameMode == "Ohio") {
                    if (GlobalScript.ReturnNum(BetInput.text) * 2 > GlobalScript.money) {
                        BoosterToggle.interactable = false;
                        BoosterToggle.isOn = false;
                    } else {
                        BoosterToggle.interactable = true;
                    }
                }

                break;
            case "Playing":
                int num = GlobalScript.random.Next(0, buttonCount);

                if (gameMode == "Regular") {
                    for (int i = 0; i < 10; i++) {
                        while (grid[num] == true) {
                            num = GlobalScript.random.Next(0, buttonCount);
                        }

                        grid[num] = true;

                        num = GlobalScript.random.Next(0, buttonCount);
                    }
                } else {
                    for (int i = 0; i < 20; i++) {
                        while (grid[num] == true) {
                            num = GlobalScript.random.Next(0, buttonCount);
                        }

                        grid[num] = true;

                        num = GlobalScript.random.Next(0, buttonCount);
                    }
                }

                state = "SetImages";

                break;

            case "SetImages":
                if (!startedCoroutine) {
                    StartCoroutine("Wait");
                }

                break;

            case "DeterminePayout":
                if (gameMode == "Regular") {
                    switch (risk) {
                        case "Classic":
                            GlobalScript.UpdateMoney("plus", GlobalScript.ReturnNum(BetInput.text) * (decimal) payoutsClassic[numButtonSelected-1, kenoSquaresHit]);
                            break;

                        case "Low Risk":
                            GlobalScript.UpdateMoney("plus", GlobalScript.ReturnNum(BetInput.text) * (decimal) payoutsLow[numButtonSelected-1, kenoSquaresHit]);
                            break;

                        case "Medium Risk":
                            GlobalScript.UpdateMoney("plus", GlobalScript.ReturnNum(BetInput.text) * (decimal) payoutsMedium[numButtonSelected-1, kenoSquaresHit]);
                            break;

                        case "High Risk":
                            GlobalScript.UpdateMoney("plus", GlobalScript.ReturnNum(BetInput.text) * (decimal) payoutsHigh[numButtonSelected-1, kenoSquaresHit]);
                            break;
                    }
                } else {
                    int index = 0;

                    if (gameMode == "Ohio" && BoosterToggle.isOn) {
                        //Show message that shows booster has been hit is achieved
                        double result = GlobalScript.random.NextDouble();

                        if (result >= 0.435 && result <= 0.835) {
                            index = 1;
                        } else if (result <= 0.897) {
                            index = 2;
                        } else if (result <= 0.96) {
                            index = 3;
                        } else if (result <= 0.99682) {
                            index = 4;
                        } else {
                            index = 5;
                        }
                    }

                    GlobalScript.UpdateMoney("plus", GlobalScript.ReturnNum(BetInput.text) * (decimal) payoutsOhio[numButtonSelected-1, kenoSquaresHit] * boosterMultipliers[index]);
                }

                MoneyText.text = $"Money: ${GlobalScript.money}";

                state = "Reset";
                break;

            case "Reset":
                ResetGame();
                break;
        }
    }
}
