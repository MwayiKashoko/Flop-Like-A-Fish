using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickedAd : MonoBehaviour {
    public void IncreaseMoney() {
        AdsManager.Instance.rewardedAds.ShowRewardedAd();
    }
}
