using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;

public class TexasHoldEmvsDealerGame : MonoBehaviour {
    public Sprite[] spriteList;

    private string state = "Betting";

    private Button BackButton;
    private Button BetButton;
    private TMP_Text MoneyText;

    private GameObject BetInputPlayerWins;
    private GameObject BetInputDraw;
    private GameObject BetInputDealerWins;
    private GameObject FourOfAKindOrHigherInput;
    private GameObject FullHouseInput;
    private GameObject FlushInput;
    private GameObject StraightInput;
    private GameObject ThreeOfAKindInput;
    private GameObject TwoPairInput;
    private GameObject PairInput;
    private GameObject HighCardInput;
    private Button HelpButton;

    private GameObject[] playerCardImages = {null, null};
    private GameObject[] dealerCardImages = {null, null};
    private GameObject[] communityCardImages = {null, null, null, null, null};

    private Card[] communityCards = {null, null, null, null, null};
    private Card[] playerCards = {null, null};
    private Card[] dealerCards = {null, null};

    private List<Card> deck;

    public static Dictionary<int, decimal> handPayouts = new Dictionary<int, decimal> {
        {1, 250},
        {2, 250},
        {3, 250},
        {4, 35},
        {5, 20},
        {6, 15},
        {7, 10},
        {8, 3},
        {9, 1.18M},
        {10, 3}
    };

    private float elapsedTime = 0;
    private float desiredAnimationTime = 0.5f;
    private float percentageComplete = 0;

    // Start is called before the first frame update
    void Start() {
        BackButton = GameObject.Find("BackButton").GetComponent<Button>();
        BetButton = GameObject.Find("BetButton").GetComponent<Button>();
        MoneyText = GameObject.Find("MoneyText").GetComponent<TMP_Text>();
        HelpButton = GameObject.Find("HelpButton").GetComponent<Button>();

        BetInputPlayerWins = GameObject.Find("BetInputPlayerWins");
        BetInputDraw = GameObject.Find("BetInputDraw");
        BetInputDealerWins = GameObject.Find("BetInputDealerWins");
        FourOfAKindOrHigherInput = GameObject.Find("FourOfAKindOrHigherInput");
        FullHouseInput = GameObject.Find("FullHouseInput");
        FlushInput = GameObject.Find("FlushInput");
        StraightInput = GameObject.Find("StraightInput");
        ThreeOfAKindInput = GameObject.Find("ThreeOfAKindInput");
        TwoPairInput = GameObject.Find("TwoPairInput");
        PairInput = GameObject.Find("PairInput");
        HighCardInput = GameObject.Find("HighCardInput");

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

        GlobalScript.ShuffleCards(deck, 1, true);
    }

    public decimal getSumOfInputs() {
        GlobalScript.RoundInput(BetInputPlayerWins.GetComponent<TMP_InputField>());
        GlobalScript.RoundInput(BetInputDraw.GetComponent<TMP_InputField>());
        GlobalScript.RoundInput(BetInputDealerWins.GetComponent<TMP_InputField>());
        GlobalScript.RoundInput(FourOfAKindOrHigherInput.GetComponent<TMP_InputField>());
        GlobalScript.RoundInput(FullHouseInput.GetComponent<TMP_InputField>());
        GlobalScript.RoundInput(FlushInput.GetComponent<TMP_InputField>());
        GlobalScript.RoundInput(StraightInput.GetComponent<TMP_InputField>());
        GlobalScript.RoundInput(ThreeOfAKindInput.GetComponent<TMP_InputField>());
        GlobalScript.RoundInput(TwoPairInput.GetComponent<TMP_InputField>());
        GlobalScript.RoundInput(PairInput.GetComponent<TMP_InputField>());
        GlobalScript.RoundInput(HighCardInput.GetComponent<TMP_InputField>());

        decimal sumMoney = GlobalScript.ReturnNum(BetInputPlayerWins.GetComponent<TMP_InputField>().text) + GlobalScript.ReturnNum(BetInputDraw.GetComponent<TMP_InputField>().text) + GlobalScript.ReturnNum(BetInputDealerWins.GetComponent<TMP_InputField>().text) + GlobalScript.ReturnNum(FourOfAKindOrHigherInput.GetComponent<TMP_InputField>().text) + GlobalScript.ReturnNum(FullHouseInput.GetComponent<TMP_InputField>().text) + GlobalScript.ReturnNum(FlushInput.GetComponent<TMP_InputField>().text) + GlobalScript.ReturnNum(StraightInput.GetComponent<TMP_InputField>().text) + GlobalScript.ReturnNum(ThreeOfAKindInput.GetComponent<TMP_InputField>().text) + GlobalScript.ReturnNum(TwoPairInput.GetComponent<TMP_InputField>().text) + GlobalScript.ReturnNum(PairInput.GetComponent<TMP_InputField>().text) + GlobalScript.ReturnNum(HighCardInput.GetComponent<TMP_InputField>().text);

        return Math.Round(sumMoney, 2);
    }

