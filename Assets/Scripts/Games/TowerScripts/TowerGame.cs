using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;

public class TowerGame : MonoBehaviour {
    public Sprite buttonSprite;

    private string state = "Betting";

    private string difficulty = "Easy";

    private int currentRow = 0;

    private Dictionary<string, decimal[]> multiplier;

    private List<List<GameObject>> gameButtons2Columns;
    private List<List<bool>> canClick2Columns;

    private List<List<GameObject>> gameButtons3Columns;
    private List<List<bool>> canClick3Columns;

    private List<List<GameObject>> gameButtons4Columns;
    private List<List<bool>> canClick4Columns;

    private Button BackButton;
    private TMP_InputField BetInput;
    private TMP_Dropdown DifficultyDropdown;
    private GameObject BetButton;
    private TMP_Text TotalProfitAmountText;
    private TMP_Text MoneyText;
    private GameObject canvas;
    private Button HelpButton;

    // Start is called before the first frame update
    void Start() {
        BackButton = GameObject.Find("BackButton").GetComponent<Button>();
        BetInput = GameObject.Find("BetInput").GetComponent<TMP_InputField>();
        DifficultyDropdown = GameObject.Find("DifficultyDropdown").GetComponent<TMP_Dropdown>();
        BetButton = GameObject.Find("BetButton");
        TotalProfitAmountText = GameObject.Find("TotalProfitAmountText").GetComponent<TMP_Text>();
        MoneyText = GameObject.Find("MoneyText").GetComponent<TMP_Text>();
        canvas = GameObject.Find("Canvas");
        HelpButton = GameObject.Find("HelpButton").GetComponent<Button>();

        gameButtons2Columns = new List<List<GameObject>>();
        canClick2Columns = new List<List<bool>>();
        
        gameButtons3Columns = new List<List<GameObject>>();
        canClick3Columns = new List<List<bool>>();

        gameButtons4Columns = new List<List<GameObject>>();
        canClick4Columns = new List<List<bool>>();

        MoneyText.text = $"Money: ${GlobalScript.money}";

        for (int i = 0; i < 10; i++) {
            gameButtons2Columns.Add(new List<GameObject>());
            canClick2Columns.Add(new List<bool>());

            gameButtons3Columns.Add(new List<GameObject>());
            canClick3Columns.Add(new List<bool>());

            gameButtons4Columns.Add(new List<GameObject>());
            canClick4Columns.Add(new List<bool>());

            for (int j = 0; j < 2; j++) {
                GameObject imgObject = new GameObject("Button");
                gameButtons2Columns[i].Add(imgObject);
                canClick2Columns[i].Add(false);

                RectTransform trans = imgObject.AddComponent<RectTransform>();
                trans.transform.SetParent(canvas.transform); // setting parent
                trans.localScale = Vector3.one;
                trans.anchoredPosition = new Vector2(j * 425 - 165, i * 90 - 450); // setting position, will be on center
                trans.sizeDelta = new Vector2(400, 75); // custom size

                imgObject.AddComponent<Button>();
                imgObject.transform.SetParent(canvas.transform);

                if (i > 0) {
                    imgObject.GetComponent<Button>().interactable = false;
                }

                int num = j;

                imgObject.GetComponent<Button>().onClick.AddListener(delegate{
                    clickTile(num);
                });

                Image image = imgObject.AddComponent<Image>();
                imgObject.GetComponent<Image>().sprite = buttonSprite;

                image.color = new Color32(128, 128, 128, 255);

                imgObject.transform.SetParent(canvas.transform);

                imgObject.SetActive(false);
            }

            for (int j = 0; j < 3; j++) {
                GameObject imgObject = new GameObject("Button");
                gameButtons3Columns[i].Add(imgObject);
                canClick3Columns[i].Add(false);

                RectTransform trans = imgObject.AddComponent<RectTransform>();
                trans.transform.SetParent(canvas.transform); // setting parent
                trans.localScale = Vector3.one;
                trans.anchoredPosition = new Vector2(j * 292 - 231.5f, i * 90 - 450); // setting position, will be on center
                trans.sizeDelta= new Vector2(267, 75); // custom size

                imgObject.AddComponent<Button>();
                imgObject.transform.SetParent(canvas.transform);

                if (i > 0) {
                    imgObject.GetComponent<Button>().interactable = false;
                }

                int num = j;

                imgObject.GetComponent<Button>().onClick.AddListener(delegate{
                    clickTile(num);
                });

                Image image = imgObject.AddComponent<Image>();
                imgObject.GetComponent<Image>().sprite = buttonSprite;

                image.color = new Color32(128, 128, 128, 255);

                imgObject.transform.SetParent(canvas.transform);

                imgObject.SetActive(false);
            }

            for (int j = 0; j < 4; j++) {
                GameObject imgObject = new GameObject("Button");
                gameButtons4Columns[i].Add(imgObject);
                canClick4Columns[i].Add(false);

                RectTransform trans = imgObject.AddComponent<RectTransform>();
                trans.transform.SetParent(canvas.transform); // setting parent
                trans.localScale = Vector3.one;
                trans.anchoredPosition = new Vector2(j * 225 - 265, i * 90 - 450); // setting position, will be on center
                trans.sizeDelta= new Vector2(200, 75); // custom size

                imgObject.AddComponent<Button>();
                imgObject.transform.SetParent(canvas.transform);

                if (i > 0) {
                    imgObject.GetComponent<Button>().interactable = false;
                }

                int num = j;

                imgObject.GetComponent<Button>().onClick.AddListener(delegate{
                    clickTile(num);
                });

                Image image = imgObject.AddComponent<Image>();
                imgObject.GetComponent<Image>().sprite = buttonSprite;

                image.color = new Color32(128, 128, 128, 255);

                imgObject.transform.SetParent(canvas.transform);
            }
        }

        multiplier = new Dictionary<string, decimal[]> {
            {"Easy", new decimal[] {1.28M, 1.64M, 2.1M, 2.68M, 3.44M, 4.4M, 5.63M, 7.21M, 10M, 15M}},
            {"Medium", new decimal[] {1.44M, 2.07M, 2.99M, 4.3M, 6.19M, 8.92M, 12.84M, 18.49M, 25M, 40M}},
            {"Hard", new decimal[] {1.92M, 3.69M, 7.08M, 13.59M, 26.09M, 50.1M, 96.19M, 184.68M, 256M, 700M}},
            {"Expert", new decimal[] {2.88M, 8.29M, 23.89M, 68.8M, 198.14M, 570.63M, 1643.42M, 4733.04M, 16000M, 60000M}},
            {"Master", new decimal[] {3.84M, 14.75M, 56.62M, 217.43M, 834.94M, 3206.18M, 12311.72M, 47276.99M, 220000M, 1000000M}}
        };
    }

