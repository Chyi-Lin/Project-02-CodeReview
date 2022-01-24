using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HyperCasualFramework
{
    public class UIButtonHandler : UIButtonHandlerBase
    {
        [SerializeField]
        protected Button _button;

        [SerializeField]
        protected Image _image;

        [SerializeField]
        protected Text _text;

        protected Action<GameObject> OnClickEvent; 

        public Button GetButton { get { return _button; } }

        public Text GetText { get { return _text; } }

        /// <summary>
        /// 初始化
        /// </summary>
        protected override void Initialization()
        {
            if (_button)
                _button.onClick.AddListener(delegate() { OnClickEvent?.Invoke(gameObject); });
        }

        /// <summary>
        /// 設定文字
        /// </summary>
        public override void SetText(string text)
        {
            if (_text)
                _text.text = text;
        }

        /// <summary>
        /// 設定圖片與顏色
        /// </summary>
        public override void SetImage(Sprite image, Color color)
        {
            _image.sprite = image;
            _image.color = color;
        }

        /// <summary>
        /// 設定顏色
        /// </summary>
        public override void SetColor(Color color)
        {
            _image.color = color;
        }

        /// <summary>
        /// 設定按鈕事件
        /// </summary>
        public override void SetButtonEvent(UnityAction<GameObject> unityAction)
        {
            if (unityAction != null)
                OnClickEvent = unityAction.Invoke;
            else
                OnClickEvent = null;
        }
    }
}
