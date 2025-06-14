using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData {
    //Yup
    public float money;
    //Yup
    public float moneyWagered;
    //Yup
    public float moneyWon;
    //Yup
    public float profit;
    //Yup
    public float moneyLost;
    //Yup
    public int betsPlaced;
    //Yup
    public int totalWins;
    //Yup
    public int totalLosses;
    //Yup
    public float largestWin;
    //Yup
    public float largestLoss;
    //Yup
    public float luckiestWin;

    public bool noAds;

    public GameData() {
        this.money = 1000;
        this.moneyWagered = 0;
        this.moneyWon = 0;
        this.profit = 0;
        this.moneyLost = 0;
        this.betsPlaced = 0;
        this.totalWins = 0;
        this.totalLosses = 0;
        this.largestWin = 0;
        this.largestLoss = 0;
        this.luckiestWin = 0;
        this.noAds = false;
    }
}
