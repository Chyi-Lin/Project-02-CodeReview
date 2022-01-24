using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;

namespace HyperCasualFramework
{
    public class UICoin : MonoBehaviour
    {
        [SerializeField]
        protected Text textCoin;

        [SerializeField]
        protected MMFeedbacks changeCoinFeedbacks;


        public void SetCoin(string coin, bool playAnimation = false)
        {
            if (textCoin)
                textCoin.text = coin;

            if(playAnimation)
                changeCoinFeedbacks?.PlayFeedbacks(this.transform.position);
        }
    }
}
