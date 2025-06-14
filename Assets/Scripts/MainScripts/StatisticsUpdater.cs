using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;

public class StatisticsUpdater : MonoBehaviour {
    private TMP_Text MoneyText;
    private TMP_Text MoneyWageredText;
    private TMP_Text MoneyWonText;
    private TMP_Text ProfitText;
    private TMP_Text MoneyLostText;
    private TMP_Text BetsPlacedText;
    private TMP_Text TotalWinsText;
    private TMP_Text TotalLossesText;
    private TMP_Text LargestWinText;
    private TMP_Text LargestLossText;
    private TMP_Text LuckiestWinText;

    void Start() {
        MoneyText = GameObject.Find("MoneyText").GetComponent<TMP_Text>();
        MoneyWageredText = GameObject.Find("MoneyWageredText").GetComponent<TMP_Text>();
        MoneyWonText = GameObject.Find("MoneyWonText").GetComponent<TMP_Text>();
        ProfitText = GameObject.Find("ProfitText").GetComponent<TMP_Text>();
        MoneyLostText = GameObject.Find("MoneyLostText").GetComponent<TMP_Text>();
        BetsPlacedText = GameObject.Find("BetsPlacedText").GetComponent<TMP_Text>();
        TotalWinsText = GameObject.Find("TotalWinsText").GetComponent<TMP_Text>();
        TotalLossesText = GameObject.Find("TotalLossesText").GetComponent<TMP_Text>();
        LargestWinText = GameObject.Find("LargestWinText").GetComponent<TMP_Text>();
        LargestLossText = GameObject.Find("LargestLossText").GetComponent<TMP_Text>();
        LuckiestWinText = GameObject.Find("LuckiestWinText").GetComponent<TMP_Text>();

        MoneyText.text = $"Money: ${GlobalScript.money}";
        MoneyWageredText.text = $"Money Wagered: ${GlobalScript.moneyWagered}";
        MoneyWonText.text = $"Money Won: ${GlobalScript.moneyWon}";
        ProfitText.text = $"Profit: ${GlobalScript.profit}";
        MoneyLostText.text = $"Money Lost: ${GlobalScript.moneyLost}";
        BetsPlacedText.text = $"Bets Placed: {GlobalScript.betsPlaced}";
        TotalWinsText.text = $"Total Wins: {GlobalScript.totalWins}";
        TotalLossesText.text = $"Total Losses: {GlobalScript.totalLosses}";
        LargestWinText.text = $"Largest Win: ${GlobalScript.largestWin}";
        LargestLossText.text = $"Largest Loss: ${GlobalScript.largestLoss}";
        LuckiestWinText.text = $"Luckiest Win: {GlobalScript.luckiestWin}x";
    }

    void Update() {
        MoneyText.text = $"Money: ${GlobalScript.money}";
        MoneyWageredText.text = $"Money Wagered: ${GlobalScript.moneyWagered}";
        MoneyWonText.text = $"Money Won: ${GlobalScript.moneyWon}";
        ProfitText.text = $"Profit: ${GlobalScript.profit}";
        MoneyLostText.text = $"Money Lost: ${GlobalScript.moneyLost}";
        BetsPlacedText.text = $"Bets Placed: {GlobalScript.betsPlaced}";
        TotalWinsText.text = $"Total Wins: {GlobalScript.totalWins}";
        TotalLossesText.text = $"Total Losses: {GlobalScript.totalLosses}";
        LargestWinText.text = $"Largest Win: ${GlobalScript.largestWin}";
        LargestLossText.text = $"Largest Loss: ${GlobalScript.largestLoss}";
        LuckiestWinText.text = $"Luckiest Win: {GlobalScript.luckiestWin}x";
    }
}
