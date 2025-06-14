using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;

public class ChoosePokerHandSinglePlayerGame : MonoBehaviour {
    public Sprite[] spriteList;

    private string state = "Betting";

    private Button BackButton;
    private TMP_InputField BetInput;
    private TMP_Dropdown HandDropdown;
    private Button BetButton;
    private TMP_Text MoneyText;

    private TMP_Text DescriptionPocketAces;
    private TMP_Text Description72;
    private TMP_Text DescriptionJack8;
    private TMP_Text DescriptionPocket2s;
    private Button HelpButton;

    private GameObject[] playerCardImages = {null, null};
    private GameObject[] dealerCardImages = {null, null};
    private GameObject[] communityCardImages = {null, null, null, null, null};

    private Card[] communityCards = {null, null, null, null, null};
    private Card[] playerCards = {null, null};
    private Card[] dealerCards = {null, null};

    private List<Card> deck;

    private int[,] cardIndexes = {{0, 13}, {6, 1}, {10, 7}, {1, 14}};

    public static Dictionary<int, decimal> handPayouts = new Dictionary<int, decimal> {
        {0, 1.15M},
        {1, 2.8M},
        {2, 1.93M},
        {3, 1.98M},
    };

    private float elapsedTime = 0;
    private float desiredAnimationTime = 0.5f;
    private float percentageComplete = 0;

    // Start is called before the first frame update
    void Start() {
        BackButton = GameObject.Find("BackButton").GetComponent<Button>();
        BetInput = GameObject.Find("BetInput").GetComponent<TMP_InputField>();
        HandDropdown = GameObject.Find("HandDropdown").GetComponent<TMP_Dropdown>();
        BetButton = GameObject.Find("BetButton").GetComponent<Button>();
        MoneyText = GameObject.Find("MoneyText").GetComponent<TMP_Text>();

        DescriptionPocketAces = GameObject.Find("DescriptionPocketAces").GetComponent<TMP_Text>();
        Description72 = GameObject.Find("Description72").GetComponent<TMP_Text>();
        DescriptionJack8 = GameObject.Find("DescriptionJack8").GetComponent<TMP_Text>();
        DescriptionPocket2s = GameObject.Find("DescriptionPocket2s").GetComponent<TMP_Text>();
        HelpButton = GameObject.Find("HelpButton").GetComponent<Button>();

        communityCardImages[0] = GameObject.Find("CardImage1");
        communityCardImages[1] = GameObject.Find("CardImage2");
        communityCardImages[2] = GameObject.Find("CardImage3");
        communityCardImages[3] = GameObject.Find("CardImage4");
        communityCardImages[4] = GameObject.Find("CardImage5");

        playerCardImages[0] = GameObject.Find("PlayerCardImage1");
        playerCardImages[1] = GameObject.Find("PlayerCardImage2");
        dealerCardImages[0] = GameObject.Find("DealerCardImage1");
        dealerCardImages[1] = GameObject.Find("DealerCardImage2");

        MoneyText.text = $"Money: ${GlobalScript.money}";

        GlobalScript.deck.ChangeCardRankings("Video Poker");

        deck = new List<Card>();

        GlobalScript.CopyDeck(deck);
    }

    public void Bet() {
        if (state == "Betting") {
            BackButton.interactable = false;
            BetInput.interactable = false;
            HandDropdown.interactable = false;
            BetButton.interactable = false;
            HelpButton.interactable = false;

            GlobalScript.ValidateInput(BetInput, BetButton);

            GlobalScript.UpdateMoney("minus", GlobalScript.ReturnNum(BetInput.text));

            for (int i = 0; i < communityCardImages.Length; i++) {
                communityCardImages[i].transform.localRotation = Quaternion.Euler(0, 180, 0);
                communityCardImages[i].GetComponent<Image>().sprite = spriteList.Last();

                try {
                    playerCardImages[i].transform.localRotation = Quaternion.Euler(0, 180, 0);
                    playerCardImages[i].GetComponent<Image>().sprite = spriteList.Last();
                    dealerCardImages[i].transform.localRotation = Quaternion.Euler(0, 180, 0);
                    dealerCardImages[i].GetComponent<Image>().sprite = spriteList.Last();
                } catch (Exception) {

                }
            }

            state = "StartGame"; 

            MoneyText.text = $"Money: ${GlobalScript.money}";
        }
    }

    public void ResetGame() {
        BackButton.interactable = true;
        BetInput.interactable = true;
        HandDropdown.interactable = true;
        BetButton.interactable = true;
        HelpButton.interactable = true;

        GlobalScript.ValidateInput(BetInput, BetButton);

        GlobalScript.CopyDeck(deck);

        MoneyText.text = $"Money: ${GlobalScript.money}";

        elapsedTime = 0;

        state = "Betting";
    }

