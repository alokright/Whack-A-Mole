public abstract class AbstractAdProvider
{
    public AbstractAdProvider(IAdListener listener)
    {
        InitializeProvider(listener);
    }
    protected IAdListener Listener;
    public abstract void InitializeProvider(IAdListener listener);
    public abstract void PreloadAds();
    public abstract bool CanShowAds();
    public abstract void ShowAds();
}