    public void changeDifficultyButtons() {
        difficulty = DifficultyDropdown.options[DifficultyDropdown.value].text;

        switch (difficulty) {
            case "Easy":
                for (int i = 0; i < gameButtons2Columns.Count; i++) {
                    for (int j = 0; j < 2; j++) {
                        gameButtons2Columns[i][j].SetActive(false);
                    }

                    for (int j = 0; j < 3; j++) {
                        gameButtons3Columns[i][j].SetActive(false);
                    }

                    for (int j = 0; j < 4; j++) {
                        gameButtons4Columns[i][j].SetActive(true);
                    }
                }

                break;

            case "Medium":
                for (int i = 0; i < gameButtons2Columns.Count; i++) {
                    for (int j = 0; j < 2; j++) {
                        gameButtons2Columns[i][j].SetActive(false);
                    }

                    for (int j = 0; j < 3; j++) {
                        gameButtons3Columns[i][j].SetActive(true);
                    }

                    for (int j = 0; j < 4; j++) {
                        gameButtons4Columns[i][j].SetActive(false);
                    }
                }

                break;

            case "Hard":
                for (int i = 0; i < gameButtons2Columns.Count; i++) {
                    for (int j = 0; j < 2; j++) {
                        gameButtons2Columns[i][j].SetActive(true);
                    }

                    for (int j = 0; j < 3; j++) {
                        gameButtons3Columns[i][j].SetActive(false);
                    }

                    for (int j = 0; j < 4; j++) {
                        gameButtons4Columns[i][j].SetActive(false);
                    }
                }

                break;

            case "Expert":
                for (int i = 0; i < gameButtons2Columns.Count; i++) {
                    for (int j = 0; j < 2; j++) {
                        gameButtons2Columns[i][j].SetActive(false);
                    }

                    for (int j = 0; j < 3; j++) {
                        gameButtons3Columns[i][j].SetActive(true);
                    }

                    for (int j = 0; j < 4; j++) {
                        gameButtons4Columns[i][j].SetActive(false);
                    }
                }

                break;

            case "Master":
                for (int i = 0; i < gameButtons2Columns.Count; i++) {
                    for (int j = 0; j < 2; j++) {
                        gameButtons2Columns[i][j].SetActive(false);
                    }

                    for (int j = 0; j < 3; j++) {
                        gameButtons3Columns[i][j].SetActive(false);
                    }

                    for (int j = 0; j < 4; j++) {
                        gameButtons4Columns[i][j].SetActive(true);
                    }
                }

                break;
        }
    }

