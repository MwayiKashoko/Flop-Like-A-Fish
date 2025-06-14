using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HiLoGame : MonoBehaviour {
    private Button BackButton;
    private TMP_InputField BetInput;
    private Button HigherButton;
    private Button LowerButton;
    private Button SkipCardButton;
    private Button BetButton;
    private Image CardImage;
    private TMP_Text MoneyText;
    private TMP_Text ProfitHigherText;
    private TMP_Text ProfitHigherAmountText;
    private TMP_Text ProfitLowerText;
    private TMP_Text ProfitLowerAmountText;
    private TMP_Text TotalProfitText;
    private TMP_Text TotalProfitAmountText;
    private GameObject CardScrollView;
    private Button HelpButton;

    //The current multiplier
    private float currentMultiplier = 1f;

    //The current card which is being shown to the screen
    private Card currentCard;
    private Card previousCard;

    private string state = "Betting";

    private List<Image> images;
    private List<GameObject> cardImages;

    //Contains all the sprites used to draw the cards
    public Sprite[] spriteList;

    // Start is called before the first frame update
    void Start() {
        BackButton = GameObject.Find("BackButton").GetComponent<Button>();
        BetInput = GameObject.Find("BetInput").GetComponent<TMP_InputField>();
        HigherButton = GameObject.Find("HigherButton").GetComponent<Button>();
        LowerButton = GameObject.Find("LowerButton").GetComponent<Button>();
        SkipCardButton = GameObject.Find("SkipCardButton").GetComponent<Button>();
        BetButton = GameObject.Find("BetButton").GetComponent<Button>();
        CardImage = GameObject.Find("CardImage").GetComponent<Image>();
        MoneyText = GameObject.Find("MoneyText").GetComponent<TMP_Text>();
        ProfitHigherText = GameObject.Find("ProfitHigherText").GetComponent<TMP_Text>();
        ProfitHigherAmountText = GameObject.Find("ProfitHigherAmountText").GetComponent<TMP_Text>();
        ProfitLowerText = GameObject.Find("ProfitLowerText").GetComponent<TMP_Text>();
        ProfitLowerAmountText = GameObject.Find("ProfitLowerAmountText").GetComponent<TMP_Text>();
        TotalProfitText = GameObject.Find("TotalProfitText").GetComponent<TMP_Text>();
        TotalProfitAmountText = GameObject.Find("TotalProfitAmountText").GetComponent<TMP_Text>();
        CardScrollView = GameObject.Find("CardScrollView");
        HelpButton = GameObject.Find("HelpButton").GetComponent<Button>();

        images = new List<Image>();
        cardImages = new List<GameObject>();

        GlobalScript.deck.ChangeCardRankings("HiLo");

        GetNextCard();

        ChangeText();

        AddCardToSlot();

        MoneyText.text = $"Money: ${GlobalScript.money}";
    }

    //Called when the bet button is pressed which starts the game
    public void Bet() {
        switch (state) {
            case "Betting":
                HigherButton.interactable = true;
                LowerButton.interactable = true;
                BackButton.interactable = false;
                BetInput.interactable = false;
                HelpButton.interactable = false;

                GameObject.Find("BetButton").transform.GetChild(0).GetComponent<TMP_Text>().text = "Cashout";

                GlobalScript.RoundInput(BetInput);

                GlobalScript.UpdateMoney("minus", GlobalScript.ReturnNum(BetInput.text));

                MoneyText.text = $"Money: ${GlobalScript.money}";

                state = "Playing";
                break;

            case "Playing":
                HigherButton.interactable = false;
                LowerButton.interactable = false;
                BackButton.interactable = true;

                GlobalScript.UpdateMoney("plus", (decimal) Math.Round(currentMultiplier * (float) GlobalScript.ReturnNum(BetInput.text), 2));

                MoneyText.text = $"Money: ${GlobalScript.money}";

                state = "Reset";

                GameObject.Find("BetButton").transform.GetChild(0).GetComponent<TMP_Text>().text = "Bet";

                break;

            case "Lost":
                ResetGame();
                
                HigherButton.interactable = true;
                LowerButton.interactable = true;
                BackButton.interactable = false;

                GameObject.Find("BetButton").transform.GetChild(0).GetComponent<TMP_Text>().text = "Cashout";

                BetInput.text = $"{Math.Round(GlobalScript.ReturnNum(BetInput.text), 2)}";

                GlobalScript.RoundInput(BetInput);

                GlobalScript.UpdateMoney("minus", GlobalScript.ReturnNum(BetInput.text));

                MoneyText.text = $"Money: ${GlobalScript.money}";

                state = "Playing";

                break;
        }
    }

    public void ValidateInput() {
        GlobalScript.ValidateInput(BetInput, BetButton);

        float higherProbability = (float) Math.Round(12.87f/(14f - (float) currentCard.rankInt), 2);
        float lowerProbability = (float) Math.Round(12.87f/(float) currentCard.rankInt, 2);
        float currentBet = (float) GlobalScript.ReturnNum(BetInput.text);

        if (higherProbability == 0.99f) {
            higherProbability = 1.07f;
        } else if (lowerProbability == 0.99f) {
            lowerProbability = 1.07f;
        }

        ProfitHigherAmountText.text = $"${Math.Round(higherProbability * currentBet - currentBet, 2)}";

        ProfitLowerAmountText.text = $"${Math.Round(lowerProbability * currentBet - currentBet, 2)}";
    }

    public void GetNextCard() {
        previousCard = currentCard;

        currentCard = GlobalScript.GetNextCard();

        CardImage.sprite = spriteList[currentCard.pos];
    }

    //Changes text and probabilities on the buttons and text game objects
    public void ChangeText() {
        float higherText = (float) Math.Round((13f - (float) currentCard.rankInt + 1f)/13f, 4)*100f;

        GameObject.Find("LowerButton").transform.GetChild(0).GetComponent<TMP_Text>().text = $"Lower or Same ▼ {Math.Round((float) currentCard.rankInt/13f, 4)*100f}%";
        GameObject.Find("HigherButton").transform.GetChild(0).GetComponent<TMP_Text>().text = $"Higher or Same ▲ {higherText}%";

        if (currentCard.rankInt == 1) {
            GameObject.Find("LowerButton").transform.GetChild(0).GetComponent<TMP_Text>().text = $"Same = {Math.Round(1f/13f, 4)*100f}%";
            GameObject.Find("HigherButton").transform.GetChild(0).GetComponent<TMP_Text>().text = $"Higher ▲ {Math.Round(12f/13f, 4)*100f}%";
        } else if (currentCard.rankInt == 13) {
            GameObject.Find("LowerButton").transform.GetChild(0).GetComponent<TMP_Text>().text = $"Lower ▼ {Math.Round(12f/13f, 4)*100f}%";
            GameObject.Find("HigherButton").transform.GetChild(0).GetComponent<TMP_Text>().text = $"Same = {Math.Round(1f/13f, 4)*100f}%";
        }

        float higherProbability = (float) Math.Round(12.87f/(14f - (float) currentCard.rankInt), 2);
        float lowerProbability = (float) Math.Round(12.87f/(float) currentCard.rankInt, 2);
        float currentBet = (float) GlobalScript.ReturnNum(BetInput.text);

        if (higherProbability == 0.99f) {
            higherProbability = 1.07f;
        } else if (lowerProbability == 0.99f) {
            lowerProbability = 1.07f;
        }

        ProfitHigherText.text = $"Profit Higher ({higherProbability}x)";
        ProfitHigherAmountText.text = $"${Math.Round(higherProbability * currentBet - currentBet, 2)}";

        ProfitLowerText.text = $"Profit Lower ({lowerProbability}x)";
        ProfitLowerAmountText.text = $"${Math.Round(lowerProbability * currentBet - currentBet, 2)}";

        TotalProfitText.text = $"Total Profit ({currentMultiplier}x)";
        TotalProfitAmountText.text = $"${Math.Round(currentMultiplier * currentBet - currentBet, 2)}";
    }

    //Animation where the cards get added to the bottom of the screen
    public void AddCardToSlot() {
        float x = -880f + 250f * (float) images.Count;

        GlobalScript.DrawCard(x, 0f, 216f, 304f, currentCard, CardScrollView.transform.GetChild(0).GetChild(0).transform, images, cardImages, spriteList);
    }

    //Called when the Higher or same button is pressed
    public void GoHigher() {
        float higherProbability = (float) Math.Round(12.87f/(14f - (float) currentCard.rankInt), 2);

        if (higherProbability == 0.99f) {
            higherProbability = 1.07f;
        }

        GetNextCard();

        if (currentCard.rankInt >= previousCard.rankInt && previousCard.rankInt != 13) {
            currentMultiplier = (float) Math.Round(currentMultiplier * higherProbability, 2);

            AddCardToSlot();

            ChangeText();
        } else if (currentCard.rankInt == previousCard.rankInt && previousCard.rankInt == 13) {
            currentMultiplier = (float) Math.Round(currentMultiplier * higherProbability, 2);

            AddCardToSlot();

            ChangeText();
        } else {
            state = "Lost";

            HigherButton.interactable = false;
            LowerButton.interactable = false;
            BetInput.interactable = true;

            GameObject.Find("BetButton").transform.GetChild(0).GetComponent<TMP_Text>().text = "Bet";
        }
    }

    //Called when the Lower or same button is pressed
    public void GoLower() {
        float lowerProbability = (float) Math.Round(12.87f/(float) currentCard.rankInt, 2);

        if (lowerProbability == 0.99f) {
            lowerProbability = 1.07f;
        }

        GetNextCard();

        if (currentCard.rankInt <= previousCard.rankInt && previousCard.rankInt != 1) {
            currentMultiplier = (float) Math.Round(currentMultiplier * lowerProbability, 2);

            AddCardToSlot();

            ChangeText();
        } else if (currentCard.rankInt == previousCard.rankInt && previousCard.rankInt == 1) {
            currentMultiplier = (float) Math.Round(currentMultiplier * lowerProbability, 2);

            AddCardToSlot();

            ChangeText();
        } else {
            state = "Lost";

            BackButton.interactable = true;
            HigherButton.interactable = false;
            LowerButton.interactable = false;
            BetInput.interactable = true;

            GameObject.Find("BetButton").transform.GetChild(0).GetComponent<TMP_Text>().text = "Bet";
        }
    }

    //Called when the skip card button is pressed
    public void SkipCard() {
        GetNextCard();

        AddCardToSlot();

        ChangeText();
    }

    public void ResetGame() {
        BackButton.interactable = true;
        BetInput.interactable = true;
        BetButton.interactable = true;
        HelpButton.interactable = true;
        HigherButton.interactable = false;
        LowerButton.interactable = false;

        currentMultiplier = 1f;

        GetNextCard();

        ChangeText();

        AddCardToSlot();

        state = "Betting";

        GameObject.Find("BetButton").transform.GetChild(0).GetComponent<TMP_Text>().text = "Bet";

        GlobalScript.RemoveCards(images, cardImages);
    }

    // Update is called once per frame
    void Update() {
        switch (state) {
            case "Betting":
                ValidateInput();
                break;

            case "Reset":
                ResetGame();
                break;
        }
    }
}
