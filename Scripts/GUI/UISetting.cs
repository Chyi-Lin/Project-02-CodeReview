using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace HyperCasualFramework
{
    /// <summary>
    /// 設定介面
    /// </summary>
    public class UISetting : MonoBehaviour
    {
        [SerializeField, Header("背景音調整")]
        protected Slider sliderBGM;
        [SerializeField]
        protected Toggle toggleMuteBGM;

        [SerializeField, Header("音效調整")]
        protected Slider sliderSoundEffect;
        [SerializeField]
        protected Toggle toggleMuteSoundEffect;

        protected void Awake()
        {
            Initialization();
            InitializationBySaveSetting();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected void Initialization()
        {
            if(sliderBGM)
                UIEventListener.Get(sliderBGM.gameObject).onSliderChanged = OnBGMChanged;

            if (toggleMuteBGM)
                UIEventListener.Get(toggleMuteBGM.gameObject).onToggleChanged = OnBGMMuteChanged;

            if (sliderSoundEffect)
                UIEventListener.Get(sliderSoundEffect.gameObject).onSliderChanged = OnSoundEffectChanged;

            if (toggleMuteSoundEffect)
                UIEventListener.Get(toggleMuteSoundEffect.gameObject).onToggleChanged = OnSoundEffectMuteChanged;
        }

        /// <summary>
        /// 讀取音量設定
        /// </summary>
        protected void InitializationBySaveSetting()
        {
            if (MMSoundManager.Instance == null)
                return;

            // 讀取設定
            MMSoundManager.Instance.LoadSettings();

            if (sliderBGM)
                sliderBGM.value = MMSoundManager.Instance.settingsSo.GetTrackVolume(MMSoundManager.MMSoundManagerTracks.Music);

            if (toggleMuteBGM)
                toggleMuteBGM.isOn = MMSoundManager.Instance.settingsSo.Settings.MusicOn;

            if (sliderSoundEffect)
                sliderSoundEffect.value = MMSoundManager.Instance.settingsSo.GetTrackVolume(MMSoundManager.MMSoundManagerTracks.Sfx);

            if (toggleMuteSoundEffect)
                toggleMuteSoundEffect.isOn = MMSoundManager.Instance.settingsSo.Settings.SfxOn;
        }

        /// <summary>
        /// 背景音樂調整
        /// </summary>
        protected void OnBGMChanged(GameObject go, float value)
        {
            if (MMSoundManager.Instance == null)
                return;

            if (value >= 0.0001f && value <= 2f)
                MMSoundManager.Instance.SetVolumeMusic(value);
        }

        /// <summary>
        /// 音效調整
        /// </summary>
        protected void OnSoundEffectChanged(GameObject go, float value)
        {
            if (MMSoundManager.Instance == null)
                return;

            if (value >= 0.0001f && value <= 2f)
                MMSoundManager.Instance.SetVolumeSfx(value);
        }

        /// <summary>
        /// 背景音樂靜音
        /// </summary>
        protected void OnBGMMuteChanged(GameObject go, bool isValue)
        {
            if (MMSoundManager.Instance == null)
                return;

            if (isValue)
                MMSoundManager.Instance.UnmuteMusic();
            else
                MMSoundManager.Instance.MuteMusic();
        }

        /// <summary>
        /// 背景音樂靜音
        /// </summary>
        protected void OnSoundEffectMuteChanged(GameObject go, bool isValue)
        {
            if (MMSoundManager.Instance == null)
                return;

            if (isValue)
                MMSoundManager.Instance.UnmuteSfx();
            else
                MMSoundManager.Instance.MuteSfx();
        }

        /// <summary>
        /// 保存設定
        /// </summary>
        public void SaveSetting()
        {
            if (MMSoundManager.Instance == null)
                return;

            MMSoundManager.Instance.SaveSettings();
        }

        /// <summary>
        /// 顯示介面
        /// </summary>
        public void Show()
        {
            gameObject.SetActive(true);
        }
    }
}