    public void Bet() {
        if (state == "Betting") {
            TotalProfitAmountText.text = "$0";

            BackButton.interactable = false;
            BetInput.interactable = false;
            HelpButton.interactable = false;
            DifficultyDropdown.interactable = false;

            BetButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "Cashout";

            GlobalScript.UpdateMoney("minus", GlobalScript.ReturnNum(BetInput.text));

            MoneyText.text = $"Money: ${GlobalScript.money}";

            for (int i = 0; i < gameButtons4Columns.Count; i++) {
                for (int j = 0; j < gameButtons4Columns[0].Count; j++) {
                    gameButtons4Columns[i][j].GetComponent<Image>().color = new Color32(128, 128, 128, 255);

                    if (i == 0) {
                        gameButtons4Columns[i][j].GetComponent<Image>().color = new Color32(0, 0, 255, 255);
                    }
                }
            }

            for (int i = 0; i < gameButtons3Columns.Count; i++) {
                for (int j = 0; j < gameButtons3Columns[0].Count; j++) {
                    gameButtons3Columns[i][j].GetComponent<Image>().color = new Color32(128, 128, 128, 255);

                    if (i == 0) {
                        gameButtons3Columns[i][j].GetComponent<Image>().color = new Color32(0, 0, 255, 255);
                    }
                }
            }

            for (int i = 0; i < gameButtons2Columns.Count; i++) {
                for (int j = 0; j < gameButtons2Columns[0].Count; j++) {
                    gameButtons2Columns[i][j].GetComponent<Image>().color = new Color32(128, 128, 128, 255);

                    if (i == 0) {
                        gameButtons2Columns[i][j].GetComponent<Image>().color = new Color32(0, 0, 255, 255);
                    }
                }
            }

            state = "Determine Game";
        } else if (state == "Playing") {
            try {
                GlobalScript.UpdateMoney("plus", (decimal) Math.Round(GlobalScript.ReturnNum(BetInput.text) * multiplier[difficulty][currentRow-1], 2));
            } catch (Exception) {
                GlobalScript.UpdateMoney("plus", GlobalScript.ReturnNum(BetInput.text));
            }

            state = "Reset";
        }
    }

    public void decideActionOnTile(List<List<GameObject>> buttonList, int column) {
        for (int i = 0; i < buttonList[currentRow].Count; i++) {
            buttonList[currentRow][i].GetComponent<Button>().interactable = false;
            buttonList[currentRow][i].GetComponent<Image>().color = new Color32(128, 128, 128, 255);

            try {
                buttonList[currentRow+1][i].GetComponent<Button>().interactable = true;
                buttonList[currentRow+1][i].GetComponent<Image>().color = new Color32(0, 0, 255, 255);
            } catch (Exception) {

            }
        }
                    
        buttonList[currentRow][column].GetComponent<Image>().color = new Color32(0, 255, 0, 255);

        currentRow++;

        TotalProfitAmountText.text = $"${Math.Round(GlobalScript.ReturnNum(BetInput.text) * multiplier[difficulty][currentRow-1] - GlobalScript.ReturnNum(BetInput.text), 2)}";
    }

    public void clickTile(int column) {
        if (state == "Playing") {
            int lost = 0;

            if (difficulty == "Hard") {
                if (canClick2Columns[currentRow][column] == true) {
                    decideActionOnTile(gameButtons2Columns, column);
                } else {
                    lost = 2;
                }
            } else if (difficulty == "Medium" || difficulty == "Expert") {
                if (canClick3Columns[currentRow][column] == true) {
                    decideActionOnTile(gameButtons3Columns, column);
                } else {
                    lost = 3;
                }
            } else {
                if (canClick4Columns[currentRow][column] == true) {
                    decideActionOnTile(gameButtons4Columns, column);
                } else {
                    lost = 4;
                }
            }

            switch (lost) {
                case 2:
                    gameButtons2Columns[currentRow][column].GetComponent<Image>().color = new Color32(255, 0, 0, 255);
                    break;

                case 3:
                    gameButtons3Columns[currentRow][column].GetComponent<Image>().color = new Color32(255, 0, 0, 255);
                    break;
                
                case 4:
                    gameButtons4Columns[currentRow][column].GetComponent<Image>().color = new Color32(255, 0, 0, 255);
                    break;
            }

            if (lost > 0) {
                TotalProfitAmountText.text = "$0";

                state = "Reset";
            } 
            
            if (currentRow == 10) {
                GlobalScript.UpdateMoney("plus", (decimal) Math.Round(GlobalScript.ReturnNum(BetInput.text) * multiplier[difficulty][currentRow-1], 2));

                state = "Reset";
            }
        }
    }

