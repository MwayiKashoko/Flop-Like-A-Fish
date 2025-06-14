using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BaccaratScript : MonoBehaviour {
    //The player's cards
    private List<Card> playerHand;
    //Player hand value
    private int playerValue = 0;

    //The dealer's cards
    private List<Card> dealerHand;
    //Dealer hand value
    private int dealerValue = 0;

    //All cards
    private List<Card> allCards;

    //List of physical gameobject images that are cards
    private List<Image> images;
    //List of GameObjects that are cards (shows front and goes under back of card image initially)
    private List<GameObject> cardImages;

    private int cardsDealtPlayer = 0;
    private int cardsDealtDealer = 0;

    //Current state the game is at
    private string state = "Betting";

    //String which determines who won
    public string winner = "";

    //Determines if all cards have been dealt
    private bool dealtCards = false;
    //Checks to see if started first animation
    private bool startedWait = false;

    private float elapsedTime = 0;

    private float desiredDurationPlayerCardDealt = 0.5f;
    private float desiredDurationDealerCardDealt = 0.25f;
    private float desiredDurationRotatingCard = 0.3f;

    //Start and target position for each card
    private Vector3 startPosition;
    private Vector3 targetPosition;

    //Determines if we actually are rotating the card
    private bool isRotating = false;

    //Game objects that the player interacts with
    private Transform canvas;
    private TMP_Text BetAmountText;
    private TMP_Text MoneyText;
    private Button BetButton;
    private TMP_InputField PlayerInput;
    private TMP_InputField TieInput;
    private TMP_InputField DealerInput;
    private TMP_Text PlayerValueText;
    private TMP_Text DealerValueText;
    //Back of card coordinates
    private Transform BackOfCard;
    private Button BackButton;
    private Button HelpButton;

    //Contains all the sprites used to draw the cards
    public Sprite[] spriteList;

    // Start is called before the first frame update
    void Start() {
        playerHand = new List<Card>();
        dealerHand = new List<Card>();
        allCards = new List<Card>();
        images = new List<Image>();
        cardImages = new List<GameObject>();

        startPosition = new Vector3(0, 0, 0);
        targetPosition = new Vector3(0, 0, 0);

        canvas = GameObject.Find("Canvas").transform;
        BetAmountText = GameObject.Find("BetAmountText").GetComponent<TMP_Text>();
        MoneyText = GameObject.Find("MoneyText").GetComponent<TMP_Text>();
        BetButton = GameObject.Find("BetButton").GetComponent<Button>();
        PlayerInput = GameObject.Find("PlayerInput").GetComponent<TMP_InputField>();
        TieInput = GameObject.Find("TieInput").GetComponent<TMP_InputField>();
        DealerInput = GameObject.Find("DealerInput").GetComponent<TMP_InputField>();
        PlayerValueText = GameObject.Find("PlayerValueText").GetComponent<TMP_Text>();
        DealerValueText = GameObject.Find("DealerValueText").GetComponent<TMP_Text>();
        BackOfCard = GameObject.Find("BackOfCardImage (4)").transform;
        BackButton = GameObject.Find("BackToGameList").GetComponent<Button>();
        HelpButton = GameObject.Find("HelpButton").GetComponent<Button>();

        MoneyText.text = $"Money: ${GlobalScript.money}";

        PlayerValueText.enabled = false;
        DealerValueText.enabled = false;

        //Changing the values of the cards to match Baccarat
        GlobalScript.deck.ChangeCardRankings("Baccarat");
    }

    //Called when the bet button is pressed which starts the game
    public void Bet() {
        switch (state) {
            case "Betting":
                state = "Dealing";

                GlobalScript.RoundInput(PlayerInput);
                GlobalScript.RoundInput(TieInput);
                GlobalScript.RoundInput(DealerInput);

                break;
        }
    }

    IEnumerator RotateCardWait() {
        elapsedTime = 0;
        yield return new WaitForSecondsRealtime(0.1f);
        isRotating = true;
    }

    //Makes sure that the inputs are valid (only 2 decimal places, non-negative, not more than current amount of money)
    public void ValidateInputs() {
        GlobalScript.ValidateInput(PlayerInput, BetButton);
        GlobalScript.ValidateInput(TieInput, BetButton);
        GlobalScript.ValidateInput(DealerInput, BetButton);

        if (GlobalScript.ReturnNum(PlayerInput.text) + GlobalScript.ReturnNum(TieInput.text) + GlobalScript.ReturnNum(DealerInput.text) > GlobalScript.money || BetButton.interactable == false) {
            BetButton.interactable = false;
        } else {
            BetButton.interactable = true;
        }
    }
    
    //Deals cards from the infinite deck and pauses for a few milliseconds before each deal
    IEnumerator Deal() {
        dealtCards = true;

        float waitTimePlayer = 0.75f;
        float waitTimeDealer = 0.5f;

        while (winner == "") {
            Card card = null;

            if (cardsDealtPlayer < 3) {
                playerHand.Add(GlobalScript.GetNextCard());
                card = playerHand.Last();
                card.onBack = true;
                allCards.Add(card);

                GlobalScript.DrawCard(BackOfCard.localPosition.x, BackOfCard.localPosition.y, 240f, 336f, card, canvas, images, cardImages, spriteList);
                
                playerValue += card.rankInt;
                playerValue %= 10;

                PlayerValueText.text = playerValue.ToString();

                PlayerValueText.enabled = true;

                yield return new WaitForSecondsRealtime(waitTimePlayer);

                elapsedTime = 0f;
                isRotating = false;
                startedWait = false;
            }

            cardsDealtPlayer++;

            //Determining if the dealer can draw their 3rd card
            if (cardsDealtPlayer == 3) {
                switch (dealerValue) {
                    case 3:
                        if (card.rankInt == 8) {
                            cardsDealtDealer++;
                        }

                        break;

                    case 4:
                        if (card.rankInt <= 1 || card.rankInt >= 8) {
                            cardsDealtDealer++;
                        }

                        break;

                    case 5:
                        if (card.rankInt <= 3 || card.rankInt >= 8) {
                                cardsDealtDealer++;
                        }

                        break;

                    case 6:
                        if (card.rankInt <= 5 || card.rankInt >= 8) {
                                cardsDealtDealer++;
                        }

                        break;

                    case 7:
                        cardsDealtDealer++;
                        break;
                }
            }

            if (cardsDealtDealer < 3) {
                dealerHand.Add(GlobalScript.GetNextCard());
                card = dealerHand.Last();
                card.onBack = true;
                allCards.Add(card);
                GlobalScript.DrawCard(BackOfCard.localPosition.x, BackOfCard.localPosition.y, 240f, 336f, card, canvas, images, cardImages, spriteList);

                dealerValue += card.rankInt;
                dealerValue %= 10;

                DealerValueText.text = dealerValue.ToString();

                DealerValueText.enabled = true;

                yield return new WaitForSecondsRealtime(waitTimeDealer);

                elapsedTime = 0f;
                isRotating = false;
                startedWait = false;
            }

            cardsDealtDealer++;

            //Determining when to stand or draw
            if (cardsDealtDealer == 2) {
                if (playerValue >= 8 || dealerValue >= 8) {
                    DetermineWinner();

                    cardsDealtPlayer++;
                    cardsDealtDealer++;
                } else if (playerValue >= 6) {
                    cardsDealtPlayer++;
                }
            } else if (cardsDealtPlayer == 3 || cardsDealtDealer == 3) {
                DetermineWinner();
            }
        }

        state = "Reset";
    }

    //Determining the result of the game
    public void DetermineWinner() {
        if (playerValue > dealerValue) {
            winner = "Player";
        } else if (dealerValue > playerValue) {
            winner = "Dealer";
        } else {
            winner = "Tie";
        }
    }

    //Resets the game to it's beginning state
    public void ResetGame() {
        switch (winner) {
            case "Player":
                GlobalScript.UpdateMoney("plus", GlobalScript.ReturnNum(PlayerInput.text)*2, 0, 0);
                break;
            
            case "Dealer":
                GlobalScript.UpdateMoney("plus", 0, 0, GlobalScript.ReturnNum(DealerInput.text)*1.95M);
                break;

            case "Tie":
                GlobalScript.UpdateMoney("plus", GlobalScript.ReturnNum(PlayerInput.text), GlobalScript.ReturnNum(TieInput.text)*9, GlobalScript.ReturnNum(DealerInput.text));
                break;
        }

        BetAmountText.text = "Bet Amount: $0";
        MoneyText.text = $"Money: ${GlobalScript.money}";

        playerHand.Clear();
        dealerHand.Clear();
        allCards.Clear();
        playerValue = 0;
        dealerValue = 0;
        cardsDealtPlayer = 0;
        cardsDealtDealer = 0;

        state = "Betting";

        BetButton.interactable = true;
        PlayerInput.interactable = true;
        TieInput.interactable = true;
        DealerInput.interactable = true;
        BackButton.interactable = true;
        HelpButton.interactable = true;

        dealtCards = false;

        winner = "";
    }

    // Update is called once per frame
    void Update() {
        switch (state) {
            case "Betting":
                ValidateInputs();
                break;
            case "Dealing":
                //Make the input only to two decimal places
                BetAmountText.text = $"Bet Amount: ${GlobalScript.ReturnNum(PlayerInput.text) + GlobalScript.ReturnNum(TieInput.text) + GlobalScript.ReturnNum(DealerInput.text)}";

                BetButton.interactable = false;
                PlayerInput.interactable = false;
                TieInput.interactable = false;
                DealerInput.interactable = false;
                BackButton.interactable = false;
                HelpButton.interactable = false;

                if (!dealtCards) {
                    PlayerValueText.text = "0";
                    DealerValueText.text = "0";

                    GlobalScript.RemoveCards(images, cardImages);

                    GlobalScript.UpdateMoney("minus", GlobalScript.ReturnNum(PlayerInput.text), GlobalScript.ReturnNum(TieInput.text), GlobalScript.ReturnNum(DealerInput.text));

                    StartCoroutine("Deal");
                } else {
                    Card card = allCards.Last();

                    float x = 0;
                    float y = 0;
                    float percentageComplete = 0;

                    elapsedTime += Time.deltaTime;

                    if (allCards.Count != 5 || playerHand.Count != 2) {
                        if (cardImages.Count % 2 == 1) {
                            x = ((playerHand.Count-1) * 50)-650;
                            y = -((playerHand.Count-1) * 25)-100;
                            percentageComplete = elapsedTime/desiredDurationPlayerCardDealt;
                        } else {
                            x = ((dealerHand.Count-1) * 50)+500;
                            y = -((dealerHand.Count-1) * 25)-100;
                            percentageComplete = elapsedTime/desiredDurationDealerCardDealt;
                        }
                    } else {
                        x = ((dealerHand.Count-1) * 50)+500;
                        y = -((dealerHand.Count-1) * 25)-100;
                        percentageComplete = elapsedTime/desiredDurationDealerCardDealt;
                    }

                    startPosition.x = card.x;
                    startPosition.y = card.y;
                    targetPosition.x = x;
                    targetPosition.y = y;

                    if (!startedWait) {
                        startedWait = GlobalScript.MoveCard(startPosition, targetPosition, percentageComplete, cardImages.Last());

                        if (startedWait) {
                            StartCoroutine("RotateCardWait");
                        }
                    }

                    if (isRotating) {
                        percentageComplete = elapsedTime/desiredDurationRotatingCard;

                        GlobalScript.FlipCardOver(percentageComplete, cardImages.Last(), spriteList, card.pos);
                    }
                }

                MoneyText.text = $"Money: ${GlobalScript.money}";

                break;

            case "Reset":
                ResetGame();
                break;
        }
    }
}
