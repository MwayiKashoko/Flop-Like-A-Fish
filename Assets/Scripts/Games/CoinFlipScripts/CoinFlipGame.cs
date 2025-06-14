using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinFlipGame : MonoBehaviour {
    private string state = "Betting";

    private bool betHeads = false;

    private int result;

    private Button BackButton;
    private TMP_InputField BetInput;
    private Button BetHeadsButton;
    private Button BetTailsButton;
    private Image TailsImage;
    private Image HeadsImage;
    private TMP_Text MoneyText;
    private Button HelpButton;

    private float elapsedTime = 0;
    private float desiredCoinFlipAnimationTime = 2f;
    private int flips = 20;
    private int desiredRotation;

    // Start is called before the first frame update
    void Start() {
        BackButton = GameObject.Find("BackButton").GetComponent<Button>();
        BetInput = GameObject.Find("BetInput").GetComponent<TMP_InputField>();
        BetHeadsButton = GameObject.Find("BetHeadsButton").GetComponent<Button>();
        BetTailsButton = GameObject.Find("BetTailsButton").GetComponent<Button>();
        TailsImage = GameObject.Find("TailsImage").GetComponent<Image>();
        HeadsImage = GameObject.Find("HeadsImage").GetComponent<Image>();
        MoneyText = GameObject.Find("MoneyText").GetComponent<TMP_Text>();
        HelpButton = GameObject.Find("HelpButton").GetComponent<Button>();

        MoneyText.text = $"Money: ${GlobalScript.money}";

        desiredRotation = 360 * flips;
    }

    public void Bet(Button button) {
        state = "Flipping";

        BackButton.interactable = false;
        BetHeadsButton.interactable = false;
        BetTailsButton.interactable = false;
        BetInput.interactable = false;
        HelpButton.interactable = false;

        result = GlobalScript.random.Next(1, 3);

        if (result == 2) {
            desiredRotation -= 180;
        }

        betHeads = button.name == "BetHeadsButton" ? true : false;

        GlobalScript.RoundInput(BetInput);

        GlobalScript.UpdateMoney("minus", GlobalScript.ReturnNum(BetInput.text));
        MoneyText.text = $"Money: ${GlobalScript.money}";
    }

    public void ValidateInput() {
        GlobalScript.ValidateInput(BetInput, BetHeadsButton);
        GlobalScript.ValidateInput(BetInput, BetTailsButton);
    }

    public void ResetGame() {
        elapsedTime = 0;

        BackButton.interactable = true;
        BetHeadsButton.interactable = true;
        BetTailsButton.interactable = true;
        BetInput.interactable = true;
        HelpButton.interactable = true;

        desiredRotation = 360 * flips;

        state = "Betting";

        MoneyText.text = $"Money: ${GlobalScript.money}";

        GameObject.Find("TailsImage").transform.localRotation = Quaternion.Euler(0, 0, 0);
        GameObject.Find("HeadsImage").transform.localRotation = GameObject.Find("TailsImage").transform.localRotation;

        ValidateInput();
    }

    // Update is called once per frame
    void Update() {
        switch (state) {
            case "Flipping":
                elapsedTime += Time.deltaTime;

                float percentageComplete = elapsedTime / desiredCoinFlipAnimationTime;

                float nextRotation = Mathf.Lerp(0, desiredRotation, Mathf.SmoothStep(0, 1, percentageComplete));

                GameObject.Find("TailsImage").transform.localRotation = Quaternion.Euler(0, nextRotation + 180f, 0);
                GameObject.Find("HeadsImage").transform.localRotation = Quaternion.Euler(0, nextRotation, 0);

                if (GameObject.Find("TailsImage").transform.localRotation.y % 360 >= 0.5f || GameObject.Find("TailsImage").transform.localRotation.y % 360 <= -0.5f) {
                    GameObject.Find("HeadsImage").GetComponent<RectTransform>().SetAsLastSibling();
                } else {
                    GameObject.Find("TailsImage").GetComponent<RectTransform>().SetAsLastSibling();
                }

                if (percentageComplete >= 1) {
                    state = "DetermineWinner";
                }

                break;

            case "DetermineWinner":
                if (betHeads) {
                    if (result == 1) {
                        GlobalScript.UpdateMoney("plus", GlobalScript.ReturnNum(BetInput.text) * (decimal) 1.98f);
                    }
                } else {
                    if (result == 2) {
                        GlobalScript.UpdateMoney("plus", GlobalScript.ReturnNum(BetInput.text) * (decimal) 1.98f);
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
