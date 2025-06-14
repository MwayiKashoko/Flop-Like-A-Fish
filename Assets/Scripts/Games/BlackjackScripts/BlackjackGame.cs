using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;

public class BlackjackGame : MonoBehaviour {
    /* NOT CORRECT
    *268 //NOT CORRECT? blackjack just stays???
    *700 //NOT CORRECT? king and jack not able to be split?
    * //NOT CORRECT? Too much money being added on wins
    * //21 on 2nd split hand has incorrect animation
    * //Blackjack on split is weird
    * //not showing correct text for each hand
    * //player text value not properly computed on split
    * //Sometimes the game goes on without any user input (dealer starts dealing?)
    * //some other weird stuff
    //Add animation to clear cards from screen when bet button is pressed
    //I think I'm done after this?
    */

    public Sprite[] cardSprites;

    private string state = "Betting";

    private List<List<Card>> playerCards;
    private List<List<Image>> playerCardImages;
    private List<Card> dealerCards;
    private List<Image> dealerCardImages;

    private List<int> playerValues;
    private List<int> aceCountPlayer;
    private int aceCountDealer = 0;
    private int dealerValue = 0;
    private int currentHand = 0;
    private bool movingCard = false;
    private bool doubledDown = false;
    private bool firstRotate = true;

    private bool splitCardAnimation1 = false;
    private bool splitCardAnimation2 = false;
    private bool splitCardAnimation3 = false;
    private bool splitCardAnimation4 = false;
    private bool splitCardAnimation5 = false;
    private bool splitCardAnimation6 = false;
    private bool switchHandAnimation = false;
    private bool hit21First = false;

    private bool getFirstHand = false;
    private bool getNextHand = false;
    private bool blackjackHit = false;

    private decimal totalBet = 0;

    private List<List<GameObject>> playerGameObjects;
    private List<GameObject> dealerGameObjects;

    private TMP_Text MoneyText;
    private TMP_Text DealerValueText;
    private TMP_Text PlayerValueText;
    private TMP_Text Text21_3;
    private TMP_Text PerfectPairText;

    private TMP_InputField BetInput;
    private TMP_InputField BetInput21_3;
    private TMP_InputField PerfectPairInput;
    private Button BackButton;
    private Button BetButton;
    private Transform BackOfCardImage5;

    private Button HitButton;
    private Button DoubleButton;
    private Button StandButton;
    private Button SplitButton;
    private Button InsureButton;
    private Button HelpButton;

    private GameObject canvas;

    private int cardsDealt = 0;

    private float elapsedTime = 0;
    private float desiredAnimationTime = 0.375f;
    private float percentageComplete = 0;
    private bool canRotateCard = false;

    private Vector3 cardStartPosition;

    private Vector3 cardEndPositionOriginal = new Vector3(-150, -350, 0);
    private Vector3 cardEndPositionPlayer = new Vector3(-150, -350, 0);
    private Vector3 cardEndPositionDealer = new Vector3(-150, 100, 0);

    private Vector3 splitCardFirstPositionFirstCardOriginal = new Vector3(200, -350, 0);
    private Vector3 splitCardFirstPositionFirstCard = new Vector3(200, -350, 0);
    private Vector3 splitCardFirstPositionSecondCard = new Vector3(250, -325, 0);

    // Start is called before the first frame update
    void Start() {
        playerCards = new List<List<Card>>();
        playerCardImages = new List<List<Image>>();
        dealerCards = new List<Card>();
        dealerCardImages = new List<Image>();

        playerValues = new List<int>();
        aceCountPlayer = new List<int>();

        playerGameObjects = new List<List<GameObject>>();
        dealerGameObjects = new List<GameObject>();

        MoneyText = GameObject.Find("MoneyText").GetComponent<TMP_Text>();
        DealerValueText = GameObject.Find("DealerValueText").GetComponent<TMP_Text>();
        PlayerValueText = GameObject.Find("PlayerValueText").GetComponent<TMP_Text>();
        Text21_3 = GameObject.Find("21+3Text").GetComponent<TMP_Text>();
        PerfectPairText = GameObject.Find("PerfectPairText").GetComponent<TMP_Text>();

        BetInput = GameObject.Find("BetInput").GetComponent<TMP_InputField>();
        BetInput21_3 = GameObject.Find("BetInput21_3").GetComponent<TMP_InputField>();
        PerfectPairInput = GameObject.Find("PerfectPairInput").GetComponent<TMP_InputField>();
        BackButton = GameObject.Find("BackButton").GetComponent<Button>();
        BetButton = GameObject.Find("BetButton").GetComponent<Button>();
        BackOfCardImage5 = GameObject.Find("BackOfCardImage5").GetComponent<Transform>();

        HitButton = GameObject.Find("HitButton").GetComponent<Button>();
        DoubleButton = GameObject.Find("DoubleButton").GetComponent<Button>();
        StandButton = GameObject.Find("StandButton").GetComponent<Button>();
        SplitButton = GameObject.Find("SplitButton").GetComponent<Button>();
        InsureButton = GameObject.Find("InsureButton").GetComponent<Button>();
        HelpButton = GameObject.Find("HelpButton").GetComponent<Button>();

        canvas = GameObject.Find("Canvas");

        cardStartPosition = new Vector3(BackOfCardImage5.transform.localPosition.x, BackOfCardImage5.transform.localPosition.y, 0);

        GlobalScript.deck.ChangeCardRankings("Blackjack");

        MoneyText.text = $"Money: ${GlobalScript.money}";
    }