    public void ValidateInput() {
        GlobalScript.ValidateInput(BetInputPlayerWins.GetComponent<TMP_InputField>(), BetButton);
        GlobalScript.ValidateInput(BetInputDraw.GetComponent<TMP_InputField>(), BetButton);
        GlobalScript.ValidateInput(BetInputDealerWins.GetComponent<TMP_InputField>(), BetButton);
        GlobalScript.ValidateInput(FourOfAKindOrHigherInput.GetComponent<TMP_InputField>(), BetButton);
        GlobalScript.ValidateInput(FullHouseInput.GetComponent<TMP_InputField>(), BetButton);
        GlobalScript.ValidateInput(FlushInput.GetComponent<TMP_InputField>(), BetButton);
        GlobalScript.ValidateInput(StraightInput.GetComponent<TMP_InputField>(), BetButton);
        GlobalScript.ValidateInput(ThreeOfAKindInput.GetComponent<TMP_InputField>(), BetButton);
        GlobalScript.ValidateInput(TwoPairInput.GetComponent<TMP_InputField>(), BetButton);
        GlobalScript.ValidateInput(PairInput.GetComponent<TMP_InputField>(), BetButton);
        GlobalScript.ValidateInput(HighCardInput.GetComponent<TMP_InputField>(), BetButton);

        if (getSumOfInputs() > GlobalScript.money || BetButton.interactable == false) {
            BetButton.interactable = false;
        } else {
            BetButton.interactable = true;
        }
    }

