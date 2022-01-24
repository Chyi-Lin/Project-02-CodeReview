using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HyperCasualFramework
{
    public class UISelectButtonHandler : UIButtonHandler
    {
        [SerializeField]
        protected Image imageSelected;

        [SerializeField]
        protected bool _isOn;

        public bool isOn
        {
            get { return _isOn; }
            set { imageSelected.enabled = _isOn = value; }
        }

        protected override void Awake()
        {
            base.Awake();
            imageSelected.enabled = _isOn;
        }
    }
}
