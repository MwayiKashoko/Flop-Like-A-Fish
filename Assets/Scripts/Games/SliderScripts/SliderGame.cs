using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderGame : MonoBehaviour {
    private float result = 0f;

    private Button BackButton;
    private Button RollOver;
    private Button RollUnder;
    private Button BetButton;
    private TMP_InputField BetInput;
    private TMP_Text PayoutMoneyText;
    private TMP_Text MoneyText;
    private GameObject slider;
    private TMP_Text MultiplierAmountText;
    private TMP_Text WinProbabilityAmountText;
    private TMP_InputField RollInput;
    private TMP_Text RollText;
    private Image PickImage;
    private TMP_Text ResultText;
    private Button HelpButton;

    private string state = "Betting";
    
    private bool rollingOver = true;

    private Vector3 startPosition;
    private Vector3 targetPosition;

    private float elapsedTime = 0;

    private float desiredPickImageAnimation = 0.5f;

    // Start is called before the first frame update
    void Start() {
        BackButton = GameObject.Find("BackButton").GetComponent<Button>();
        RollOver = GameObject.Find("RollOverButton").GetComponent<Button>();
        RollUnder = GameObject.Find("RollUnderButton").GetComponent<Button>();
        BetButton = GameObject.Find("BetButton").GetComponent<Button>();
        BetInput = GameObject.Find("BetInput").GetComponent<TMP_InputField>();
        PayoutMoneyText = GameObject.Find("PayoutMoneyText").GetComponent<TMP_Text>();
        MoneyText = GameObject.Find("MoneyText").GetComponent<TMP_Text>();
        slider = GameObject.Find("Slider");
        MultiplierAmountText = GameObject.Find("MultiplierAmountText").GetComponent<TMP_Text>();
        WinProbabilityAmountText = GameObject.Find("WinProbabilityAmountText").GetComponent<TMP_Text>();
        RollInput = GameObject.Find("RollInput").GetComponent<TMP_InputField>();
        RollText = GameObject.Find("RollText").GetComponent<TMP_Text>();
        PickImage = GameObject.Find("PickImage").GetComponent<Image>();
        ResultText = GameObject.Find("PickImage").transform.GetChild(0).GetComponent<TMP_Text>();
        HelpButton = GameObject.Find("HelpButton").GetComponent<Button>();

        MoneyText.text = $"Money: ${GlobalScript.money}";

        startPosition = new Vector3(PickImage.transform.localPosition.x, PickImage.transform.localPosition.y, 0);
        targetPosition = new Vector3(0, PickImage.transform.localPosition.y, 0);
    }

    //Called when the bet button is pressed which starts the game
    public void Bet() {
        switch (state) {
            case "Betting":
                state = "Dealing";
                break;
        }
    }

    public void setColor(Button button) {
        if (button.name == "RollOverButton") {
            slider.transform.GetChild(0).GetComponent<Image>().color = new Color(0.2856814f, 1f, 0.2216981f);
            slider.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = new Color(0.7735849f, 0.08392668f, 0.08392668f);
            rollingOver = true;
            RollText.text = "Roll Over";
        } else {
            slider.transform.GetChild(0).GetComponent<Image>().color = new Color(0.7735849f, 0.08392668f, 0.08392668f);
            slider.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = new Color(0.2856814f, 1f, 0.2216981f);
            rollingOver = false;
            RollText.text = "Roll Under";
        }
    }

    public void updateOtherValue(GameObject obj) {
        if (obj.GetComponent<Slider>() != null) {
            try {
                RollInput.text = $"{slider.GetComponent<Slider>().value}";
            } catch (Exception) {

            }
        } else {
            try {
                slider.GetComponent<Slider>().value = (float) GlobalScript.ReturnNum(RollInput.text);
            } catch (Exception) {

            }
        }
    }

    public void ValidateInput() {
        GlobalScript.ValidateInput(BetInput, BetButton);

        if (GlobalScript.ReturnNum(RollInput.text) < 0.01M || GlobalScript.ReturnNum(RollInput.text) > 99.99M || BetButton.interactable == false) {
            BetButton.interactable = false;
        } else {
            BetButton.interactable = true;
        }

        if (rollingOver) {
            WinProbabilityAmountText.text = $"{100f - (float) GlobalScript.ReturnNum(RollInput.text)}";
        } else {
            WinProbabilityAmountText.text = $"{GlobalScript.ReturnNum(RollInput.text)}";
        }

        MultiplierAmountText.text = $"{99f / (float) GlobalScript.ReturnNum(WinProbabilityAmountText.text)}";

        PayoutMoneyText.text = $"${Math.Round(GlobalScript.ReturnNum(MultiplierAmountText.text) * GlobalScript.ReturnNum(BetInput.text), 2)}";
    }

    public void ResetGame() {
        elapsedTime = 0;
        state = "Betting";

        BetButton.interactable = true;
        BackButton.interactable = true;
        RollOver.interactable = true;
        RollUnder.interactable = true;
        BetInput.interactable = true;
        RollInput.interactable = true;
        HelpButton.interactable = true;
        slider.GetComponent<Slider>().interactable = true;
    }

    // Update is called once per frame
    void Update() {
        switch (state) {
            case "Betting":
                ValidateInput();
                break;

            case "Dealing":
                BetButton.interactable = false;
                BackButton.interactable = false;
                RollOver.interactable = false;
                RollUnder.interactable = false;
                BetInput.interactable = false;
                RollInput.interactable = false;
                HelpButton.interactable = false;
                slider.GetComponent<Slider>().interactable = false;

                GlobalScript.RoundInput(BetInput);

                GlobalScript.UpdateMoney("minus", GlobalScript.ReturnNum(BetInput.text));

                MoneyText.text = $"Money: ${GlobalScript.money}";

                result = (float) GlobalScript.random.Next(0, 10001) / 100f;

                ResultText.text = $"{result}";

                state = "Animation";

                targetPosition.x = slider.transform.localPosition.x + ((result - 50f)/100f * (slider.GetComponent<RectTransform>().rect.width * (slider.transform.localScale.x)));

                ResultText.color = new Color32(255, 0, 0, 255);

                if (rollingOver) {
                    if (result >= slider.GetComponent<Slider>().value) {
                        ResultText.color = new Color32(0, 255, 0, 255);
                        GlobalScript.UpdateMoney("plus", Math.Round(GlobalScript.ReturnNum(BetInput.text) * (decimal) GlobalScript.ReturnNum(MultiplierAmountText.text), 2));
                    }
                } else {
                    if (result <= slider.GetComponent<Slider>().value) {
                        ResultText.color = new Color32(0, 255, 0, 255);
                        GlobalScript.UpdateMoney("plus", Math.Round(GlobalScript.ReturnNum(BetInput.text) * (decimal) GlobalScript.ReturnNum(MultiplierAmountText.text), 2));
                    }
                }

                break;

            case "Animation":
                elapsedTime += Time.deltaTime;

                float percentageComplete = elapsedTime / desiredPickImageAnimation;

                PickImage.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, Mathf.SmoothStep(0, 1, percentageComplete));

                if (Vector3.Distance(PickImage.transform.localPosition, targetPosition) < 0.1f) {
                    startPosition.x = targetPosition.x;
                    startPosition.y = targetPosition.y;

                    state = "Reset";
                }

                break;

            case "Reset":
                MoneyText.text = $"Money: ${GlobalScript.money}";

                ResetGame();
                break;
        }
    }
}
