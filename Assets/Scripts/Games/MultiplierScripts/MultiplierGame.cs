using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MultiplierGame : MonoBehaviour {
    private float result = 0f;

    private Button BackButton;
    private TMP_InputField BetInput;
    private TMP_Text PayoutMoneyText;
    private Button BetButton;
    private TMP_Text MoneyText;
    private TMP_Text WinProbabilityAmountText;
    private TMP_InputField MultiplierInput;
    private TMP_Text ResultText;
    private Button HelpButton;

    private string state = "Betting";

    private float elapsedTime = 0;
    private float startingValue = 0;
    private float percentageComplete = 0;
    private float desiredSlidingNumberAnimationTime = 1f;

    // Start is called before the first frame update
    void Start() {
        BackButton = GameObject.Find("BackButton").GetComponent<Button>();
        BetInput = GameObject.Find("BetInput").GetComponent<TMP_InputField>();
        PayoutMoneyText = GameObject.Find("PayoutMoneyText").GetComponent<TMP_Text>();
        BetButton = GameObject.Find("BetButton").GetComponent<Button>();
        MoneyText = GameObject.Find("MoneyText").GetComponent<TMP_Text>();
        WinProbabilityAmountText = GameObject.Find("WinProbabilityAmountText").GetComponent<TMP_Text>();
        MultiplierInput = GameObject.Find("MultiplierInput").GetComponent<TMP_InputField>();
        ResultText = GameObject.Find("ResultText").GetComponent<TMP_Text>();
        HelpButton = GameObject.Find("HelpButton").GetComponent<Button>();

        MoneyText.text = $"Money: ${GlobalScript.money}";
    }

    //Called when the bet button is pressed which starts the game
    public void Bet() {
        switch (state) {
            case "Betting":
                state = "Dealing";
                break;
        }
    }

    public void ValidateInput() {
        GlobalScript.ValidateInput(BetInput, BetButton);

        if (GlobalScript.ReturnNum(MultiplierInput.text) < 1.01M || GlobalScript.ReturnNum(MultiplierInput.text) > 1_000_000M || BetButton.interactable == false) {
            BetButton.interactable = false;
        } else {
            BetButton.interactable = true;
        }

        if (GlobalScript.ReturnNum(MultiplierInput.text) >= 1.01M) {
            WinProbabilityAmountText.text = $"{Math.Round(1M / GlobalScript.ReturnNum(MultiplierInput.text) * 99, 9)}";

            PayoutMoneyText.text = $"${GlobalScript.ReturnNum(BetInput.text) * GlobalScript.ReturnNum(MultiplierInput.text)}";
        } else {
            WinProbabilityAmountText.text = $"0";

            PayoutMoneyText.text = $"$0";
        }
    }

    public void ResetGame() {
        elapsedTime = 0;

        BackButton.interactable = true;
        BetInput.interactable = true;
        BetButton.interactable = true;
        MultiplierInput.interactable = true;
        HelpButton.interactable = true;

        state = "Betting";
    }

    // Update is called once per frame
    void Update() {
        switch (state) {
            case "Betting":
                ValidateInput();
                break;

            case "Dealing":
                BackButton.interactable = false;
                BetInput.interactable = false;
                BetButton.interactable = false;
                MultiplierInput.interactable = false;
                HelpButton.interactable = false;

                GlobalScript.RoundInput(BetInput);

                GlobalScript.UpdateMoney("minus", GlobalScript.ReturnNum(BetInput.text));

                MoneyText.text = $"Money: ${GlobalScript.money}";

                if (GlobalScript.random.Next(1, 101) == 1) {
                    result = 1;
                } else {
                    result = (float) GlobalScript.random.NextDouble();
                }

                startingValue = (float) GlobalScript.ReturnNum(ResultText.text);

                state = "Animation";

                break;

            case "Animation":
                if ((float) GlobalScript.ReturnNum(ResultText.text) == (float) Math.Round(1f / result, 2)) {
                    state = "Reset";
                } else {
                    elapsedTime += Time.deltaTime;

                    percentageComplete = elapsedTime/desiredSlidingNumberAnimationTime;

                    ResultText.text = $"{Math.Round(Mathf.Lerp(startingValue, (float) Math.Round(1f / result, 2), Mathf.SmoothStep(0, 1, percentageComplete)), 2)}";
                }

                break;

            case "Reset":
                ResultText.color = new Color32(162, 0, 0, 255);

                if ((decimal) result < 1M / GlobalScript.ReturnNum(MultiplierInput.text)) {
                    ResultText.color = new Color32(0, 148, 197, 255);
                    GlobalScript.UpdateMoney("plus", GlobalScript.ReturnNum(BetInput.text) * GlobalScript.ReturnNum(MultiplierInput.text));
                }

                MoneyText.text = $"Money: ${GlobalScript.money}";

                ResetGame();
                break;
        }
    }
}
