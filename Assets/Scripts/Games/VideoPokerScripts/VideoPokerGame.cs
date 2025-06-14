using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;

public class VideoPokerGame : MonoBehaviour {
    public Sprite[] spriteList;

    private string state = "Betting";

    private Regex findNum = new Regex(@"\d+");

    private Button BackButton;
    private TMP_InputField BetInput;
    private GameObject BetButton;
    private TMP_Text MoneyText;

    private GameObject[] cardImages = {null, null, null, null, null};

    private Card[] cards = {null, null, null, null, null};

    private bool[] heldCards = {false, false, false, false, false};

    private List<Card> deck;

    private TMP_Text RoyalFlushPayoutText;
    private TMP_Text StraightFlushPayoutText;
    private TMP_Text FourOfAKindPayoutText;
    private TMP_Text FullHousePayoutText;
    private TMP_Text FlushPayoutText;
    private TMP_Text StraightPayoutText;
    private TMP_Text ThreeOfAKindPayoutText;
    private TMP_Text TwoPairPayoutText;
    private TMP_Text JacksOrBetterPayoutText;
    private Button HelpButton;

    private float elapsedTime = 0;
    private float desiredAnimationTime = 0.5f;
    private float percentageComplete = 0;

    public static Dictionary<string, decimal> handPayouts = new Dictionary<string, decimal> {
        {"Royal Flush", 1000M},
        {"Straight Flush", 200M},
        {"Four of a Kind", 50M},
        {"Full House", 10M},
        {"Flush", 6M},
        {"Straight", 5M},
        {"Three of a Kind", 3M},
        {"Two Pair", 2M},
        {"Pair", 1M}
    };

    private Vector3 sendCardsPosition = new Vector3(-1500, 850, 0);
    private Vector3[] vectorPositions = {new Vector3(-250, -300, 0), new Vector3(25, -300, 0), new Vector3(300, -300, 0), new Vector3(575, -300, 0), new Vector3(850, -300, 0)};

    // Start is called before the first frame update
    void Start() {
        BackButton = GameObject.Find("BackButton").GetComponent<Button>();
        BetInput = GameObject.Find("BetInput").GetComponent<TMP_InputField>();
        BetButton = GameObject.Find("BetButton");
        MoneyText = GameObject.Find("MoneyText").GetComponent<TMP_Text>();
        HelpButton = GameObject.Find("HelpButton").GetComponent<Button>();

        cardImages[0] = GameObject.Find("CardButton1");
        cardImages[1] = GameObject.Find("CardButton2");
        cardImages[2] = GameObject.Find("CardButton3");
        cardImages[3] = GameObject.Find("CardButton4");
        cardImages[4] = GameObject.Find("CardButton5");

        for (int i = 0; i < cardImages.Length; i++) {
            cardImages[i].transform.GetChild(0).gameObject.SetActive(false);
        }

        MoneyText.text = $"Money: ${GlobalScript.money}";

        RoyalFlushPayoutText = GameObject.Find("RoyalFlushPayoutText").GetComponent<TMP_Text>();
        StraightFlushPayoutText = GameObject.Find("StraightFlushPayoutText").GetComponent<TMP_Text>();
        FourOfAKindPayoutText = GameObject.Find("FourOfAKindPayoutText").GetComponent<TMP_Text>();
        FullHousePayoutText = GameObject.Find("FullHousePayoutText").GetComponent<TMP_Text>();
        FlushPayoutText = GameObject.Find("FlushPayoutText").GetComponent<TMP_Text>();
        StraightPayoutText = GameObject.Find("StraightPayoutText").GetComponent<TMP_Text>();
        ThreeOfAKindPayoutText = GameObject.Find("ThreeOfAKindPayoutText").GetComponent<TMP_Text>();
        TwoPairPayoutText = GameObject.Find("TwoPairPayoutText").GetComponent<TMP_Text>();
        JacksOrBetterPayoutText = GameObject.Find("JacksOrBetterPayoutText").GetComponent<TMP_Text>();

        GlobalScript.deck.ChangeCardRankings("Video Poker");

        deck = new List<Card>();

        GlobalScript.ShuffleCards(deck, 1, true);

        /*deck[deck.Count-1] = GlobalScript.deck.deck[1];
        deck[deck.Count-2] = GlobalScript.deck.deck[14];
        deck[deck.Count-3] = GlobalScript.deck.deck[12];
        deck[deck.Count-4] = GlobalScript.deck.deck[20];
        deck[deck.Count-5] = GlobalScript.deck.deck[3];*/
    }

