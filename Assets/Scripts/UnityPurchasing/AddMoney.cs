using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class AddMoney : MonoBehaviour {
    public void BuyMoney(Product product) {
        if (!GlobalScript.noAds) {
            GlobalScript.noAds = true;
        }

        GlobalScript.money += (decimal) product.definition.payout.quantity;
    }
}
