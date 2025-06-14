using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;

public class AllInOrFoldSinglePlayerGame : MonoBehaviour {
    public Sprite[] spriteList;

    private string state = "Betting";

    private decimal totalBetAmount = 0;

    private Button BackButton;
    private TMP_InputField BetInput;
    private Button BetButton;
    private GameObject FoldButton;
    private TMP_Text MoneyText;

    private GameObject[] playerCardImages = {null, null};
    private GameObject[] communityCardImages = {null, null, null, null, null};

    private Card[] communityCards = {null, null, null, null, null};
    private Card[] playerCards = {null, null};

    private List<Card> deck;

    private TMP_Text DescriptionTextRoyalFlushPayout;
    private TMP_Text DescriptionTextStraightFlushPayout;
    private TMP_Text DescriptionTextFourOfAKindPayout;
    private TMP_Text DescriptionTextFullHousePayout;
    private TMP_Text DescriptionTextFlushPayout;
    private TMP_Text DescriptionTextStraightPayout;
    private TMP_Text DescriptionTextThreeOfAKindPayout;
    private TMP_Text DescriptionTextTwoPairPayout;
    private Button HelpButton;

    private float elapsedTime = 0;
    private float desiredAnimationTime = 0.5f;
    private float percentageComplete = 0;

    public static Dictionary<string, decimal> handPayouts = new Dictionary<string, decimal> {
        {"Royal Flush", 200M},
        {"Straight Flush", 100M},
        {"Four of a Kind", 30M},
        {"Full House", 8M},
        {"Flush", 6M},
        {"Straight", 5M},
        {"Three of a Kind", 3M},
        {"Two Pair", 0.5M},
        {"Pair", 0M},
        {"High", 0M}
    };

    // Start is called before the first frame update
    void Start() {
        BackButton = GameObject.Find("BackButton").GetComponent<Button>();
        BetInput = GameObject.Find("BetInput").GetComponent<TMP_InputField>();
        BetButton = GameObject.Find("BetButton").GetComponent<Button>();
        FoldButton = GameObject.Find("FoldButton");
        MoneyText = GameObject.Find("MoneyText").GetComponent<TMP_Text>();

        communityCardImages[0] = GameObject.Find("CardImage1");
        communityCardImages[1] = GameObject.Find("CardImage2");
        communityCardImages[2] = GameObject.Find("CardImage3");
        communityCardImages[3] = GameObject.Find("CardImage4");
        communityCardImages[4] = GameObject.Find("CardImage5");

        playerCardImages[0] = GameObject.Find("PlayerCardImage1");
        playerCardImages[1] = GameObject.Find("PlayerCardImage2");

        MoneyText.text = $"Money: ${GlobalScript.money}";

        DescriptionTextRoyalFlushPayout = GameObject.Find("DescriptionTextRoyalFlush").GetComponent<TMP_Text>();
        DescriptionTextStraightFlushPayout = GameObject.Find("DescriptionTextStraightFlush").GetComponent<TMP_Text>();
        DescriptionTextFourOfAKindPayout = GameObject.Find("DescriptionTextFourOfAKind").GetComponent<TMP_Text>();
        DescriptionTextFullHousePayout = GameObject.Find("DescriptionTextFullHouse").GetComponent<TMP_Text>();
        DescriptionTextFlushPayout = GameObject.Find("DescriptionTextFlush").GetComponent<TMP_Text>();
        DescriptionTextStraightPayout = GameObject.Find("DescriptionTextStraight").GetComponent<TMP_Text>();
        DescriptionTextThreeOfAKindPayout = GameObject.Find("DescriptionTextThreeOfAKind").GetComponent<TMP_Text>();
        DescriptionTextTwoPairPayout = GameObject.Find("DescriptionTextTwoPair").GetComponent<TMP_Text>();
        HelpButton = GameObject.Find("HelpButton").GetComponent<Button>();

        GlobalScript.deck.ChangeCardRankings("Video Poker");

        deck = new List<Card>();

        GlobalScript.ShuffleCards(deck, 1, true);
    }

    public void ValidateInput() {
        GlobalScript.ValidateInput(BetInput, BetButton.GetComponent<Button>());
    }