    void ValidateInput() {
        GlobalScript.ValidateInput(BetInput, BetButton);
        GlobalScript.ValidateInput(BetInput21_3, BetButton);
        GlobalScript.ValidateInput(PerfectPairInput, BetButton);

        if (BetButton.interactable == false || GlobalScript.ReturnNum(BetInput.text) + GlobalScript.ReturnNum(BetInput21_3.text) + GlobalScript.ReturnNum(PerfectPairInput.text) > GlobalScript.money) {
            BetButton.interactable = false;
        }
    }

    public void Bet() {
        if (state == "Betting") {
            playerCards.Clear();
            dealerCards.Clear();

            for (int i = 0; i < playerCardImages.Count; i++) {
                GlobalScript.RemoveCards(playerCardImages[i], playerGameObjects[i]);
            }

            playerCardImages.Clear();
            playerGameObjects.Clear();

            GlobalScript.RemoveCards(dealerCardImages, dealerGameObjects);

            DealerValueText.text = "0";
            PlayerValueText.text = "0";

            PlayerValueText.color = new Color32(255, 255, 255, 255);
            DealerValueText.color = new Color32(255, 255, 255, 255);
            Text21_3.color = new Color32(255, 255, 255, 255);
            PerfectPairText.color = new Color32(255, 255, 255, 255);

            BetInput.interactable = false;
            BetInput21_3.interactable = false;
            PerfectPairInput.interactable = false;
            BackButton.interactable = false;
            BetButton.interactable = false;
            HelpButton.interactable = false;

            GlobalScript.RoundInput(BetInput);
            GlobalScript.RoundInput(BetInput21_3);
            GlobalScript.RoundInput(PerfectPairInput);

            GlobalScript.UpdateMoney("minus", GlobalScript.ReturnNum(BetInput.text), GlobalScript.ReturnNum(BetInput21_3.text), GlobalScript.ReturnNum(PerfectPairInput.text));

            MoneyText.text = $"Money: ${GlobalScript.money}";

            state = "Setup";

            InsureButton.interactable = false;
        }
    }

    public void Hit() {
        if (state == "Playing") {
            if (playerValues[currentHand] < 21 && !movingCard && !canRotateCard) {
                if (InsureButton.interactable && dealerCards[0].rankInt == 10) {
                    state = "Dealer";
                    elapsedTime = 0;
                    canRotateCard = true;
                    DetermineDealerValue();
                    return;
                }

                Card card = GlobalScript.deck.deck[GlobalScript.random.Next(0, 52)].Clone();

                card.onBack = true;

                playerCards[currentHand].Add(card);

                if (card.rankInt == 11) {
                    aceCountPlayer[currentHand]++;
                }

                GlobalScript.DrawCard(BackOfCardImage5.transform.localPosition.x, BackOfCardImage5.transform.localPosition.y, card.width, card.height, card, canvas.transform, playerCardImages[currentHand], playerGameObjects[currentHand], cardSprites);

                movingCard = true;

                HitButton.interactable = false;
                StandButton.interactable = false;
                InsureButton.interactable = false;
                DoubleButton.interactable = false;
                SplitButton.interactable = false; 

                elapsedTime = 0;
            }
        }
    }

    public void Double() {
        if (state == "Playing") {
            if (playerValues[currentHand] < 21 && !movingCard && !canRotateCard) {
                if (InsureButton.interactable && dealerCards[0].rankInt == 10) {
                    state = "Dealer";
                    elapsedTime = 0;
                    canRotateCard = true;
                    DetermineDealerValue();
                    return;
                }

                if (GlobalScript.money >= totalBet) {
                    totalBet *= 2;

                    GlobalScript.UpdateMoney("minus", GlobalScript.ReturnNum(BetInput.text));

                    MoneyText.text = $"Money: ${GlobalScript.money}";
                }

                Card card = GlobalScript.deck.deck[GlobalScript.random.Next(0, 52)].Clone();

                card.onBack = true;

                playerCards[currentHand].Add(card);

                if (card.rankInt == 11) {
                    aceCountPlayer[currentHand]++;
                }

                GlobalScript.DrawCard(BackOfCardImage5.transform.localPosition.x, BackOfCardImage5.transform.localPosition.y, card.width, card.height, card, canvas.transform, playerCardImages[currentHand], playerGameObjects[currentHand], cardSprites);

                movingCard = true;

                doubledDown = true;

                HitButton.interactable = false;
                StandButton.interactable = false;
                InsureButton.interactable = false;
                DoubleButton.interactable = false;
                SplitButton.interactable = false;

                elapsedTime = 0;
            }
        }
    }

    public void Stand() {
        if (state == "Playing") {
            if (InsureButton.interactable && dealerCards[0].rankInt == 10) {
                //NOT CORRECT? blackjack just stays???
                state = "Dealer";
                elapsedTime = 0;
                canRotateCard = true;
                DetermineDealerValue();
                return;
            }

            currentHand++;

            HitButton.interactable = false;
            StandButton.interactable = false;
            InsureButton.interactable = false;
            DoubleButton.interactable = false;
            SplitButton.interactable = false;

            switchHandAnimation = true;

            elapsedTime = 0;
        }
    }

