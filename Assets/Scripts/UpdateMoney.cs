using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpdateMoney : MonoBehaviour {
    // Update is called once per frame
    void Update() {
        this.GetComponent<TMP_Text>().text = $"Money: ${Math.Round(GlobalScript.money, 2)}";
    }
}
