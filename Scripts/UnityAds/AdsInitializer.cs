using UnityEngine;
using UnityEngine.Advertisements;

public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField]
    protected string androidGameId;
    [SerializeField]
    protected string iosGameId;
    [SerializeField]
    protected bool testMode = true;
    [SerializeField]
    protected bool enablePrePlacementMode = true;

    protected string gameId;

    protected void Awake()
    {
        InitializeAds();
    }

    public void InitializeAds()
    {
        gameId = (Application.platform == RuntimePlatform.IPhonePlayer) ? iosGameId : androidGameId;
        Advertisement.Initialize(gameId, testMode, enablePrePlacementMode, this);
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
}
