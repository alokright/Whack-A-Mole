using System.Collections.Generic;

public class AdManager : IAdListener
{
    private List<AbstractAdProvider> adProviders = new List<AbstractAdProvider>();
    public static AdManager Instance = null;
    public static void Initialize()
    {
        Instance = new AdManager();
    }

    public AdManager()
    {
        IronSourceAdsProvider provider =  new IronSourceAdsProvider(this);
        adProviders.Add(provider);

    }

    public bool CanShowAds()
    {
        foreach (var provider in adProviders)
        {
            if (provider.CanShowAds())
            {
                return true;
            }
        }
        return false;
    }

    public void ShowAds()
    {
#if UNITY_EDITOR
        AdsShown();
        OnVideoAdRewardReceived();
        return;
#endif
        foreach (var provider in adProviders)
        {
            if (provider.CanShowAds())
            {
                provider.ShowAds();
                break; // Assuming you only want to show an ad from one provider at a time
            }
        }
    }

    public void AdsShown()
    {
        // Handle AdsShown event
    }

    public void OnAdTapped()
    {
        // Handle OnAdTapped event
    }

    public void OnAdClosed()
    {
        // Handle OnAdClosed event
    }

    public void OnVideoAdRewardReceived()
    {
        GameEventManager.VideoAdRewardRecieved();
    }
}
