using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    public bool isRewarded = false;
    //private bool showBanner = false;
    private Scene scene;

    public static GameManager Instance { get; private set; }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //DontDestroyOnLoad(gameObject);

        scene = SceneManager.GetActiveScene();

        if (scene.name == "About" || scene.name == "Statistics" || scene.name == "Settings" || scene.name == "Shop" || scene.name == "GameExplanation" || scene.name == "GameList") {
            //showBanner = true;
            StartCoroutine(DisplayBannerWithDelay());
        }
    }

    private void OnDestroy() {        
        /*showBanner = false;
        AdsManager.Instance.bannerAds.HideBannerAd();*/
    }

    private IEnumerator DisplayBannerWithDelay() {
        yield return new WaitForSeconds(0.5f);

        if (!GlobalScript.noAds) {
            AdsManager.Instance.bannerAds.ShowBannerAd();
        }
    }

    public void startAd() {
        AdsManager.Instance.rewardedAds.ShowRewardedAd();
    }

    public void AddMoney() {
        if (isRewarded) {
            GlobalScript.money += 100;
            isRewarded = false;
        }
    }
}