    public void Bet() {
        if (state == "Betting") {
            BackButton.interactable = false;
            BetInput.interactable = false;
            HelpButton.interactable = false;
            FoldButton.GetComponent<Button>().interactable = true;

            ValidateInput();

            GlobalScript.RoundInput(BetInput);

            GlobalScript.UpdateMoney("minus", GlobalScript.ReturnNum(BetInput.text));

            for (int i = 0; i < communityCardImages.Length; i++) {
                communityCardImages[i].transform.localRotation = Quaternion.Euler(0, 180, 0);
                communityCardImages[i].GetComponent<Image>().sprite = spriteList.Last();

                try {
                    playerCardImages[i].transform.localRotation = Quaternion.Euler(0, 180, 0);
                    playerCardImages[i].GetComponent<Image>().sprite = spriteList.Last();
                } catch (Exception) {

                }
            }

            totalBetAmount += GlobalScript.ReturnNum(BetInput.text);

            state = "StartGame";
        } else if (state == "Bet Or Fold") {
            FoldButton.GetComponent<Button>().interactable = false;

            GlobalScript.UpdateMoney("minus", GlobalScript.ReturnNum(BetInput.text) * 10M);

            totalBetAmount += GlobalScript.ReturnNum(BetInput.text) * 10M;

            state = "Flop";
        }

        MoneyText.text = $"Money: ${GlobalScript.money}";
    }

    public void Fold() {
        state = "Reset";
    }

    public void ResetGame() {
        BackButton.interactable = true;
        BetInput.interactable = true;
        HelpButton.interactable = true;
        FoldButton.GetComponent<Button>().interactable = false;

        ValidateInput();

        GlobalScript.ShuffleCards(deck, 1, true);

        MoneyText.text = $"Money: ${GlobalScript.money}";

        elapsedTime = 0;

        totalBetAmount = 0;

        state = "Betting";
    }

    // Update is called once per frame
    void Update() {
        switch (state) {
            case "Betting":
                ValidateInput();

                break;

            case "StartGame":
                DescriptionTextRoyalFlushPayout.color = new Color32(255, 255, 255, 255);
                DescriptionTextStraightFlushPayout.color = new Color32(255, 255, 255, 255);
                DescriptionTextFourOfAKindPayout.color = new Color32(255, 255, 255, 255);
                DescriptionTextFullHousePayout.color = new Color32(255, 255, 255, 255);
                DescriptionTextFlushPayout.color = new Color32(255, 255, 255, 255);
                DescriptionTextStraightPayout.color = new Color32(255, 255, 255, 255);
                DescriptionTextThreeOfAKindPayout.color = new Color32(255, 255, 255, 255);
                DescriptionTextTwoPairPayout.color = new Color32(255, 255, 255, 255);

                Card nextCard = GlobalScript.Pop(deck);

                for (int i = 0; i < playerCards.Length; i++) {
                    nextCard = GlobalScript.Pop(deck);

                    playerCards[i] = nextCard;
                    playerCards[i].onBack = true;
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
                }

                if (percentageComplete >= 1) {
                    elapsedTime = 0;
                    state = "Bet Or Fold";
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
                int[] hand = GlobalScript.DeterminePokerHand("Texas Hold'Em", communityCards, playerCards);
                string handDescription = GlobalScript.handRankings[hand[0]];

                try {
                    GlobalScript.UpdateMoney("plus", totalBetAmount * handPayouts[handDescription]);

                    switch(handDescription) {
                        case "Royal Flush":
                            DescriptionTextRoyalFlushPayout.color = new Color32(200, 0, 0, 255);
                            break;

                        case "Straight Flush":
                            DescriptionTextStraightFlushPayout.color = new Color32(200, 0, 0, 255);
                            break;

                        case "Four of a Kind":
                            DescriptionTextFourOfAKindPayout.color = new Color32(200, 0, 0, 255);
                            break;

                        case "Full House":
                            DescriptionTextFullHousePayout.color = new Color32(200, 0, 0, 255);
                            break;

                        case "Flush":
                            DescriptionTextFlushPayout.color = new Color32(200, 0, 0, 255);
                            break;

                        case "Straight":
                            DescriptionTextStraightPayout.color = new Color32(200, 0, 0, 255);
                            break;

                        case "Three of a Kind":
                            DescriptionTextThreeOfAKindPayout.color = new Color32(200, 0, 0, 255);
                            break;

                        case "Two Pair":
                            DescriptionTextTwoPairPayout.color = new Color32(200, 0, 0, 255);
                            break;
                    }
                } catch (Exception) {
                            
                }

                state = "Reset";

                break;

            case "Reset":
                ResetGame();
                break;
        }
    }
}
