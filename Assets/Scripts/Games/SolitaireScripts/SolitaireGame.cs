using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;

public class SolitaireGame : MonoBehaviour {
    public Sprite[] spriteList;
    public Sprite cardOutline;

    private List<Card> deck;

    private List<Card> stockCards;
    private List<Card> wasteCards;
    private List<List<Card>> tableauCards;
    private List<List<Card>> foudationCards;

    private List<GameObject> stockCardsGameObject;
    private List<GameObject> wasteCardsGameObject;
    private List<List<GameObject>> tableauCardsGameObject;
    private List<List<GameObject>> foundationCardsGameObject;

    private List<Image> stockCardsImage;
    private List<Image> wasteCardsImage;
    private List<List<Image>> tableauCardsImage;

    private int tableaus = 7;
    private int tableauPositionI = 0;
    private int tableauPositionJ = 0;

    //Formula to calculate triangular numbers
    private int tableauCardNumber;

    private string state = "Betting";

    private bool draw3Cards = false;
    private bool vegasGame = false;

    private int cardsDealt = 0;
    private int cardsPlaced = 0;

    private float elapsedTime = 0;
    private float desiredAnimationTime = 0.025f;
    private float desiredRotateAnimationTime = 0.25f;
    private float percentageComplete = 0;
    private bool canRotateCard = false;

    private int[] cardPositionX = {-700, -450, -200, 50, 300, 550, 800}; 

    private Button BackButton;
    private TMP_Text MoneyText;
    private TMP_Text ScoreText;
    private GameObject BetButton;
    private TMP_InputField BetInput;
    private TMP_Dropdown GameDropdown;
    private GameObject StockImage;
    private GameObject WasteImage1;
    private GameObject WasteImage2;
    private GameObject WasteImage3;
    private GameObject canvas;

    // Start is called before the first frame update
    void Start() {
        BackButton = GameObject.Find("BackButton").GetComponent<Button>();
        MoneyText = GameObject.Find("MoneyText").GetComponent<TMP_Text>();
        ScoreText = GameObject.Find("ScoreText").GetComponent<TMP_Text>();
        BetButton = GameObject.Find("BetButton");
        BetInput = GameObject.Find("BetInput").GetComponent<TMP_InputField>();
        GameDropdown = GameObject.Find("GameDropdown").GetComponent<TMP_Dropdown>();
        StockImage = GameObject.Find("StockImage");
        WasteImage1 = GameObject.Find("WasteImage1");
        WasteImage2 = GameObject.Find("WasteImage2");
        WasteImage3 = GameObject.Find("WasteImage3");
        canvas = GameObject.Find("Canvas");

        WasteImage1.SetActive(false);
        WasteImage2.SetActive(false);
        WasteImage3.SetActive(false);

        GlobalScript.deck.ChangeCardRankings("Solitaire");

        MoneyText.text = $"Money: ${GlobalScript.money}";

        deck = new List<Card>();

        stockCards = new List<Card>();
        wasteCards = new List<Card>();
        tableauCards = new List<List<Card>>();
        foudationCards = new List<List<Card>>();

        stockCardsGameObject = new List<GameObject>();
        wasteCardsGameObject = new List<GameObject>();
        tableauCardsGameObject = new List<List<GameObject>>();
        foundationCardsGameObject = new List<List<GameObject>>();

        stockCardsImage = new List<Image>();
        wasteCardsImage = new List<Image>();
        tableauCardsImage = new List<List<Image>>();

        tableauCardNumber = ((tableaus)*(tableaus+1))/2;

        for (int i = 0; i < tableaus; i++) {
            tableauCards.Add(new List<Card>());
            tableauCardsGameObject.Add(new List<GameObject>());
            tableauCardsImage.Add(new List<Image>());

            if (i < 4) {
                foudationCards.Add(new List<Card>());
                foundationCardsGameObject.Add(new List<GameObject>());
            }
        }

        GlobalScript.ShuffleCards(deck, 1, true);
    }

    public void Bet() {
        if (state == "Betting") {
            state = "Setup";
        } else if (state == "Playing") {

        }
    }

