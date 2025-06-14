using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class RewardedAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener {
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

    public void LoadRewardedAd() {
        Advertisement.Load(adUnitId, this);
    }

    public void ShowRewardedAd() {
        Advertisement.Show(adUnitId, this);
        LoadRewardedAd();
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
        
        if (placementId == adUnitId && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED)) {
            Debug.Log("add $10/100 to globalscript.money because ad is completed");
            GameManager.Instance.isRewarded = true;
            GameManager.Instance.AddMoney();
        }
    }
    #endregion
}
