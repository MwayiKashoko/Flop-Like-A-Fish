using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class BannerAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener {
    [SerializeField] private string androidAdUnitId;
    [SerializeField] private string iosAdUnitId;

    private string adUnitId;

    private void Awake() {
        #if UNITY_IOS
            adUnitId = iosAdUnitId;
        #elif UNITY_ANDROID
            adUnitId = androidAdUnitId;
        #endif

        Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);
    }

    public void LoadBannerAd() {
        BannerLoadOptions options = new BannerLoadOptions {
            loadCallback = BannerLoaded,
            errorCallback = BannerLoadedError
        };

        Advertisement.Banner.Load(adUnitId, options);
    }

    public void ShowBannerAd() {
        BannerOptions options = new BannerOptions {
            showCallback = BannerShown,
            clickCallback = BannerClicked,
            hideCallback = BannerHidden
        };

        Advertisement.Banner.Show(adUnitId, options);
    }
    
    private void BannerLoaded() {

    }

    private void BannerLoadedError(string message) {

    }

    private void BannerHidden() {

    }

    private void BannerClicked() {

    }

    private void BannerShown() {

    } 

    public void HideBannerAd() {
        Advertisement.Banner.Hide();
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
        //throw new System.NotImplementException();
    }
    #endregion
}