    public void Bet() {
        if (state == "Betting") {
            BackButton.interactable = false;
            BetButton.interactable = false;
            HelpButton.interactable = false;

            BetInputPlayerWins.GetComponent<TMP_InputField>().interactable = false;
            BetInputDraw.GetComponent<TMP_InputField>().interactable = false;
            BetInputDealerWins.GetComponent<TMP_InputField>().interactable = false;
            FourOfAKindOrHigherInput.GetComponent<TMP_InputField>().interactable = false;
            FullHouseInput.GetComponent<TMP_InputField>().interactable = false;
            FlushInput.GetComponent<TMP_InputField>().interactable = false;
            StraightInput.GetComponent<TMP_InputField>().interactable = false;
            ThreeOfAKindInput.GetComponent<TMP_InputField>().interactable = false;
            TwoPairInput.GetComponent<TMP_InputField>().interactable = false;
            PairInput.GetComponent<TMP_InputField>().interactable = false;
            HighCardInput.GetComponent<TMP_InputField>().interactable = false;

            ValidateInput();

            GlobalScript.UpdateMoney("minus", GlobalScript.ReturnNum(BetInputPlayerWins.GetComponent<TMP_InputField>().text), GlobalScript.ReturnNum(BetInputDraw.GetComponent<TMP_InputField>().text), GlobalScript.ReturnNum(BetInputDealerWins.GetComponent<TMP_InputField>().text), GlobalScript.ReturnNum(FourOfAKindOrHigherInput.GetComponent<TMP_InputField>().text), GlobalScript.ReturnNum(FullHouseInput.GetComponent<TMP_InputField>().text), GlobalScript.ReturnNum(FlushInput.GetComponent<TMP_InputField>().text), GlobalScript.ReturnNum(StraightInput.GetComponent<TMP_InputField>().text), GlobalScript.ReturnNum(ThreeOfAKindInput.GetComponent<TMP_InputField>().text), GlobalScript.ReturnNum(TwoPairInput.GetComponent<TMP_InputField>().text), GlobalScript.ReturnNum(PairInput.GetComponent<TMP_InputField>().text), GlobalScript.ReturnNum(HighCardInput.GetComponent<TMP_InputField>().text));

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
        BetButton.interactable = true;
        HelpButton.interactable = true;
        
        BetInputPlayerWins.GetComponent<TMP_InputField>().interactable = true;
        BetInputDraw.GetComponent<TMP_InputField>().interactable = true;
        BetInputDealerWins.GetComponent<TMP_InputField>().interactable = true;
        FourOfAKindOrHigherInput.GetComponent<TMP_InputField>().interactable = true;
        FullHouseInput.GetComponent<TMP_InputField>().interactable = true;
        FlushInput.GetComponent<TMP_InputField>().interactable = true;
        StraightInput.GetComponent<TMP_InputField>().interactable = true;
        ThreeOfAKindInput.GetComponent<TMP_InputField>().interactable = true;
        TwoPairInput.GetComponent<TMP_InputField>().interactable = true;
        PairInput.GetComponent<TMP_InputField>().interactable = true;
        HighCardInput.GetComponent<TMP_InputField>().interactable = true;

        ValidateInput();

        GlobalScript.ShuffleCards(deck, 1, true);

        MoneyText.text = $"Money: ${GlobalScript.money}";

        elapsedTime = 0;

        state = "Betting";
    }

    // Update is called once per frame
    void Update() {
        switch (state) {
            case "Betting":
                ValidateInput();

                break;

            case "StartGame":
                BetButton.interactable = false;

                BetInputPlayerWins.transform.GetChild(0).GetComponent<TMP_Text>().color = new Color32(255, 255, 255, 255);
                BetInputDraw.transform.GetChild(0).GetComponent<TMP_Text>().color = new Color32(255, 255, 255, 255);
                BetInputDealerWins.transform.GetChild(0).GetComponent<TMP_Text>().color = new Color32(255, 255, 255, 255);
                FourOfAKindOrHigherInput.transform.GetChild(0).GetComponent<TMP_Text>().color = new Color32(255, 255, 255, 255);
                FullHouseInput.transform.GetChild(0).GetComponent<TMP_Text>().color = new Color32(255, 255, 255, 255);
                FlushInput.transform.GetChild(0).GetComponent<TMP_Text>().color = new Color32(255, 255, 255, 255);
                StraightInput.transform.GetChild(0).GetComponent<TMP_Text>().color = new Color32(255, 255, 255, 255);
                ThreeOfAKindInput.transform.GetChild(0).GetComponent<TMP_Text>().color = new Color32(255, 255, 255, 255);
                TwoPairInput.transform.GetChild(0).GetComponent<TMP_Text>().color = new Color32(255, 255, 255, 255);
                PairInput.transform.GetChild(0).GetComponent<TMP_Text>().color = new Color32(255, 255, 255, 255);
                HighCardInput.transform.GetChild(0).GetComponent<TMP_Text>().color = new Color32(255, 255, 255, 255);

                Card nextCard = GlobalScript.Pop(deck);

                for (int i = 0; i < playerCards.Length; i++) {
                    nextCard = GlobalScript.Pop(deck);

                    playerCards[i] = nextCard;
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

                try {
                    //GlobalScript.UpdateMoney("minus", GlobalScript.ReturnNum(BetInputPlayerWins.GetComponent<TMP_InputField>().text), GlobalScript.ReturnNum(BetInputDraw.GetComponent<TMP_InputField>().text), GlobalScript.ReturnNum(BetInputDealerWins.GetComponent<TMP_InputField>().text), GlobalScript.ReturnNum(FourOfAKindOrHigherInput.GetComponent<TMP_InputField>().text), GlobalScript.ReturnNum(FullHouseInput.GetComponent<TMP_InputField>().text), GlobalScript.ReturnNum(FlushInput.GetComponent<TMP_InputField>().text), GlobalScript.ReturnNum(StraightInput.GetComponent<TMP_InputField>().text), GlobalScript.ReturnNum(ThreeOfAKindInput.GetComponent<TMP_InputField>().text), GlobalScript.ReturnNum(TwoPairInput.GetComponent<TMP_InputField>().text), GlobalScript.ReturnNum(PairInput.GetComponent<TMP_InputField>().text), GlobalScript.ReturnNum(HighCardInput.GetComponent<TMP_InputField>().text));
                    
                    if (winner == "Tie") {
                        GlobalScript.UpdateMoney("plus", 0, GlobalScript.ReturnNum(BetInputDraw.GetComponent<TMP_InputField>().text) * 24M, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                        //GlobalScript.UpdateMoney("plus", GlobalScript.ReturnNum(BetInputDraw.GetComponent<TMP_InputField>().text) * 24M);
                        BetInputDraw.transform.GetChild(0).GetComponent<TMP_Text>().color = new Color32(128, 0, 0, 255);
                        
                        state = "Reset";
                    } else { 
                        if (winner == "Player") {
                            GlobalScript.UpdateMoney("plus", GlobalScript.ReturnNum(BetInputPlayerWins.GetComponent<TMP_InputField>().text) * 1.98M, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                            //GlobalScript.UpdateMoney("plus", GlobalScript.ReturnNum(BetInputPlayerWins.GetComponent<TMP_InputField>().text) * 1.98M);
                            BetInputPlayerWins.transform.GetChild(0).GetComponent<TMP_Text>().color = new Color32(128, 0, 0, 255);
                        }   else {
                            GlobalScript.UpdateMoney("plus", 0, 0, GlobalScript.ReturnNum(BetInputDealerWins.GetComponent<TMP_InputField>().text) * 1.98M, 0, 0, 0, 0, 0, 0, 0, 0);
                            //GlobalScript.UpdateMoney("plus", GlobalScript.ReturnNum(BetInputDealerWins.GetComponent<TMP_InputField>().text) * 1.98M);
                            BetInputDealerWins.transform.GetChild(0).GetComponent<TMP_Text>().color = new Color32(128, 0, 0, 255);
                        }

                        //GlobalScript.UpdateMoney("plus", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

                        if (bestHand[0] <= 3) {
                            GlobalScript.UpdateMoney("plus", 0, 0, 0, GlobalScript.ReturnNum(FourOfAKindOrHigherInput.GetComponent<TMP_InputField>().text) * handPayouts[bestHand[0]], 0, 0, 0, 0, 0, 0, 0);
                            //GlobalScript.UpdateMoney("plus", GlobalScript.ReturnNum(FourOfAKindOrHigherInput.GetComponent<TMP_InputField>().text) * handPayouts[bestHand[0]]);
                            FourOfAKindOrHigherInput.transform.GetChild(0).GetComponent<TMP_Text>().color = new Color32(128, 0, 0, 255);
                        } else if (bestHand[0] == 4) {
                            GlobalScript.UpdateMoney("plus", 0, 0, 0, 0, GlobalScript.ReturnNum(FullHouseInput.GetComponent<TMP_InputField>().text) * handPayouts[bestHand[0]], 0, 0, 0, 0, 0, 0);
                            //GlobalScript.UpdateMoney("plus", GlobalScript.ReturnNum(FullHouseInput.GetComponent<TMP_InputField>().text) * handPayouts[bestHand[0]]);
                            FullHouseInput.transform.GetChild(0).GetComponent<TMP_Text>().color = new Color32(128, 0, 0, 255);
                        } else if (bestHand[0] == 5) {
                            GlobalScript.UpdateMoney("plus", 0, 0, 0, 0, 0, GlobalScript.ReturnNum(FlushInput.GetComponent<TMP_InputField>().text) * handPayouts[bestHand[0]], 0, 0, 0, 0, 0);
                            //GlobalScript.UpdateMoney("plus", GlobalScript.ReturnNum(FlushInput.GetComponent<TMP_InputField>().text) * handPayouts[bestHand[0]]);
                            FlushInput.transform.GetChild(0).GetComponent<TMP_Text>().color = new Color32(128, 0, 0, 255);
                        } else if (bestHand[0] == 6) {
                            GlobalScript.UpdateMoney("plus", 0, 0, 0, 0, 0, 0, GlobalScript.ReturnNum(StraightInput.GetComponent<TMP_InputField>().text) * handPayouts[bestHand[0]], 0, 0, 0, 0);
                            //GlobalScript.UpdateMoney("plus", GlobalScript.ReturnNum(StraightInput.GetComponent<TMP_InputField>().text) * handPayouts[bestHand[0]]);
                            StraightInput.transform.GetChild(0).GetComponent<TMP_Text>().color = new Color32(128, 0, 0, 255);
                        } else if (bestHand[0] == 7) {
                            GlobalScript.UpdateMoney("plus", 0, 0, 0, 0, 0, 0, 0, GlobalScript.ReturnNum(ThreeOfAKindInput.GetComponent<TMP_InputField>().text) * handPayouts[bestHand[0]], 0, 0, 0);
                            //GlobalScript.UpdateMoney("plus", GlobalScript.ReturnNum(ThreeOfAKindInput.GetComponent<TMP_InputField>().text) * handPayouts[bestHand[0]]);
                            ThreeOfAKindInput.transform.GetChild(0).GetComponent<TMP_Text>().color = new Color32(128, 0, 0, 255);
                        } else if (bestHand[0] == 8) {
                            GlobalScript.UpdateMoney("plus", 0, 0, 0, 0, 0, 0, 0, 0, GlobalScript.ReturnNum(TwoPairInput.GetComponent<TMP_InputField>().text) * handPayouts[bestHand[0]], 0, 0);
                            //GlobalScript.UpdateMoney("plus", GlobalScript.ReturnNum(TwoPairInput.GetComponent<TMP_InputField>().text) * handPayouts[bestHand[0]]);
                            TwoPairInput.transform.GetChild(0).GetComponent<TMP_Text>().color = new Color32(128, 0, 0, 255);
                        } else if (bestHand[0] == 9) {
                            GlobalScript.UpdateMoney("plus", 0, 0, 0, 0, 0, 0, 0, 0, 0, GlobalScript.ReturnNum(PairInput.GetComponent<TMP_InputField>().text) * handPayouts[bestHand[0]], 0);
                            //GlobalScript.UpdateMoney("plus", GlobalScript.ReturnNum(PairInput.GetComponent<TMP_InputField>().text) * handPayouts[bestHand[0]]);
                            PairInput.transform.GetChild(0).GetComponent<TMP_Text>().color = new Color32(128, 0, 0, 255);
                        } else if (bestHand[0] == 10) {
                            GlobalScript.UpdateMoney("plus", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, GlobalScript.ReturnNum(HighCardInput.GetComponent<TMP_InputField>().text) * handPayouts[bestHand[0]]);
                            //GlobalScript.UpdateMoney("plus", GlobalScript.ReturnNum(HighCardInput.GetComponent<TMP_InputField>().text) * handPayouts[bestHand[0]]);
                            HighCardInput.transform.GetChild(0).GetComponent<TMP_Text>().color = new Color32(128, 0, 0, 255);
                        }
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
