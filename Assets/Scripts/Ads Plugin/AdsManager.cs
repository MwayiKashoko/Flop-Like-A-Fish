using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour {
    public InitializeAds initializeAds;
    public InterstitialAds interstitialAds;
    public RewardedAds rewardedAds;
    public BannerAds bannerAds;

    public static AdsManager Instance { get; private set; }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        interstitialAds.LoadInterstitialAd();
        rewardedAds.LoadRewardedAd();
        bannerAds.LoadBannerAd();
    }
}