    public void Split() {
        if (state == "Playing") {
            if (InsureButton.interactable && dealerCards[0].rankInt == 10) {
                state = "Dealer";
                elapsedTime = 0;
                canRotateCard = true;
                DetermineDealerValue();
                return;
            }

            playerCards.Add(new List<Card>());
            playerCards.Last().Add(GlobalScript.Pop(playerCards[currentHand]));
            playerValues.Add(playerCards.Last()[0].rankInt);

            playerCardImages.Add(new List<Image>());
            playerCardImages.Last().Add(GlobalScript.Pop(playerCardImages[currentHand]));
            playerGameObjects.Add(new List<GameObject>());
            playerGameObjects.Last().Add(GlobalScript.Pop(playerGameObjects[currentHand]));
            aceCountPlayer.Add(0);

            if (GlobalScript.money >= totalBet) {
                totalBet += GlobalScript.ReturnNum(BetInput.text);

                GlobalScript.UpdateMoney("minus", GlobalScript.ReturnNum(BetInput.text));

                MoneyText.text = $"Money: ${GlobalScript.money}";
            }

            //First iteration of adding card
            Card card = GlobalScript.deck.deck[GlobalScript.random.Next(0, 52)].Clone();
            //Card card = GlobalScript.deck.deck[9].Clone();

            card.onBack = true;

            playerCards[currentHand].Add(card);

            if (card.rankInt == 11) {
                aceCountPlayer[currentHand]++;
            }

            GlobalScript.DrawCard(BackOfCardImage5.transform.localPosition.x, BackOfCardImage5.transform.localPosition.y, card.width, card.height, card, canvas.transform, playerCardImages[currentHand], playerGameObjects[currentHand], cardSprites);

            //Adding new card to 2nd split card at the end of the list
            card = GlobalScript.deck.deck[GlobalScript.random.Next(0, 52)].Clone();
            //card = GlobalScript.deck.deck[0].Clone();

            card.onBack = true;

            if (card.rankInt == 11) {
                aceCountPlayer[aceCountPlayer.Count-1]++;
            }

            playerCards.Last().Add(card);

            GlobalScript.DrawCard(BackOfCardImage5.transform.localPosition.x, BackOfCardImage5.transform.localPosition.y, card.width, card.height, card, canvas.transform, playerCardImages.Last(), playerGameObjects.Last(), cardSprites);

            elapsedTime = 0;

            splitCardAnimation1 = true;

            playerValues[playerValues.Count-1] += playerCards.Last()[1].rankInt;

            if (playerValues.Last() > 21) {
                playerValues[playerValues.Count-1] -= 10;
            }

            HitButton.interactable = false;
            StandButton.interactable = false;
            InsureButton.interactable = false;
            DoubleButton.interactable = false;
            SplitButton.interactable = false;

            playerValues[currentHand] = playerCards[currentHand][0].rankInt + playerCards[currentHand][1].rankInt;
            
            if (playerValues[currentHand] == 21) {
                hit21First = true;
                canRotateCard = true;
                elapsedTime = 0;
            }

            cardEndPositionPlayer = new Vector3(-100, -325, 0);
        }
    }

    public void Insure() {
        if (state == "Playing") {
            if (dealerCards[0].rankInt == 10) {
                GlobalScript.UpdateMoney("plus", GlobalScript.ReturnNum(BetInput.text));
                state = "Dealer";
                elapsedTime = 0;
                canRotateCard = true;
            } else {
                totalBet = Math.Round(GlobalScript.ReturnNum(BetInput.text) / 2M, 2);
                GlobalScript.UpdateMoney("minus", Math.Round(GlobalScript.ReturnNum(BetInput.text) / 2M, 2));
            }

            elapsedTime = 0;

            InsureButton.interactable = false;

            MoneyText.text = $"Money: ${GlobalScript.money}";
        }
    }

    IEnumerator WaitForResults() {
        PlayerValueText.color = new Color32(255, 255, 255, 255);
        DealerValueText.color = new Color32(255, 255, 255, 255);

        //Winning cases
        if ((dealerValue > 21 && playerValues[currentHand] <= 21) || (playerValues[currentHand] > dealerValue && playerValues[currentHand] <= 21 && dealerValue <= 21)) {
            GlobalScript.UpdateMoney("plus", 2M * GlobalScript.ReturnNum(BetInput.text));

            PlayerValueText.color = new Color32(0, 0, 128, 255);
            DealerValueText.color = new Color32(128, 0, 0, 255);
        } else if (playerValues[currentHand] == dealerValue && playerValues[currentHand] <= 21) {
            GlobalScript.UpdateMoney("plus", GlobalScript.ReturnNum(BetInput.text));

            PlayerValueText.color = new Color32(128, 128, 0, 255);
            DealerValueText.color = new Color32(128, 128, 0, 255);
        } else {
            PlayerValueText.color = new Color32(128, 0, 0, 255);
            DealerValueText.color = new Color32(0, 0, 128, 255);
        }

        try {
            PlayerValueText.text = $"{playerValues[currentHand]}";
        } catch (Exception) {

        }

        yield return new WaitForSecondsRealtime(1.5f);

        getNextHand = true;
        elapsedTime = 0;
        currentHand++;
    }

    public void DetermineDealerValue() {
        dealerValue = 0;

        int aceCount = 0;

        for (int i = 0; i < dealerCards.Count; i++) {
            dealerValue += dealerCards[i].rankInt;

            if (dealerCards[i].rankInt == 11) {
                aceCount++;
            }
        }

        for (int i = 0; i < aceCount; i++) {
            if (dealerValue > 21) {
                dealerValue -= 10;
            } else {
                break;
            }
        }

        DealerValueText.text = $"{dealerValue}";
    }

