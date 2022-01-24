using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HyperCasualFramework
{
    public class UIProgressBar : MonoBehaviour
    {
        public Text textValue;
        public Image imageProgress;
        public Image imageDelayProgress;
        public float delayTime = 1.5f;
        [Range(0.01f, 1f)]
        public float speed = .1f;

        protected Coroutine delayAnimation;

        /// <summary>
        /// 設定進度條
        /// </summary>
        public virtual void SetProgressBar(int currentValue, int minValue, int maxValue)
        {
            if (textValue)
                textValue.text = $"{currentValue.ToString("00")}/{maxValue.ToString("00")}";

            if (imageProgress)
                imageProgress.fillAmount = (float)currentValue / (float)maxValue;

            if (imageDelayProgress)
            {
                if(delayAnimation != null)
                {
                    StopCoroutine(delayAnimation);
                }

                if(imageDelayProgress.IsActive())
                    delayAnimation = StartCoroutine(DelayAnimation(currentValue, maxValue));
            }
        }

        /// <summary>
        /// 延遲進度條動畫
        /// </summary>
        protected virtual IEnumerator DelayAnimation(int currentValue, int maxValue)
        {
            yield return new WaitForSeconds(delayTime);
            float currentFillAmount = (float)currentValue / (float)maxValue;

            while (currentFillAmount < imageDelayProgress.fillAmount)
            {
                imageDelayProgress.fillAmount -= speed * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            imageDelayProgress.fillAmount = currentFillAmount;
            delayAnimation = null;
        }
    }
}
