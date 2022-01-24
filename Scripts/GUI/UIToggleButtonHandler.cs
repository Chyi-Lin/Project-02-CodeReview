using UnityEngine;
using UnityEngine.UI;

namespace HyperCasualFramework
{
    public class UIToggleButtonHandler : UIButtonHandler
    {
        [SerializeField]
        protected Image foregroundImage;

        [SerializeField, Header("Color")]
        protected Color onColor = Color.white;
        [SerializeField]
        protected Color offColor = Color.white;

        [SerializeField]
        protected bool _isOn;

        public bool isOn
        {
            get { return _isOn; }
            set { _isOn = value; Toggle(); }
        }

        /// <summary>
        /// 按鈕開關
        /// </summary>
        protected void Toggle()
        {
            if (_isOn)
            {
                foregroundImage.enabled = false;
                SetText("0");
                SetColor(onColor);
            }
            else
            {
                foregroundImage.enabled = true;
                SetText("");
                SetColor(offColor);
            }
        }

#if UNITY_EDITOR
        /// <summary>
        /// 開關測試
        /// </summary>
        protected void OnValidate()
        {
            Toggle();
        }
#endif
    }
}

