using UnityEngine;

public class IronSourceAdsProvider : AbstractAdProvider
{

    public IronSourceAdsProvider(IAdListener listener) : base(listener)
    {
    }

    public override void InitializeProvider(IAdListener listener)
    {
        Listener = listener;
#if UNITY_ANDROID
        string appKey = "1b3d3bf1d";
#elif UNITY_IPHONE
        string appKey = "8545d445";
#else
        string appKey = "unexpected_platform";
#endif
        Debug.Log("unity-script: unity version" + IronSource.unityVersion());

        // SDK init
        Debug.Log("unity-script: IronSource.Agent.init");
        IronSource.Agent.init(appKey, IronSourceAdUnits.REWARDED_VIDEO);
        //For Interstitial
        IronSource.Agent.init(appKey, IronSourceAdUnits.INTERSTITIAL);
       //For Banners
        IronSource.Agent.init(appKey, IronSourceAdUnits.BANNER);

        IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;
       
        Debug.Log("unity-script: IronSource.Agent.validateIntegration");
        IronSource.Agent.validateIntegration();

        IronSource.Agent.setAdaptersDebug(true); // Enable debug mode for adapters;


        IronSourceRewardedVideoEvents.onAdAvailableEvent += RewardedVideoAvailabilityChangedEvent;
        IronSourceRewardedVideoEvents.onAdClickedEvent += RewardedVideoAdClickedEvent;
        IronSourceRewardedVideoEvents.onAdRewardedEvent += OnVideoAdReward;
    }

    private void SdkInitializationCompletedEvent() {
        Debug.Log("unity-script: SdkInitializationCompletedEvent");
    }

    public override void PreloadAds()
    {
        // Implementation for preloading IronSource ads
    }

    public override bool CanShowAds()
    {
        // Implementation to check if IronSource ads can be shown
        return IronSource.Agent.isRewardedVideoAvailable(); // Replace with actual logic
    }

    public override void ShowAds()
    {
        if (!CanShowAds())
            return;
        IronSource.Agent.showRewardedVideo();
    }

    void RewardedVideoAvailabilityChangedEvent(IronSourceAdInfo info)
    {
        Debug.Log("unity-script: I got RewardedVideoAvailabilityChangedEvent, value = " );
    }

    void RewardedVideoAdClickedEvent(IronSourcePlacement ssp, IronSourceAdInfo info)
    {
        Debug.Log("unity-script: I got RewardedVideoAdClickedEvent, name = " + ssp.getRewardName());
    }

    void OnVideoAdReward(IronSourcePlacement ssp, IronSourceAdInfo info)
    {
        Debug.Log("unity-script: I got OnVideoAdReward, name = " + ssp.getRewardName());
        Listener.OnVideoAdRewardReceived();
    }
}
