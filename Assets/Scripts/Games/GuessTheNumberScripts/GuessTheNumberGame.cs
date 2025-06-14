using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GuessTheNumberGame : MonoBehaviour {
    private int result = 0;

    private Button BackButton;
    private TMP_InputField BetInput;
    private TMP_InputField MinNumberInput;
    private TMP_InputField MaxNumberInput;
    private TMP_Text PayoutMoneyText;
    private Button BetButton;
    private TMP_Text MoneyText;
    private TMP_Text WinProbabilityAmountText;
    private TMP_InputField GuessInput;
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
        MinNumberInput = GameObject.Find("MinNumberInput").GetComponent<TMP_InputField>();
        MaxNumberInput = GameObject.Find("MaxNumberInput").GetComponent<TMP_InputField>();
        PayoutMoneyText = GameObject.Find("PayoutMoneyText").GetComponent<TMP_Text>();
        BetButton = GameObject.Find("BetButton").GetComponent<Button>();
        MoneyText = GameObject.Find("MoneyText").GetComponent<TMP_Text>();
        WinProbabilityAmountText = GameObject.Find("WinProbabilityAmountText").GetComponent<TMP_Text>();
        GuessInput = GameObject.Find("GuessInput").GetComponent<TMP_InputField>();
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

        if (GlobalScript.ReturnNum(GuessInput.text) < GlobalScript.ReturnNum(MinNumberInput.text) || GlobalScript.ReturnNum(GuessInput.text) > GlobalScript.ReturnNum(MaxNumberInput.text) || GlobalScript.ReturnNum(MinNumberInput.text) < 1M || GlobalScript.ReturnNum(MinNumberInput.text) > 999_999_999M || GlobalScript.ReturnNum(MaxNumberInput.text) < 2M || GlobalScript.ReturnNum(MaxNumberInput.text) > 1_000_000_000M || BetButton.interactable == false) {
            BetButton.interactable = false;
        } else {
            BetButton.interactable = true;
        }

        if (BetButton.interactable) {
            WinProbabilityAmountText.text = $"{Math.Round(1M / (GlobalScript.ReturnNum(MaxNumberInput.text) - GlobalScript.ReturnNum(MinNumberInput.text) + 1M), 9) * 100M}";

            PayoutMoneyText.text = $"${(GlobalScript.ReturnNum(MaxNumberInput.text) - GlobalScript.ReturnNum(MinNumberInput.text) + 1M) * 0.98M * GlobalScript.ReturnNum(BetInput.text)}";
        } else {
            WinProbabilityAmountText.text = $"0";

            PayoutMoneyText.text = $"$0";
        }
    }

    public void ResetGame() {
        elapsedTime = 0;

        BackButton.interactable = true;
        BetInput.interactable = true;
        MinNumberInput.interactable = true;
        MaxNumberInput.interactable = true;
        BetButton.interactable = true;
        GuessInput.interactable = true;
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
                MinNumberInput.interactable = false;
                MaxNumberInput.interactable = false;
                BetButton.interactable = false;
                GuessInput.interactable = false;
                HelpButton.interactable = false;

                GlobalScript.RoundInput(BetInput);

                GlobalScript.UpdateMoney("minus", GlobalScript.ReturnNum(BetInput.text));

                MoneyText.text = $"Money: ${GlobalScript.money}";

                result = GlobalScript.random.Next((int) GlobalScript.ReturnNum(MinNumberInput.text), (int) GlobalScript.ReturnNum(MaxNumberInput.text) + 1);

                startingValue = (float) GlobalScript.ReturnNum(ResultText.text);

                state = "Animation";

                break;

            case "Animation":
                if ((int) GlobalScript.ReturnNum(ResultText.text) == result) {
                    state = "Reset";
                } else {
                    elapsedTime += Time.deltaTime;

                    percentageComplete = elapsedTime/desiredSlidingNumberAnimationTime;

                    ResultText.text = $"{Math.Round(Mathf.Lerp(startingValue, result, Mathf.SmoothStep(0, 1, percentageComplete)))}";
                }

                break;

            case "Reset":
                ResultText.color = new Color32(162, 0, 0, 255);

                if ((int) GlobalScript.ReturnNum(GuessInput.text) == result) {
                    ResultText.color = new Color32(0, 148, 197, 255);
                    GlobalScript.UpdateMoney("plus", (GlobalScript.ReturnNum(MaxNumberInput.text) - GlobalScript.ReturnNum(MinNumberInput.text) + 1M) * 0.98M * GlobalScript.ReturnNum(BetInput.text));
                }

                MoneyText.text = $"Money: ${GlobalScript.money}";

                ResetGame();
                break;
        }
    }
}