    public void ValidateInput() {
        GlobalScript.ValidateInput(BetInput, BetButton.GetComponent<Button>());
    }

    public void Bet() {
        if (state == "Betting") {
            BetButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "Deal";

            BackButton.interactable = false;
            BetInput.interactable = false;
            HelpButton.interactable = false;
            BetButton.GetComponent<Button>().interactable = false;

            ValidateInput();

            GlobalScript.RoundInput(BetInput);

            GlobalScript.UpdateMoney("minus", GlobalScript.ReturnNum(BetInput.text));

            MoneyText.text = $"Money: ${GlobalScript.money}";

            for (int i = 0; i < cardImages.Length; i++) {
                cardImages[i].transform.localRotation = Quaternion.Euler(0, 180, 0);
                cardImages[i].GetComponent<Image>().sprite = spriteList.Last();
            }

            state = "ChooseCards";
        } else if (state == "Dealing") {
            elapsedTime = 0;
            state = "Replace Cards1";
        }
    }

    public void DetermineSelected(Button button) {
        if (state == "Dealing") {
            heldCards[Convert.ToInt32(findNum.Match((button.name)).Groups[0].Value)-1] = !heldCards[Convert.ToInt32(findNum.Match((button.name)).Groups[0].Value)-1];

            button.transform.GetChild(0).gameObject.SetActive(heldCards[Convert.ToInt32(findNum.Match((button.name)).Groups[0].Value)-1]);
        }
    }

    public void ResetGame() {
        BetButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "Bet";

        BackButton.interactable = true;
        BetInput.interactable = true;
        HelpButton.interactable = true;
        BetButton.GetComponent<Button>().interactable = true;

        elapsedTime = 0;

        ValidateInput();

        GlobalScript.ShuffleCards(deck, 1, true);

        for (int i = 0; i < cards.Length; i++) {
            heldCards[i] = false;

            cardImages[i].transform.GetChild(0).gameObject.SetActive(false);
        }

        MoneyText.text = $"Money: ${GlobalScript.money}";

        state = "Betting";
    }