    // Update is called once per frame
    void Update() {
        switch (state) {
            case "Betting":
                GlobalScript.ValidateInput(BetInput, BetButton);

                break;

            case "StartGame":
                DescriptionPocketAces.color = new Color32(255, 255, 255, 255);
                Description72.color = new Color32(255, 255, 255, 255);
                DescriptionJack8.color = new Color32(255, 255, 255, 255);
                DescriptionPocket2s.color = new Color32(255, 255, 255, 255);

                int randInt1 = GlobalScript.random.Next(0, 4);
                int randInt2 = GlobalScript.random.Next(0, 4);

                if (HandDropdown.value == 0 || HandDropdown.value == 3) {
                    while (cardIndexes[HandDropdown.value, 0] + randInt1 * 13 == cardIndexes[HandDropdown.value, 1] + randInt2 * 13 || cardIndexes[HandDropdown.value, 1] + randInt2 * 13 > 51) {
                        randInt2 = GlobalScript.random.Next(0, 4);
                    }
                } else {
                    while (randInt1 == randInt2 || cardIndexes[HandDropdown.value, 1] + randInt2 * 13 > 51) {
                        randInt2 = GlobalScript.random.Next(0, 4);
                    }
                }

                playerCards[0] = deck[cardIndexes[HandDropdown.value, 0] + randInt1 * 13];
                playerCards[1] = deck[cardIndexes[HandDropdown.value, 1] + randInt2 * 13];

                deck.RemoveAt(cardIndexes[HandDropdown.value, 0] + randInt1 * 13);
                deck.RemoveAt(cardIndexes[HandDropdown.value, 1] + randInt2 * 13);

                GlobalScript.ShuffleCards(deck, 1, false);

                Card nextCard = GlobalScript.Pop(deck);

                for (int i = 0; i < playerCards.Length; i++) {
                    playerCards[i].onBack = true;

                    nextCard = GlobalScript.Pop(deck);

                    dealerCards[i] = nextCard;
                    dealerCards[i].onBack = true;
                }

                for (int i = 0; i < communityCards.Length; i++) {
                    nextCard = GlobalScript.Pop(deck);

                    communityCards[i] = nextCard;
                    communityCards[i].onBack = true;
                }

                state = "Preflop";

                break;

            case "Preflop":
                elapsedTime += Time.deltaTime;
                percentageComplete = elapsedTime / desiredAnimationTime;

                for (int i = 0; i < playerCards.Length; i++) {
                    GlobalScript.FlipCardOver(percentageComplete, playerCardImages[i], spriteList, playerCards[i].pos);
                    GlobalScript.FlipCardOver(percentageComplete, dealerCardImages[i], spriteList, dealerCards[i].pos);
                }

                if (percentageComplete >= 1) {
                    elapsedTime = 0;
                    state = "Flop";
                }

                break;

            case "Flop":
                elapsedTime += Time.deltaTime;
                percentageComplete = elapsedTime / desiredAnimationTime;

                for (int i = 0; i < communityCards.Length-2; i++) {
                    GlobalScript.FlipCardOver(percentageComplete, communityCardImages[i], spriteList, communityCards[i].pos);
                }

                if (percentageComplete >= 1) {
                    elapsedTime = 0;
                    state = "Turn";
                }

                break;

            case "Turn":
                elapsedTime += Time.deltaTime;
                percentageComplete = elapsedTime / desiredAnimationTime;

                GlobalScript.FlipCardOver(percentageComplete, communityCardImages[3], spriteList, communityCards[3].pos);

                if (percentageComplete >= 1) {
                    elapsedTime = 0;
                    state = "River";
                }

                break;

            case "River":
                elapsedTime += Time.deltaTime;
                percentageComplete = elapsedTime / desiredAnimationTime;

                GlobalScript.FlipCardOver(percentageComplete, communityCardImages.Last(), spriteList, communityCards.Last().pos);

                if (percentageComplete >= 1) {
                    elapsedTime = 0;
                    state = "Determine Payout";
                }

                break;

            case "Determine Payout":
                int[] playerHand = GlobalScript.DeterminePokerHand("Texas Hold'Em", communityCards, playerCards);
                int[] dealerHand = GlobalScript.DeterminePokerHand("Texas Hold'Em", communityCards, dealerCards);

                int[] bestHand = {100};
                string winner = "Player";

                if (GlobalScript.DeterminePokerTie(playerHand, dealerHand)) {
                    winner = "Tie";
                } else {
                    bestHand = GlobalScript.DetermineBestHand(playerHand, dealerHand);

                    if (dealerHand == bestHand) {
                        winner = "Dealer";
                    }
                }

                if (winner == "Player") {
                    int val = HandDropdown.value;

                    GlobalScript.UpdateMoney("plus", GlobalScript.ReturnNum(BetInput.text) * handPayouts[val]);

                    if (val == 0) {
                        DescriptionPocketAces.color = new Color32(128, 0, 0, 255);
                    } else if (val == 1) {
                        Description72.color = new Color32(128, 0, 0, 255);
                    } else if (val == 2) {
                        DescriptionJack8.color = new Color32(128, 0, 0, 255);
                    } else {
                        DescriptionPocket2s.color = new Color32(128, 0, 0, 255);
                    }
                }

                state = "Reset";

                break;

            case "Reset":
                ResetGame();
                break;
        }
    }
}