    public void GetNewCards() {
        if (state == "Playing") {
            if (stockCards.Count > 0) {
                if (draw3Cards) {
                    if (stockCards.Count >= 3) {
                        for (int i = 0; i < 3; i++) {
                            wasteCards.Add(GlobalScript.Pop(stockCards));
                            wasteCardsGameObject.Add(GlobalScript.Pop(stockCardsGameObject));
                            wasteCardsImage.Add(GlobalScript.Pop(stockCardsImage));
                        }
                    } else {
                        for (int i = 0; i < stockCards.Count; i++) {
                            wasteCards.Add(GlobalScript.Pop(stockCards));
                            wasteCardsGameObject.Add(GlobalScript.Pop(stockCardsGameObject));
                            wasteCardsImage.Add(GlobalScript.Pop(stockCardsImage));
                        }
                    }
                } else {
                    wasteCards.Add(GlobalScript.Pop(stockCards));
                    wasteCardsGameObject.Add(GlobalScript.Pop(stockCardsGameObject));
                    wasteCardsImage.Add(GlobalScript.Pop(stockCardsImage));
                }

                if (!draw3Cards) {
                    WasteImage1.SetActive(true);
                    WasteImage1.GetComponent<Image>().sprite = spriteList[wasteCards.Last().pos];
                    WasteImage1.transform.name = wasteCardsGameObject.Last().transform.name;

                    if (WasteImage1.GetComponent<Drag>() == null) {
                        WasteImage1.AddComponent<Drag>();
                    }
                } else {
                    if (wasteCards.Count >= 3) {
                        WasteImage3.SetActive(true);
                        WasteImage3.GetComponent<Image>().sprite = spriteList[wasteCards[wasteCards.Count-3].pos];
                        WasteImage3.AddComponent<Drag>();
                        WasteImage3.transform.name = wasteCardsGameObject[wasteCardsGameObject.Count-3].transform.name;
                    }

                    if (wasteCards.Count >= 2) {
                        WasteImage2.SetActive(true);
                        WasteImage2.GetComponent<Image>().sprite = spriteList[wasteCards[wasteCards.Count-2].pos];
                        WasteImage2.transform.name = wasteCardsGameObject[wasteCardsGameObject.Count-2].transform.name;
                    }

                    WasteImage1.SetActive(true);
                    WasteImage1.GetComponent<Image>().sprite = spriteList[wasteCards[wasteCards.Count-1].pos];
                    WasteImage1.transform.name = wasteCardsGameObject.Last().transform.name;
                   
                    if (wasteCards.Count == 2) {
                        WasteImage2.AddComponent<Drag>();
                    } else if (wasteCards.Count == 1) {
                        WasteImage1.AddComponent<Drag>();
                    }
                }
            }

            if (stockCards.Count == 0) {
                if (StockImage.GetComponent<Image>().sprite == cardOutline) {
                    stockCards = GlobalScript.CloneList(wasteCards);
                    stockCardsGameObject = GlobalScript.CloneList(wasteCardsGameObject);
                    stockCardsImage = GlobalScript.CloneList(wasteCardsImage);

                    wasteCards.Clear();
                    wasteCardsGameObject.Clear();
                    wasteCardsImage.Clear();

                    StockImage.GetComponent<Image>().sprite = spriteList.Last();
                    
                    WasteImage1.SetActive(false);
                    WasteImage2.SetActive(false);
                    WasteImage3.SetActive(false);

                    return;
                }

                StockImage.GetComponent<Image>().sprite = cardOutline;
            }
        }
    }

    public Vector3 GetTableauPosition(int i, int j) {
        return new Vector3(i*250-700, 150 - j*20, 0);
    }