    public void ResetGame() {
        MoneyText.text = $"Money: ${GlobalScript.money}";

        playerValues.Clear();
        aceCountPlayer.Clear();
        aceCountDealer = 0;
        dealerValue = 0;
        currentHand = 0;
        movingCard = false;
        doubledDown = false;
        firstRotate = true;

        splitCardAnimation1 = false;
        splitCardAnimation2 = false;
        splitCardAnimation3 = false;
        splitCardAnimation4 = false;
        splitCardAnimation5 = false;
        splitCardAnimation6 = false;
        switchHandAnimation = false;
        hit21First = false;

        getFirstHand = false;
        getNextHand = false;
        blackjackHit = false;
        
        totalBet = 0;

        cardsDealt = 0;
        elapsedTime = 0;
        percentageComplete = 0;
        canRotateCard = false;

        cardEndPositionPlayer = new Vector3(-150, -350, 0);
        cardEndPositionDealer = new Vector3(-150, 100, 0);
        splitCardFirstPositionFirstCard = new Vector3(200, -350, 0);
        splitCardFirstPositionSecondCard = new Vector3(250, -325, 0);
        
        BetInput.interactable = true;
        BetInput21_3.interactable = true;
        PerfectPairInput.interactable = true;
        BackButton.interactable = true;
        BetButton.interactable = true;
        HelpButton.interactable = true;

        HitButton.interactable = false;
        DoubleButton.interactable = false;
        StandButton.interactable = false;
        SplitButton.interactable = false;
        InsureButton.interactable = false;

        currentHand = 0;
        movingCard = false;
        doubledDown = false;

        cardsDealt = 0;

        elapsedTime = 0;

        canRotateCard = false;

        splitCardAnimation1 = false;

        cardEndPositionPlayer = new Vector3(-150, -350, 0);
        cardEndPositionDealer = new Vector3(-150, 100, 0);

        totalBet = 0;

        state = "Betting";
    }

