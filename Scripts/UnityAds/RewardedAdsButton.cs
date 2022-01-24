using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Events;

namespace HyperCasualFramework
{
    public class RewardedAdsButton : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        [SerializeField]
        protected bool loadAnotherAd = false;
        [SerializeField]
        protected UIMMButtonHandler showAdButton;
        [SerializeField]
        protected string androidAdUnitId = "Rewarded_Android";
        [SerializeField]
        protected string iosAdUnitId = "Rewarded_iOS";

        [SerializeField]
        protected UnityEvent OnShowCompleteEvent;

        protected string adUnitId;

        protected void Awake()
        {
            adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer) ? iosAdUnitId : androidAdUnitId;
        }

        protected void Start()
        {
            showAdButton.GetButton.DisableButton();
            LoadAd();
        }

        /// <summary>
        /// Load content to the Ad Unit:
        /// </summary>
        public void LoadAd()
        {
            // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
            MMDebug.DebugLogTime("Loading Ad: " + adUnitId);
            Advertisement.Load(adUnitId, this);
        }

        // Implement a method to execute when the user clicks the button.
        public void ShowAd(GameObject go)
        {
            // Disable the button: 
            showAdButton.GetButton.DisableButton();
            // Then show the ad:
            Advertisement.Show(adUnitId, this);
        }

        #region Load Listener

        /// <summary>
        /// If the ad successfully loads, add a listener to the button and enable it:
        /// </summary>
        public void OnUnityAdsAdLoaded(string placementId)
        {
            MMDebug.DebugLogTime("Ad Loaded: " + adUnitId);
            //Debug.Log("Ad Loaded: " + adUnitId);

            if (adUnitId.Equals(adUnitId))
            {
                // Configure the button to call the ShowAd() method when clicked:
                UIEventListener.Get(showAdButton.GetButton.gameObject).onClick = ShowAd;
                // Enable the button for users to click:
                showAdButton.GetButton.EnableButton();
            }
        }

        /// <summary>
        /// Implement Load and Show Listener error callbacks:
        /// </summary>
        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            Debug.LogWarning($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
            // Use the error details to determine whether to try to load another ad.
        }

        #endregion // Load Listener

        #region Show Listener

        /// <summary>
        /// Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
        /// </summary>
        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            if (adUnitId.Equals(adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                Debug.Log("Unity Ads Rewarded Ad Completed");
                // Grant a reward.
                OnShowCompleteEvent?.Invoke();
                // Load another ad:
                if(loadAnotherAd)
                    Advertisement.Load(adUnitId, this);
            }
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
            // Use the error details to determine whether to try to load another ad.
        }

        public void OnUnityAdsShowClick(string placementId) { }
        public void OnUnityAdsShowStart(string placementId) { }

        #endregion // Show Listener

#if UNITY_EDITOR

        [ContextMenu("Ads Show Complete")]
        void AdsShowComplete()
        {
            OnShowCompleteEvent?.Invoke();
        }

#endif

        void OnDestroy()
        {
            // Clean up the button listeners:
            UIEventListener.Get(showAdButton.GetButton.gameObject).onClick = null;
        }
    }

}