    void ResetGame() {
        BackButton.interactable = true;
        BetInput.interactable = true;
        DifficultyDropdown.interactable = true;
        HelpButton.interactable = true;

        currentRow = 0;

        MoneyText.text = $"Money: ${GlobalScript.money}";

        BetButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "Bet";

        gameButtons2Columns[0][0].GetComponent<Button>().interactable = true;
        gameButtons2Columns[0][1].GetComponent<Button>().interactable = true;

        for (int i = 0; i < 3; i++) {
            gameButtons3Columns[0][i].GetComponent<Button>().interactable = true;
        }

        for (int i = 0; i < 4; i++) {
            gameButtons4Columns[0][i].GetComponent<Button>().interactable = true;
        }

        state = "Betting";
    }

    // Update is called once per frame
    void Update() {
        switch (state) {
            case "Betting":
                GlobalScript.ValidateInput(BetInput, BetButton.GetComponent<Button>());

                break;

            case "Determine Game":
                if (difficulty == "Easy") {
                    for (int i = 0; i < gameButtons4Columns.Count; i++) {
                        for (int j = 0; j < gameButtons4Columns[0].Count; j++) {
                            canClick4Columns[i][j] = true;
                            //gameButtons4Columns[i][j].GetComponent<Image>().color = new Color32(0, 0, 255, 255);
                        }
                    }

                    for (int i = 0; i < gameButtons4Columns.Count; i++) {
                        int num = GlobalScript.random.Next(0, 4);
                        
                        canClick4Columns[i][num] = false;
                        //gameButtons4Columns[i][num].GetComponent<Image>().color = new Color32(255, 0, 0, 255);
                    }
                } else if (difficulty == "Medium") {
                    for (int i = 0; i < gameButtons3Columns.Count; i++) {
                        for (int j = 0; j < gameButtons3Columns[0].Count; j++) {
                            canClick3Columns[i][j] = true;
                            //gameButtons3Columns[i][j].GetComponent<Image>().color = new Color32(0, 0, 255, 255);
                        }
                    }

                    for (int i = 0; i < gameButtons3Columns.Count; i++) {
                        int num = GlobalScript.random.Next(0, 3);

                        canClick3Columns[i][num] = false;
                        //gameButtons3Columns[i][num].GetComponent<Image>().color = new Color32(255, 0, 0, 255);
                    }
                } else if (difficulty == "Hard") {
                    for (int i = 0; i < gameButtons2Columns.Count; i++) {
                        for (int j = 0; j < gameButtons2Columns[0].Count; j++) {
                            canClick2Columns[i][j] = true;
                            //gameButtons2Columns[i][j].GetComponent<Image>().color = new Color32(0, 0, 255, 255);
                        }
                    }

                    for (int i = 0; i < gameButtons2Columns.Count; i++) {
                        int num = GlobalScript.random.Next(0, 2);

                        canClick2Columns[i][num] = false;
                        //gameButtons2Columns[i][num].GetComponent<Image>().color = new Color32(255, 0, 0, 255);
                    }
                } else if (difficulty == "Expert") {
                    for (int i = 0; i < gameButtons3Columns.Count; i++) {
                        for (int j = 0; j < gameButtons3Columns[0].Count; j++) {
                            canClick3Columns[i][j] = false;
                            //gameButtons3Columns[i][j].GetComponent<Image>().color = new Color32(255, 0, 0, 255);
                        }
                    }

                    for (int i = 0; i < gameButtons3Columns.Count; i++) {
                        int num = GlobalScript.random.Next(0, 3);

                        canClick3Columns[i][num] = true;
                        //gameButtons3Columns[i][num].GetComponent<Image>().color = new Color32(0, 0, 255, 255);
                    }
                } else if (difficulty == "Master") {
                    for (int i = 0; i < gameButtons4Columns.Count; i++) {
                        for (int j = 0; j < gameButtons4Columns[0].Count; j++) {
                            canClick4Columns[i][j] = false;
                            //gameButtons4Columns[i][j].GetComponent<Image>().color = new Color32(255, 0, 0, 255);
                        }
                    }

                    for (int i = 0; i < gameButtons4Columns.Count; i++) {
                        int num = GlobalScript.random.Next(0, 4);
                        canClick4Columns[i][num] = true;
                       
                        //gameButtons4Columns[i][num].GetComponent<Image>().color = new Color32(0, 0, 255, 255);
                    }
                }

                state = "Playing";
                break;

            case "Reset":
                ResetGame();
                break;
        }
    }
}
