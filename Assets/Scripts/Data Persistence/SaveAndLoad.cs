using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveAndLoad : MonoBehaviour, IDataPersistence {
    public void LoadData(GameData data) {
        GlobalScript.money = (decimal) Math.Round(data.money, 2);
        GlobalScript.moneyWagered = (decimal) Math.Round(data.moneyWagered, 2);
        GlobalScript.moneyWon = (decimal) Math.Round(data.moneyWon, 2);
        GlobalScript.profit = (decimal) Math.Round(data.profit, 2);
        GlobalScript.moneyLost = (decimal) Math.Round(data.moneyLost, 2);
        GlobalScript.betsPlaced = data.betsPlaced;
        GlobalScript.totalWins = data.totalWins;
        GlobalScript.totalLosses = data.totalLosses;
        GlobalScript.largestWin = (decimal) Math.Round(data.largestWin, 2);
        GlobalScript.largestLoss = (decimal) Math.Round(data.largestLoss, 2);
        GlobalScript.luckiestWin = (float) Math.Round(data.luckiestWin, 2);
        GlobalScript.noAds = data.noAds;
    }

    public void SaveData(ref GameData data) {
        data.money = (float) Math.Round(GlobalScript.money, 2);
        data.moneyWagered = (float) Math.Round(GlobalScript.moneyWagered, 2);
        data.moneyWon = (float) Math.Round(GlobalScript.moneyWon, 2);
        data.profit = (float) Math.Round(GlobalScript.profit, 2);
        data.moneyLost = (float) Math.Round(GlobalScript.moneyLost, 2);
        data.betsPlaced = GlobalScript.betsPlaced;
        data.totalWins = GlobalScript.totalWins;
        data.totalLosses = GlobalScript.totalLosses;
        data.largestWin = (float) Math.Round(GlobalScript.largestWin, 2);
        data.largestLoss = (float) Math.Round(GlobalScript.largestLoss, 2);
        data.luckiestWin = (float) Math.Round(GlobalScript.luckiestWin, 2);
        data.noAds = GlobalScript.noAds;
    }
}
