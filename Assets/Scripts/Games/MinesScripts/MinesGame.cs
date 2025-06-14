using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;

public class MinesGame : MonoBehaviour {
    public Sprite buttonSprite;

    private string state = "Betting";

    private int squaresClicked = 0;

    private decimal multiplier = 1;

    private decimal payout = 0;

    private List<List<GameObject>> mines;
    private List<List<bool>> canClick;

    private Button BackButton;
    private TMP_InputField BetInput;
    private TMP_InputField MinesInput;
    private GameObject BetButton;
    private TMP_Text MoneyText;
    private TMP_Text TotalProfitAmountText;
    private GameObject canvas;
    private Button HelpButton;

    // Start is called before the first frame update
    void Start() {
        BackButton = GameObject.Find("BackButton").GetComponent<Button>();
        BetInput = GameObject.Find("BetInput").GetComponent<TMP_InputField>();;
        MinesInput = GameObject.Find("MinesInput").GetComponent<TMP_InputField>();;
        BetButton = GameObject.Find("BetButton");
        MoneyText = GameObject.Find("MoneyText").GetComponent<TMP_Text>();
        TotalProfitAmountText = GameObject.Find("TotalProfitAmountText").GetComponent<TMP_Text>();
        canvas = GameObject.Find("Canvas");
        HelpButton = GameObject.Find("HelpButton").GetComponent<Button>();

        mines = new List<List<GameObject>>();
        canClick = new List<List<bool>>();

        MoneyText.text = $"Money: ${GlobalScript.money}";

        for (int i = 0; i < 5; i++) {
            mines.Add(new List<GameObject>());
            canClick.Add(new List<bool>());

            for (int j = 0; j < 5; j++) {
                GameObject imgObject = new GameObject("Button");
                mines[i].Add(imgObject);
                canClick[i].Add(true);

                RectTransform trans = imgObject.AddComponent<RectTransform>();
                trans.transform.SetParent(canvas.transform); // setting parent
                trans.localScale = Vector3.one;
                trans.anchoredPosition = new Vector2(j * 175 - 275, i * 175 - 400); // setting position, will be on center
                trans.sizeDelta = new Vector2(150, 150); // custom size

                imgObject.AddComponent<Button>();
                imgObject.transform.SetParent(canvas.transform);

                int x = j;
                int y = i;

                imgObject.GetComponent<Button>().onClick.AddListener(delegate{
                    ClickTile(x, y);
                });

                Image image = imgObject.AddComponent<Image>();
                imgObject.GetComponent<Image>().sprite = buttonSprite;

                image.color = new Color32(128, 128, 128, 255);

                imgObject.transform.SetParent(canvas.transform);
            }
        }
    }

    void ClickTile(int x, int y) {
        if (state == "Playing") {
            if (canClick[y][x]) {
                squaresClicked++;
                mines[y][x].GetComponent<Image>().color = new Color32(0, 255, 0, 255);
                mines[y][x].GetComponent<Button>().interactable = false;

                int numMines = (int) GlobalScript.ReturnNum(MinesInput.text);

                multiplier *= ((decimal) (25 - squaresClicked + 1) / (decimal) (25 - (squaresClicked + numMines) + 1));

                payout = Math.Round(GlobalScript.ReturnNum(BetInput.text) * multiplier * (618M/625M), 2);

                TotalProfitAmountText.text = $"${payout - GlobalScript.ReturnNum(BetInput.text)}";

                if (squaresClicked == 25-numMines) {
                    GlobalScript.UpdateMoney("plus", payout);

                    state = "Reset";
                }
            } else {
                mines[y][x].GetComponent<Image>().color = new Color32(255, 0, 0, 255);

                state = "Reset";
            }
        }
    }

    void ValidateInput() {
        GlobalScript.ValidateInput(BetInput, BetButton.GetComponent<Button>());

        if (GlobalScript.ReturnNum(MinesInput.text) < 1 || GlobalScript.ReturnNum(MinesInput.text) > 24 || BetButton.GetComponent<Button>().interactable == false) {
            BetButton.GetComponent<Button>().interactable = false;
        } else {
            BetButton.GetComponent<Button>().interactable = true;
        }
    }

    public void Bet() {
        if (state == "Betting") {
            BackButton.interactable = false;
            BetInput.interactable = false;
            MinesInput.interactable = false;
            HelpButton.interactable = false;

            BetButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "Cashout";

            GlobalScript.RoundInput(BetInput);

            GlobalScript.UpdateMoney("minus", GlobalScript.ReturnNum(BetInput.text));

            MoneyText.text = $"Money: ${GlobalScript.money}";

            for (int i = 0; i < mines.Count; i++) {
                for (int j = 0; j < mines[0].Count; j++) {
                    mines[i][j].GetComponent<Image>().color = new Color32(128, 128, 128, 255);
                    mines[i][j].GetComponent<Button>().interactable = true;
                    canClick[i][j] = true;
                }
            }

            TotalProfitAmountText.text = "$0";

            state = "Dealing";
        } else if (state == "Playing") {
            GlobalScript.UpdateMoney("plus", payout);

            state = "Reset";
        }
    }

    void ResetGame() {
        BackButton.interactable = true;
        BetInput.interactable = true;
        MinesInput.interactable = true;
        HelpButton.interactable = true;

        MoneyText.text = $"Money: ${GlobalScript.money}";

        squaresClicked = 0;

        multiplier = 1;

        payout = 0;

        BetButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "Bet";

        state = "Betting";
    }

    // Update is called once per frame
    void Update() {
        switch (state) {
            case "Betting":
                ValidateInput();
                break;

            case "Dealing":
                int minesCount = (int) GlobalScript.ReturnNum(MinesInput.text);
                bool setMinesTrue = false;

                if (minesCount > 13) {
                    for (int i = 0; i < canClick.Count; i++) {
                        for (int j = 0; j < canClick[0].Count; j++) {
                            canClick[i][j] = false;
                        }
                    }

                    setMinesTrue = true;

                    minesCount = 25 - minesCount;
                }

                int x;
                int y;

                for (int i = 0; i < minesCount; i++) {
                    x = GlobalScript.random.Next(0, 4);
                    y = GlobalScript.random.Next(0, 4);

                    while (canClick[x][y] == setMinesTrue) {
                        x = GlobalScript.random.Next(0, 4);
                        y = GlobalScript.random.Next(0, 4);
                    }

                    canClick[x][y] = setMinesTrue;
                }

                state = "Playing";

                break;

            case "Reset":
                ResetGame();
                break;
        }
    }
}
