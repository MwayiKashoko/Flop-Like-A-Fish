using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class InterstitialAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener {
    [SerializeField] private string androidAdUnitId;
    [SerializeField] private string iosAdUnitId;

    private string adUnitId;

    private void Awake() {
        #if UNITY_IOS
            adUnitId = iosAdUnitId;
        #elif UNITY_ANDROID
            adUnitId = androidAdUnitId;
        #endif
    }

    public void LoadInterstitialAd() {
        Advertisement.Load(adUnitId, this);
    }

    public void ShowInterstitialAd() {
        Advertisement.Show(adUnitId, this);
        LoadInterstitialAd();
    }

    public void OnUnityAdsAdLoaded(string placementId) {
        //throw new System.NotImplementedException();
        //Debug.Log("ad error");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message) {
        //throw new System.NotImplementedException();
        Debug.Log("ad error");
    }

    #region ShowCallbacks
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) {
        //throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowStart(string placementId) {
        //throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowClick(string placementId) {
        //throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState) {
        Debug.Log("Ad completed");
    }
    #endregion
}