    // Update is called once per frame
    void Update() {
        switch (state) {
            case "Betting":
                GlobalScript.ValidateInput(BetInput, BetButton.GetComponent<Button>());
                break;

            case "Setup":
                GlobalScript.RoundInput(BetInput);

                GlobalScript.money -= GlobalScript.ReturnNum(BetInput.text);

                MoneyText.text = $"Money: ${GlobalScript.money}";

                BackButton.interactable = false;
                BetInput.interactable = false;
                
                BetButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "Cashout";

                GameDropdown.interactable = false;

                if (GameDropdown.value%2 == 1) {
                    draw3Cards = true;
                }

                if (GameDropdown.value >= 2) {
                    vegasGame = true;
                }

                for (int i = 0; i < tableaus; i++) {
                    for (int j = i; j < tableaus; j++) {
                        tableauCards[j].Add(GlobalScript.Pop(deck));

                        tableauCards[j].Last().onBack = true;

                        GlobalScript.DrawCard(StockImage.transform.localPosition.x, StockImage.transform.localPosition.y, tableauCards[j].Last().width, tableauCards[j].Last().height, tableauCards[j].Last(), canvas.transform, tableauCardsImage[j], tableauCardsGameObject[j], spriteList);
                        tableauCardsGameObject[j].Last().transform.localScale = new Vector3(0.75f, 0.75f, 1);
                        tableauCardsGameObject[j].Last().transform.name = $"{tableauCards[j].Last().suit}{tableauCards[j].Last().color}{tableauCards[j].Last().rankInt}";
                    }
                }

                state = "Dealing";

                break;

            case "Dealing":
                elapsedTime += Time.deltaTime;
                percentageComplete = elapsedTime / desiredAnimationTime;

                if (canRotateCard) {
                    GlobalScript.FlipCardOver(percentageComplete, tableauCardsGameObject[tableauPositionI][tableauPositionJ], spriteList, tableauCards[tableauPositionI][tableauPositionJ].pos);

                    if (percentageComplete >= 1) {
                        tableauPositionJ++;

                        if (tableauPositionJ >= tableauCards[tableauPositionI].Count) {
                            tableauPositionI++;
                            tableauPositionJ = 0;
                        }

                        elapsedTime = 0;
                        canRotateCard = false;

                        cardsDealt++;

                        if (cardsDealt >= tableauCardNumber) {
                            for (int i = 0; i < tableauCards.Count; i++) {
                                tableauCardsGameObject[i].Last().AddComponent<Drag>();
                                tableauCardsGameObject[i].Last().AddComponent<Drop>();
                            }

                            stockCards = deck;

                            for (int i = 0; i < stockCards.Count; i++) {
                                stockCards[i].onBack = true;

                                GlobalScript.DrawCard(-1000, -1000, stockCards[i].width, stockCards[i].height, stockCards[i], canvas.transform, stockCardsImage, stockCardsGameObject, spriteList);

                                stockCardsGameObject[i].transform.localScale = new Vector3(0.75f, 0.75f, 1);
                                stockCardsGameObject[i].transform.name = $"{stockCards[i].suit}{stockCards[i].color}{stockCards[i].rankInt}Waste";
                            }

                            state = "Playing";
                        }
                    }
                } else {
                    GlobalScript.MoveCard(StockImage.transform.localPosition, GetTableauPosition(tableauPositionI, tableauPositionJ), percentageComplete, tableauCardsGameObject[tableauPositionI][tableauPositionJ]);

                    if (percentageComplete >= 1) {
                        elapsedTime = 0;

                        if (tableauPositionJ >= tableauCards[tableauPositionI].Count-1) {
                            canRotateCard = true;
                        } else {
                            tableauPositionJ++;

                            if (tableauPositionJ >= tableauCards[tableauPositionI].Count) {
                                tableauPositionI++;
                                tableauPositionJ = 0;
                            }

                            cardsDealt++;

                            if (cardsDealt >= tableauCardNumber) {
                                state = "Playing";
                            }
                        }
                    }
                }

                break;
            
            case "Playing":
                if (wasteCards.Count > 0 && (WasteImage3.GetComponent<Image>().sprite == null || WasteImage2.GetComponent<Image>().sprite == null || WasteImage1.GetComponent<Image>().sprite == null)) {
                    Destroy(wasteCardsGameObject.Last());
                    Destroy(wasteCardsImage.Last());

                    GlobalScript.Pop(wasteCards);
                    GlobalScript.Pop(wasteCardsGameObject);
                    GlobalScript.Pop(wasteCardsImage);

                    if (draw3Cards) {
                        if (wasteCards.Count > 0) {
                            
                        }
                    } else {
                        if (wasteCards.Count > 0 && WasteImage1.GetComponent<Drag>() == null) {
                            WasteImage1.GetComponent<Image>().sprite = spriteList[wasteCards.Last().pos];
                            WasteImage1.AddComponent<Drag>();
                            WasteImage1.transform.name = wasteCardsGameObject.Last().transform.name;
                        }
                    }
                } else if (wasteCards.Count == 0) {
                    WasteImage1.SetActive(false);
                }

                for (int i = 0; i < tableaus; i++) {
                    if (tableauCardsGameObject[i].Last().GetComponent<Image>().sprite == null) {
                        Destroy(tableauCardsGameObject[i].Last());
                        Destroy(tableauCardsImage[i].Last());

                        GlobalScript.Pop(tableauCards[i]);
                        GlobalScript.Pop(tableauCardsGameObject[i]);
                        GlobalScript.Pop(tableauCardsImage[i]);
                    } else if (tableauCards[i].Last().onBack) {
                        tableauCards[i].Last().onBack = false;

                        tableauCardsGameObject[i].Last().GetComponent<Image>().sprite = spriteList[tableauCards[i].Last().pos];
                    }

                    if (tableauCards[i].Last().rankInt > 1 && tableauCardsGameObject[i].Last().GetComponent<Drop>() == null) {
                        tableauCardsGameObject[i].Last().AddComponent<Drop>();
                    }

                    for (int j = 0; j < tableauCards[i].Count; j++) {
                        if (!tableauCards[i][j].onBack && tableauCardsGameObject[i][j].GetComponent<Drag>() == null) {
                            tableauCardsGameObject[i][j].AddComponent<Drag>();
                        }

                        if (tableauCardsGameObject[i][j].tag == "Moved") {
                            tableauCardsGameObject[i][j].transform.localPosition = new Vector3(tableauCardsGameObject[i][j].transform.localPosition.x, tableauCardsGameObject[i][j].transform.localPosition.y-50, 0);

                            int column = Array.IndexOf(cardPositionX, (int) Math.Round(tableauCardsGameObject[i][j].transform.localPosition.x));

                            tableauCards[column].Add(GlobalScript.Pop(tableauCards[i]));
                            tableauCardsGameObject[column].Add(GlobalScript.Pop(tableauCardsGameObject[i]));
                            tableauCardsImage[column].Add(GlobalScript.Pop(tableauCardsImage[i]));

                            tableauCardsGameObject[column].Last().tag = "Untagged";
                        }
                    }
                }

                break;

            case "Reset":
                break;
        }
    }
}