    // Update is called once per frame
    void Update() {
        switch (state) {
            case "Betting":
                ValidateInput();

                RoyalFlushPayoutText.text = $"${GlobalScript.ReturnNum(BetInput.text) * 1000M}";
                StraightFlushPayoutText.text = $"${GlobalScript.ReturnNum(BetInput.text) * 250M}";
                FourOfAKindPayoutText.text = $"${GlobalScript.ReturnNum(BetInput.text) * 50M}";
                FullHousePayoutText.text = $"${GlobalScript.ReturnNum(BetInput.text) * 10M}";
                FlushPayoutText.text = $"${GlobalScript.ReturnNum(BetInput.text) * 6M}";
                StraightPayoutText.text = $"${GlobalScript.ReturnNum(BetInput.text) * 5M}";
                ThreeOfAKindPayoutText.text = $"${GlobalScript.ReturnNum(BetInput.text) * 3M}";
                TwoPairPayoutText.text = $"${GlobalScript.ReturnNum(BetInput.text) * 2M}";
                JacksOrBetterPayoutText.text = $"${GlobalScript.ReturnNum(BetInput.text)}";

                break;

            case "ChooseCards":
                RoyalFlushPayoutText.color = new Color32(255, 255, 255, 255);
                StraightFlushPayoutText.color = new Color32(255, 255, 255, 255);
                FourOfAKindPayoutText.color = new Color32(255, 255, 255, 255);
                FullHousePayoutText.color = new Color32(255, 255, 255, 255);
                FlushPayoutText.color = new Color32(255, 255, 255, 255);
                StraightPayoutText.color = new Color32(255, 255, 255, 255);
                ThreeOfAKindPayoutText.color = new Color32(255, 255, 255, 255);
                TwoPairPayoutText.color = new Color32(255, 255, 255, 255);
                JacksOrBetterPayoutText.color = new Color32(255, 255, 255, 255);

                for (int i = 0; i < cards.Length; i++) {
                    Card nextCard = GlobalScript.Pop(deck);
                    cards[i] = nextCard;
                    cards[i].onBack = true;
                }

                state = "Animation";

                break;

            case "Animation":
                elapsedTime += Time.deltaTime;
                percentageComplete = elapsedTime / desiredAnimationTime;

                for (int i = 0; i < cards.Length; i++) {
                    GlobalScript.FlipCardOver(percentageComplete, cardImages[i], spriteList, cards[i].pos);
                }

                if (percentageComplete >= 1) {
                    state = "Dealing";
                }

                break;

            case "Replace Cards1":
                if (!heldCards.Any(x => x == false)) {
                    state = "Determine Payout";
                }

                elapsedTime += Time.deltaTime;
                percentageComplete = elapsedTime / (desiredAnimationTime * 2);

                for (int i = 0; i < heldCards.Length; i++) {
                    if (!heldCards[i]) {
                        GlobalScript.MoveCard(cardImages[i].transform.localPosition, sendCardsPosition, percentageComplete, cardImages[i]);
                    }
                }

                if (percentageComplete >= 1) {
                    for (int i = 0; i < heldCards.Length; i++) {
                        if (!heldCards[i]) {
                            Card nextCard = GlobalScript.Pop(deck);
                            cards[i] = nextCard;
                            cardImages[i].GetComponent<Image>().sprite = spriteList[nextCard.pos];
                        }
                    }

                    elapsedTime = 0;
                    state = "Replace Cards2";
                }

                break;

            case "Replace Cards2":
                elapsedTime += Time.deltaTime;
                percentageComplete = elapsedTime / desiredAnimationTime;

                for (int i = 0; i < heldCards.Length; i++) {
                    if (!heldCards[i]) {
                        GlobalScript.MoveCard(sendCardsPosition, vectorPositions[i], percentageComplete, cardImages[i]);
                    }
                }

                if (percentageComplete >= 1) {
                    state = "Determine Payout";
                }

                break;

            case "Determine Payout":
                int[] hand = GlobalScript.DeterminePokerHand("Video Poker", cards, null);
                string handDescription = GlobalScript.handRankings[hand[0]];

                //Debug.Log(hand);
                //Debug.Log(handDescription);

                try {
                    GlobalScript.UpdateMoney("plus", GlobalScript.ReturnNum(BetInput.text) * handPayouts[handDescription]);

                    switch(handDescription) {
                        case "Royal Flush":
                            RoyalFlushPayoutText.color = new Color32(200, 0, 0, 255);
                            break;

                        case "Straight Flush":
                            StraightFlushPayoutText.color = new Color32(200, 0, 0, 255);
                            break;

                        case "Four of a Kind":
                            FourOfAKindPayoutText.color = new Color32(200, 0, 0, 255);
                            break;

                        case "Full House":
                            FullHousePayoutText.color = new Color32(200, 0, 0, 255);
                            break;

                        case "Flush":
                            FlushPayoutText.color = new Color32(200, 0, 0, 255);
                            break;

                        case "Straight":
                            StraightPayoutText.color = new Color32(200, 0, 0, 255);
                            break;

                        case "Three of a Kind":
                            ThreeOfAKindPayoutText.color = new Color32(200, 0, 0, 255);
                            break;

                        case "Two Pair":
                            TwoPairPayoutText.color = new Color32(200, 0, 0, 255);
                            break;

                        case "Pair":
                            if (hand[1] >= 11) {
                                JacksOrBetterPayoutText.color = new Color32(200, 0, 0, 255);
                            } else {
                                GlobalScript.money -= GlobalScript.ReturnNum(BetInput.text) * handPayouts[handDescription];
                            }

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