    // Update is called once per frame
    void Update() {
        switch (state) {
            case "Betting":
                ValidateInput();

                break;

            case "Setup":
                playerCards.Add(new List<Card>());
                playerCardImages.Add(new List<Image>());
                playerGameObjects.Add(new List<GameObject>());
                playerValues.Add(0);
                aceCountPlayer.Add(0);

                state = "Deal Cards";

                break;

            case "Deal Cards":
                totalBet = GlobalScript.ReturnNum(BetInput.text);

                if (cardsDealt < 4) {
                    Card nextCard = GlobalScript.deck.deck[GlobalScript.random.Next(0, 52)].Clone();
                    nextCard.onBack = true;

                    //Card nextCard1 = GlobalScript.deck.deck[0].Clone();
                    //Card nextCard2 = GlobalScript.deck.deck[10].Clone();

                    //nextCard1.onBack = true;
                    //nextCard2.onBack = true;

                    /*Card card1 = GlobalScript.deck.deck[0].Clone();
                    Card card2 = GlobalScript.deck.deck[2].Clone();
                    Card card3 = GlobalScript.deck.deck[0].Clone();
                    Card card4 = GlobalScript.deck.deck[3].Clone();

                    card1.onBack = true;
                    card2.onBack = true;
                    card3.onBack = true;
                    card4.onBack = true;*/

                    if (cardsDealt % 2 == 0) {
                        playerCards[0].Add(nextCard);
                        GlobalScript.DrawCard(BackOfCardImage5.transform.localPosition.x, BackOfCardImage5.transform.localPosition.y, nextCard.width, nextCard.height, nextCard, canvas.transform, playerCardImages[0], playerGameObjects[0], cardSprites);
                        
                        if (nextCard.rankInt == 11) {
                            aceCountPlayer[0]++;
                        }

                        /*if (cardsDealt/2 == 1) {
                            playerCards[0].Add(nextCard1);
                            GlobalScript.DrawCard(BackOfCardImage5.transform.localPosition.x, BackOfCardImage5.transform.localPosition.y, nextCard1.width, nextCard1.height, nextCard1, canvas.transform, playerCardImages[0], playerGameObjects[0], cardSprites);
                        } else {
                            playerCards[0].Add(nextCard2);
                            GlobalScript.DrawCard(BackOfCardImage5.transform.localPosition.x, BackOfCardImage5.transform.localPosition.y, nextCard2.width, nextCard2.height, nextCard2, canvas.transform, playerCardImages[0], playerGameObjects[0], cardSprites);
                        }*/
                    } else {
                        dealerCards.Add(nextCard);
                        GlobalScript.DrawCard(BackOfCardImage5.transform.localPosition.x, BackOfCardImage5.transform.localPosition.y, nextCard.width, nextCard.height, nextCard, canvas.transform, dealerCardImages, dealerGameObjects, cardSprites);

                        if (nextCard.rankInt == 11) {
                            aceCountDealer++;
                        }

                        /*if ((cardsDealt-1)/2 == 1) {
                            dealerCards.Add(nextCard1);
                            GlobalScript.DrawCard(BackOfCardImage5.transform.localPosition.x, BackOfCardImage5.transform.localPosition.y, nextCard1.width, nextCard1.height, nextCard1, canvas.transform, dealerCardImages, dealerGameObjects[0], cardSprites);
                        } else {
                            dealerCards.Add(nextCard2);
                            GlobalScript.DrawCard(BackOfCardImage5.transform.localPosition.x, BackOfCardImage5.transform.localPosition.y, nextCard2.width, nextCard2.height, nextCard2, canvas.transform, dealerCardImages, dealerGameObjects[0], cardSprites);
                        }*/
                    }

                    /*playerCards[0].Add(card1);
                    playerCards[0].Add(card3);

                    dealerCards.Add(card2);
                    dealerCards.Add(card4);

                    if (card1.rankInt == 11) {
                        aceCountPlayer[0]++;
                    }

                    if (card3.rankInt == 11) {
                        aceCountPlayer[0]++;
                    }

                    if (card2.rankInt == 1) {
                        aceCountDealer++;
                    }

                    if (card4.rankInt == 1) {
                        aceCountDealer++;
                    }

                    GlobalScript.DrawCard(BackOfCardImage5.transform.localPosition.x, BackOfCardImage5.transform.localPosition.y, card1.width, card1.height, card1, canvas.transform, playerCardImages[0], playerGameObjects[0], cardSprites);
                    GlobalScript.DrawCard(BackOfCardImage5.transform.localPosition.x, BackOfCardImage5.transform.localPosition.y, card3.width, card3.height, card3, canvas.transform, playerCardImages[0], playerGameObjects[0], cardSprites);
                    GlobalScript.DrawCard(BackOfCardImage5.transform.localPosition.x, BackOfCardImage5.transform.localPosition.y, card2.width, card2.height, card2, canvas.transform, dealerCardImages, dealerGameObjects, cardSprites);
                    GlobalScript.DrawCard(BackOfCardImage5.transform.localPosition.x, BackOfCardImage5.transform.localPosition.y, card4.width, card4.height, card4, canvas.transform, dealerCardImages, dealerGameObjects, cardSprites);

                    cardsDealt = 500;*/

                    cardsDealt++;
                } else {
                    cardsDealt = 0;
                    state = "Move Cards";
                }

                break;

            case "Move Cards":
                elapsedTime += Time.deltaTime;
                percentageComplete = elapsedTime / desiredAnimationTime;

                if (cardsDealt < 4) {
                    if (cardsDealt % 2 == 0) {
                        if (canRotateCard) {
                            GlobalScript.FlipCardOver(percentageComplete, playerGameObjects[0][cardsDealt/2], cardSprites, playerCards[0][cardsDealt/2].pos);
                        } else {
                            GlobalScript.MoveCard(cardStartPosition, cardEndPositionPlayer, percentageComplete, playerGameObjects[0][cardsDealt/2]);
                        }
                    } else {
                        if (canRotateCard && cardsDealt != 1) {
                            GlobalScript.FlipCardOver(percentageComplete, dealerGameObjects[(cardsDealt-1)/2], cardSprites, dealerCards[(cardsDealt-1)/2].pos);
                        } else {
                            GlobalScript.MoveCard(cardStartPosition, cardEndPositionDealer, percentageComplete, dealerGameObjects[(cardsDealt-1)/2]);
                        }
                    }

                    if (percentageComplete >= 1) {
                        elapsedTime = 0;

                        if (canRotateCard) {
                            if (cardsDealt % 2 == 0) {
                                cardEndPositionPlayer.x += 50f;
                                cardEndPositionPlayer.y += 25f;
                            } else {
                                cardEndPositionDealer.x += 50f;
                                cardEndPositionDealer.y += 25f;
                            }

                            cardsDealt++;
                            canRotateCard = false;
                        } else {
                            if (cardsDealt % 2 == 0) {
                                playerValues[0] += playerCards[0][cardsDealt/2].rankInt;

                                if (playerValues[0] > 21) {
                                    playerValues[0] -= 10;
                                }

                                PlayerValueText.text = $"{playerValues[0]}";
                            } else if (cardsDealt > 1) {
                                dealerValue += dealerCards[(cardsDealt-1)/2].rankInt;

                                if (dealerValue > 21) {
                                    dealerValue -= 10;
                                }

                                DealerValueText.text = $"{dealerValue}";
                            }

                            canRotateCard = true;
                        }
                    }
                } else {
                    elapsedTime = 0;
                    state = "Determine Game";
                }

                break;

            case "Determine Game":
                state = "Playing";

                string value21_3 = GlobalScript.Determine21_3(playerCards[0][0], dealerCards[1], playerCards[0][1]);
                string perfectPair = GlobalScript.DeterminePerfectPair(playerCards[0][0], playerCards[0][1]);

                decimal bet21_3 = GlobalScript.ReturnNum(BetInput21_3.text);
                decimal betPerfectPair = GlobalScript.ReturnNum(PerfectPairInput.text);

                if (value21_3 != "None") {
                    Text21_3.color = new Color32(0, 0, 128, 255);
                }

                if (perfectPair != "None") {
                    PerfectPairText.color = new Color32(0, 0, 128, 255);
                }

                if (value21_3 == "Suited Three of a Kind") {
                    GlobalScript.UpdateMoney("plus", bet21_3 * 101);
                } else if (value21_3 == "Straight Flush") {
                    GlobalScript.UpdateMoney("plus", bet21_3 * 41);
                } else if (value21_3 == "Three of a Kind") {
                    GlobalScript.UpdateMoney("plus", bet21_3 * 31);
                } else if (value21_3 == "Straight") {
                    GlobalScript.UpdateMoney("plus", bet21_3 * 11);
                } else if (value21_3 == "Flush") {
                    GlobalScript.UpdateMoney("plus", bet21_3 * 6);
                }

                if (perfectPair == "Perfect Pair") {
                    GlobalScript.UpdateMoney("plus", betPerfectPair * 31);
                } else if (perfectPair == "Colored Pair") {
                    GlobalScript.UpdateMoney("plus", betPerfectPair * 11);
                } else if (perfectPair == "Mixed Pair") {
                    GlobalScript.UpdateMoney("plus", betPerfectPair * 6);
                }

                if (playerValues[0] == 21 && dealerValue == 21) {
                    GlobalScript.UpdateMoney("plus", GlobalScript.ReturnNum(BetInput.text));
                    
                    state = "Dealer";
                } else if (playerValues[0] == 21) {
                    blackjackHit = true;
                    GlobalScript.UpdateMoney("plus", GlobalScript.ReturnNum(BetInput.text) * 2.5M);

                    state = "Dealer";
                } else if (dealerCards[0].rankInt + dealerCards[1].rankInt == 21 && dealerCards[0].rankInt == 11) {
                    dealerValue = 21;
                    DealerValueText.text = $"{dealerValue}";
                    state = "Dealer";
                }

                MoneyText.text = $"Money: ${GlobalScript.money}";

                HitButton.interactable = true;
                DoubleButton.interactable = true;
                StandButton.interactable = true;

                if (playerCards[0][0].rankInt == playerCards[0][1].rankInt) {
                    //NOT CORRECT? king and jack not able to be split?
                    SplitButton.interactable = true;
                }

                if (dealerCards[1].rankInt == 11) {
                    InsureButton.interactable = true;
                }

                if (state == "Dealer") {
                    HitButton.interactable = false;
                    DoubleButton.interactable = false;
                    StandButton.interactable = false;
                    SplitButton.interactable = false;
                    InsureButton.interactable = false;

                    dealerValue = dealerCards[0].rankInt + dealerCards[1].rankInt;

                    if (dealerValue > 21) {
                        dealerValue -= 10;
                    }

                    DealerValueText.text = $"{dealerValue}";

                    elapsedTime = 0;

                    canRotateCard = true;
                }

                break;

            case "Playing":
                elapsedTime += Time.deltaTime;

                percentageComplete = elapsedTime / desiredAnimationTime;

                if (movingCard) {
                    GlobalScript.MoveCard(cardStartPosition, cardEndPositionPlayer, percentageComplete, playerGameObjects[currentHand].Last());

                    if (percentageComplete >= 1) {
                        elapsedTime = 0;

                        movingCard = false;

                        canRotateCard = true;
                    }
                } else if (splitCardAnimation1) {
                    GlobalScript.MoveCard(cardEndPositionPlayer, splitCardFirstPositionFirstCard, percentageComplete, playerGameObjects.Last()[0]);

                    if (percentageComplete >= 1) {
                        elapsedTime = 0;

                        splitCardAnimation1 = false;

                        splitCardAnimation2 = true;
                    }
                } else if (splitCardAnimation2) {
                    GlobalScript.MoveCard(BackOfCardImage5.transform.localPosition, cardEndPositionPlayer, percentageComplete, playerGameObjects[currentHand][1]);

                    if (percentageComplete >= 1) {
                        elapsedTime = 0;

                        splitCardAnimation2 = false;

                        splitCardAnimation3 = true;
                    }
                } else if (splitCardAnimation3) {
                    GlobalScript.FlipCardOver(percentageComplete, playerGameObjects[currentHand][1], cardSprites, playerCards[currentHand][1].pos);

                    if (percentageComplete >= 1) {
                        elapsedTime = 0;

                        splitCardAnimation3 = false;

                        splitCardAnimation4 = true;
                    }
                } else if (splitCardAnimation4) {
                    GlobalScript.MoveCard(BackOfCardImage5.transform.localPosition, splitCardFirstPositionSecondCard, percentageComplete, playerGameObjects.Last()[1]);

                    if (percentageComplete >= 1) {
                        elapsedTime = 0;

                        splitCardAnimation4 = false;

                        splitCardAnimation5 = true;

                        playerValues[currentHand] = playerCards[currentHand][0].rankInt + playerCards[currentHand][1].rankInt;
                        playerValues[playerValues.Count-1] = playerCards.Last()[0].rankInt + playerCards.Last()[1].rankInt;

                        if (playerValues[currentHand] > 21) {
                            playerValues[currentHand] -= 10;
                        }

                        if (playerValues.Last() > 21) {
                            playerValues[playerValues.Count-1] -= 10;
                        }

                        PlayerValueText.text = $"{playerValues[currentHand]}";
                    }
                } else if (splitCardAnimation5) {
                    GlobalScript.FlipCardOver(percentageComplete, playerGameObjects.Last()[1], cardSprites, playerCards.Last()[1].pos);

                    if (percentageComplete >= 1) {
                        elapsedTime = 0;

                        splitCardAnimation5 = false;

                        splitCardAnimation6 = true;

                        cardEndPositionPlayer = new Vector3(-50, -300, 0);
                    }
                } else if (splitCardAnimation6) {
                    GlobalScript.MoveCard(splitCardFirstPositionFirstCard, new Vector3(0, splitCardFirstPositionFirstCard.y-500, 0), percentageComplete, playerGameObjects.Last()[0]);
                    GlobalScript.MoveCard(splitCardFirstPositionSecondCard, new Vector3(0, splitCardFirstPositionFirstCard.y-500, 0), percentageComplete, playerGameObjects.Last()[1]);

                    if (percentageComplete >= 1) {
                        elapsedTime = 0;

                        splitCardAnimation6 = false;

                        try {
                            /*if (playerCards[currentHand][0].rankInt + playerCards[currentHand][1].rankInt == 21) {
                                playerValues[currentHand] = 21;
                                currentHand++;
                            }*/

                            HitButton.interactable = true;
                            StandButton.interactable = true;

                            if (playerCards[currentHand].Count == 2) {
                                if (playerCards[currentHand][0].rankInt == playerCards[currentHand][1].rankInt) {
                                    SplitButton.interactable = true;
                                }
                            }

                            PlayerValueText.text = $"{playerValues[currentHand]}";
                        } catch (Exception) {

                        }

                        HitButton.interactable = true;
                        StandButton.interactable = true;
                    }
                } else if (switchHandAnimation) {
                    for (int i = 0; i < playerGameObjects[currentHand-1].Count; i++) {
                        GlobalScript.MoveCard(new Vector3(cardEndPositionOriginal.x + (float) i * 50f, cardEndPositionOriginal.y + (float) i * 25f, 0), new Vector3(0, splitCardFirstPositionFirstCardOriginal.y-500, 0), percentageComplete, playerGameObjects[currentHand-1][i]);
                    }

                    try {
                        GlobalScript.MoveCard(new Vector3(0, splitCardFirstPositionFirstCardOriginal.y-500, 0), new Vector3(cardEndPositionOriginal.x, cardEndPositionOriginal.y, 0), percentageComplete, playerGameObjects[currentHand][0]);
                        GlobalScript.MoveCard(new Vector3(0, splitCardFirstPositionFirstCardOriginal.y-500, 0), new Vector3(cardEndPositionOriginal.x + 50f, cardEndPositionOriginal.y + 50f, 0), percentageComplete, playerGameObjects[currentHand][1]);
                    } catch (Exception) {

                    }

                    if (percentageComplete >= 1) {
                        elapsedTime = 0;

                        switchHandAnimation = false;

                        try {
                            if (playerCards[currentHand][0].rankInt + playerCards[currentHand][1].rankInt == 21) {
                                playerValues[currentHand] = 21;
                                currentHand++;
                            }

                            HitButton.interactable = true;
                            StandButton.interactable = true;

                            if (playerCards[currentHand].Count == 2) {
                                if (playerCards[currentHand][0].rankInt == playerCards[currentHand][1].rankInt) {
                                    SplitButton.interactable = true;
                                }
                            }

                            PlayerValueText.text = $"{playerValues[currentHand]}";
                        } catch (Exception) {

                        }
                    }
                } else if (canRotateCard) {
                    if (!hit21First) {
                        GlobalScript.FlipCardOver(percentageComplete, playerGameObjects[currentHand].Last(), cardSprites, playerCards[currentHand].Last().pos);

                        playerValues[currentHand] += playerCards[currentHand].Last().rankInt;
                    } 

                    if (percentageComplete >= 1) {
                        elapsedTime = 0;

                        canRotateCard = false;

                        HitButton.interactable = true;
                        StandButton.interactable = true;

                        cardEndPositionPlayer.x += 50f;
                        cardEndPositionPlayer.y += 25f;

                        if (playerCards[currentHand].Count == 2 && playerValues[currentHand] > 21 && playerCards[currentHand].Last().rankInt == 11) {
                            playerValues[currentHand] -= 10;
                        } else if (playerCards[currentHand].Count > 2 && playerValues[currentHand] > 21) {
                            playerValues[currentHand] = 0;

                            int aceCount = 0;

                            for (int i = 0; i < playerCards[currentHand].Count; i++) {
                                playerValues[currentHand] += playerCards[currentHand][i].rankInt;

                                if (playerCards[currentHand][i].rankInt == 11) {
                                    aceCount++;
                                }
                            }

                            for (int i = 0; i < aceCount; i++) {
                                if (playerValues[currentHand] > 21) {
                                    playerValues[currentHand] -= 10;
                                } else {
                                    break;
                                }
                            }
                        }

                        PlayerValueText.text = $"{playerValues[currentHand]}";

                        if (playerValues[currentHand] >= 21 || doubledDown) {
                            currentHand++;

                            HitButton.interactable = false;
                            StandButton.interactable = false;
                            InsureButton.interactable = false;
                            DoubleButton.interactable = false;
                            SplitButton.interactable = false;

                            try {
                                if (playerCards[currentHand].Count == 2) {
                                    PlayerValueText.text = $"{playerValues[currentHand]}";
                                }
                            } catch (Exception) {

                            }

                            switchHandAnimation = true;

                            doubledDown = false;

                            hit21First = false;
                        }
                    }
                }

                if (currentHand >= playerCards.Count) {
                    state = "Dealer";

                    HitButton.interactable = false;
                    DoubleButton.interactable = false;
                    StandButton.interactable = false;
                    SplitButton.interactable = false;
                    InsureButton.interactable = false;

                    dealerValue = dealerCards[0].rankInt + dealerCards[1].rankInt;

                    if (dealerValue > 21) {
                        dealerValue -= 10;
                    }

                    DealerValueText.text = $"{dealerValue}";

                    elapsedTime = 0;

                    canRotateCard = true;
                }

                break;

            case "Dealer":
                HitButton.interactable = false;
                DoubleButton.interactable = false;
                StandButton.interactable = false;
                SplitButton.interactable = false;
                InsureButton.interactable = false;

                elapsedTime += Time.deltaTime;

                percentageComplete = elapsedTime / desiredAnimationTime;

                if (movingCard) {
                    GlobalScript.MoveCard(cardStartPosition, cardEndPositionDealer, percentageComplete, dealerGameObjects.Last());

                    if (percentageComplete >= 1) {
                        cardEndPositionDealer.x += 50;
                        cardEndPositionDealer.y += 25;

                        elapsedTime = 0;
                        percentageComplete = 0;

                        movingCard = false;

                        canRotateCard = true;

                        dealerValue += dealerCards.Last().rankInt;

                        if (dealerValue > 21) {
                            dealerValue = 0;

                            int aceCount = 0;

                            for (int i = 0; i < dealerCards.Count; i++) {
                                dealerValue += dealerCards[i].rankInt;

                                if (dealerCards[i].rankInt == 11) {
                                    aceCount++;
                                }
                            }

                            for (int i = 0; i < aceCount; i++) {
                                if (dealerValue > 21) {
                                    dealerValue -= 10;
                                } else {
                                    break;
                                }
                            }
                        }

                        DealerValueText.text = $"{dealerValue}";
                    }
                } if (canRotateCard) {
                    if (firstRotate) {
                        GlobalScript.FlipCardOver(percentageComplete, dealerGameObjects[0], cardSprites, dealerCards[0].pos);
                    } else {
                        GlobalScript.FlipCardOver(percentageComplete, dealerGameObjects.Last(), cardSprites, dealerCards.Last().pos);
                    }

                    if (percentageComplete >= 1) {
                        firstRotate = false;

                        elapsedTime = 0;
                        
                        canRotateCard = false;

                        bool allBust = true;

                        for (int i = 0; i < playerValues.Count; i++) {
                            if (playerValues[i] < 21) {
                                allBust = false;
                            }
                        }

                        if (dealerValue < 17 && !allBust) {
                            Card card = GlobalScript.deck.deck[GlobalScript.random.Next(0, 52)].Clone();

                            card.onBack = true;

                            dealerCards.Add(card);

                            if (card.rankInt == 11) {
                                aceCountDealer++;
                            }

                            GlobalScript.DrawCard(BackOfCardImage5.transform.localPosition.x, BackOfCardImage5.transform.localPosition.y, card.width, card.height, card, canvas.transform, dealerCardImages, dealerGameObjects, cardSprites);

                            movingCard = true;
                        } else {
                            //This is actually if the dealer bust or got 21
                            state = "Determine Winner";

                            currentHand = 0;

                            getFirstHand = true;
                            elapsedTime = 0;
                        }
                    }
                }

                break;

            case "Determine Winner":
                if (playerValues.Count > 1) {
                    /*for (int i = 0; i < playerValues.Count; i++) {
                        //Winning cases
                        if ((dealerValue > 21 && playerValues[i] <= 21) || (playerValues[i] > dealerValue && playerValues[i] <= 21 && dealerValue <= 21)) {
                            GlobalScript.money += GlobalScript.ReturnNum(BetInput.text);
                        }
                    }*/

                    elapsedTime += Time.deltaTime;

                    percentageComplete = elapsedTime / desiredAnimationTime;

                    try {
                        if (getFirstHand) {
                            for (int i = 0; i < playerCards.Last().Count; i++) {
                                GlobalScript.MoveCard(new Vector3(cardEndPositionOriginal.x + (float) i * 50f, cardEndPositionOriginal.y + (float) i * 25f, 0), new Vector3(0, splitCardFirstPositionFirstCardOriginal.y-500, 0), percentageComplete, playerGameObjects.Last()[i]);
                            }

                            for (int i = 0; i < playerCards[0].Count; i++) {
                                GlobalScript.MoveCard(new Vector3(0, splitCardFirstPositionFirstCardOriginal.y-500, 0), new Vector3(cardEndPositionOriginal.x + (float) i * 50f, cardEndPositionOriginal.y + (float) i * 25f, 0), percentageComplete, playerGameObjects[0][i]);

                            }

                            if (percentageComplete >= 1) {
                                elapsedTime = 0;
                                getFirstHand = false;
                                
                                StartCoroutine("WaitForResults");
                            }
                        } else if (getNextHand) {
                            for (int i = 0; i < playerCards[currentHand-1].Count; i++) {
                                GlobalScript.MoveCard(new Vector3(cardEndPositionOriginal.x + (float) i * 50f, cardEndPositionOriginal.y + (float) i * 25f, 0), new Vector3(0, splitCardFirstPositionFirstCardOriginal.y-500, 0), percentageComplete, playerGameObjects[currentHand-1][i]);
                            }

                            for (int i = 0; i < playerCards[currentHand].Count; i++) {
                                GlobalScript.MoveCard(new Vector3(0, splitCardFirstPositionFirstCardOriginal.y-500, 0), new Vector3(cardEndPositionOriginal.x + (float) i * 50f, cardEndPositionOriginal.y + (float) i * 25f, 0), percentageComplete, playerGameObjects[currentHand][i]);

                            }

                            if (percentageComplete >= 1) {
                                elapsedTime = 0;
                                getNextHand = false;
                                
                                StartCoroutine("WaitForResults");
                            }
                        }
                    } catch(Exception) {
                        state = "Reset";
                    }
                } else {
                    //Winning cases
                    if ((dealerValue > 21 && playerValues[0] <= 21) || (playerValues[0] > dealerValue && playerValues[0] <= 21 && dealerValue <= 21)) {
                        if (!blackjackHit) {
                            if (totalBet > GlobalScript.ReturnNum(BetInput.text)) {
                                GlobalScript.UpdateMoney("plus", 4M * GlobalScript.ReturnNum(BetInput.text));
                            } else if (totalBet < GlobalScript.ReturnNum(BetInput.text)) {
                                GlobalScript.UpdateMoney("plus", GlobalScript.ReturnNum(BetInput.text));
                            } else {
                                GlobalScript.UpdateMoney("plus", 2M * GlobalScript.ReturnNum(BetInput.text));
                            }
                        }

                        PlayerValueText.color = new Color32(0, 0, 128, 255);
                        DealerValueText.color = new Color32(128, 0, 0, 255);
                    } else if (playerValues[0] == dealerValue && playerValues[0] <= 21) {
                        GlobalScript.UpdateMoney("plus", GlobalScript.ReturnNum(BetInput.text));

                        PlayerValueText.color = new Color32(128, 128, 0, 255);
                        DealerValueText.color = new Color32(128, 128, 0, 255);
                    } else {
                        PlayerValueText.color = new Color32(128, 0, 0, 255);
                        DealerValueText.color = new Color32(0, 0, 128, 255);
                    }

                    state = "Reset";
                }

                break;

            case "Reset":
                ResetGame();

                break;
        }
    }
}
