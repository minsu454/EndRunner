using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Common.Yield;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour, IInit
{
    public static AdManager instance;

    private string aosAppID = "ca-app-pub-3870306051087422~8143028566";
    private string iosAppID = "";

    private string aosRewardAdID = "ca-app-pub-3870306051087422/9874912630";
    private string iosRewardAdID = "";
    private string testRewardAdID = "ca-app-pub-3940256099942544/5224354917";

    private RewardedAd rewardedAd;

    private string appId;
    private string adUnitId;

    public void Init()
    {
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            Debug.Log("광고 초기화 성공!");
        });
    }

    private void Start()
    {
#if UNITY_ANDROID
        appId = aosAppID;
#elif UNITY_IPHONE
            appId = "";
#else
            appId = "unexpected_platform";
#endif
        TitleManager.instance.ShowTestLabel("Init");

        RequestRewardBasedVideo();
    }

    private void RequestRewardBasedVideo()
    {
#if UNITY_ANDROID
        adUnitId = testRewardAdID;
#elif UNITY_IPHONE
            adUnitId = testRewardAdID;
#else
            adUnitId = "unexpected_platform";
#endif
        //TitleManager.instance.ShowTestLabel("Request");

        // Load the rewarded video ad with the request.
        LoadRewardedAd();
    }

    public void LoadRewardedAd()
    {
        // 새 광고를 로드하기 전에 이전 광고를 정리하십시오.
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

        // 광고를 로드하는 데 사용되는 요청을 생성합니다.
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        // 광고 로드 요청을 보냅니다.
        RewardedAd.Load(adUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                RegisterEventHandlers(ad);

                // 오류가 null이 아니면 로드 요청이 실패한 것입니다.
                if (error != null || ad == null)
                {
                    Debug.LogError("보상형 광고가 광고를 로드하지 못했습니다. " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("응답으로 로드된 보상형 광고 : " + ad.GetResponseInfo());

                rewardedAd = ad;
            });
    }

    public void ShowRewardedAd()
    {
        const string rewardMsg =
            "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                // TODO: Reward the user.
                Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));

                //부활 코드
                GameManager.instance.Bomb();
                GameManager.instance.ManaReset();
                GameManager.instance.ReSet();
                PlayerManager.instance.hatSprite.gameObject.SetActive(true);
                PlayerManager.instance.eyeSprite.gameObject.SetActive(true);
                PlayerManager.instance.tieSprite.gameObject.SetActive(true);
                PopupContainer.GetActivatedPopup().Close();
                PlayerManager.instance.gameObject.SetActive(true);
                PlayerManager.instance.restartPaticle.SetActive(true);
                Managers.Sound.SetBGM(1f);
            });
        }
        else
        {
            //예외 팝업
            BasePopup basePopup = PopupContainer.CreatePopup(PopupType.ConfirmationPopup);
            ((ConfirmationPopup)basePopup).SetCallback(() =>
            {
                PopupContainer.GetActivatedPopup(2).Close();
                GameManager.instance.GameOver();
            });
            ((ConfirmationPopup)basePopup).SetText(Managers.Data.GetText(23));
            basePopup.Init();
        }
    }

    private void RegisterEventHandlers(RewardedAd ad)
    {
        // 광고에서 수익이 발생한 것으로 추정될 때 발생합니다.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log("광고에서 수익이 발생한 것으로 추정될 때 발생합니다. " + adValue.Value + " // " + adValue.CurrencyCode);
        };
        // 광고에 대한 노출이 기록될 때 발생합니다.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("광고에 대한 노출이 기록될 때 발생합니다.");
        };
        // 광고에 대한 클릭이 기록될 때 발생합니다.
        ad.OnAdClicked += () =>
        {
            Debug.Log("광고에 대한 클릭이 기록될 때 발생합니다.");
        };
        // 광고가 전체 화면 콘텐츠를 열 때 발생합니다.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("광고가 전체 화면 콘텐츠를 열 때 발생합니다.");
        };
        // 광고가 전체 화면 콘텐츠를 닫을 때 발생합니다.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("광고가 전체 화면 콘텐츠를 닫을 때 발생합니다.");
            // 다음 보상형 광고 미리 로드
            LoadRewardedAd();
        };
        // 광고가 전체 화면 콘텐츠를 열지 못했을 때 발생합니다.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("광고가 전체 화면 콘텐츠를 열지 못했을 때 발생합니다." + " with error : " + error);
            // 다음 보상형 광고 미리 로드
            LoadRewardedAd();
        };
    }
}
