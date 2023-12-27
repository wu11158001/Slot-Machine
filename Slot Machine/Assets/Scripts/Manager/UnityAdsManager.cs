using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class UnityAdsManager : BaseManager,  IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    private const string androidAdUnitId = "Rewarded_Android";
    private const string androidGameId = "5502091";
    private bool testMode = true;

    public override void OnInit()
    {
        base.OnInit();

        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(androidGameId, testMode, this);
        }
    }

    /// <summary>
    /// 初始化完成
    /// </summary>
    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads 初始化完成");
        LoadAd();
    }

    /// <summary>
    /// 初始化失敗
    /// </summary>
    /// <param name="error"></param>
    /// <param name="message"></param>
    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads 初始化失敗: {error} - {message}");
    }

    /// <summary>
    /// 載入廣告
    /// </summary>
    public void LoadAd()
    {
        Debug.Log("載入廣告: " + androidGameId);
        entry.isAdComplete = false;        
        Advertisement.Load(androidAdUnitId, this);
    }

    /// <summary>
    /// 載入廣告完成
    /// </summary>
    /// <param name="adUnitId"></param>
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("載入廣告完成: " + adUnitId);
        entry.isAdComplete = true;
        if (adUnitId.Equals(androidGameId))
        {
            //_showAdButton.onClick.AddListener(ShowAd);
            //_showAdButton.interactable = true;
        }
    }

    /// <summary>
    /// 載入廣告失敗
    /// </summary>
    /// <param name="adUnitId"></param>
    /// <param name="error"></param>
    /// <param name="message"></param>
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"載入廣告失敗: {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }

    /// <summary>
    /// 廣告開始
    /// </summary>
    public void ShowAd()
    {
        Advertisement.Show(androidAdUnitId, this);
        LoadAd();
    }

    /// <summary>
    /// 廣告結束
    /// </summary>
    /// <param name="adUnitId"></param>
    /// <param name="showCompletionState"></param>
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(androidAdUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("廣告結束");
        }
    }

    /// <summary>
    /// 顯示廣告失敗
    /// </summary>
    /// <param name="adUnitId"></param>
    /// <param name="error"></param>
    /// <param name="message"></param>
    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"顯示廣告失敗: {adUnitId}: {error} - {message}");
    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }
}
