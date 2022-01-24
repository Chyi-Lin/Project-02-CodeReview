using UnityEngine;

namespace HyperCasualFramework
{
    public class UIPlayGameSignIn : MonoBehaviour
    {
        /// <summary>
        /// 重新登入
        /// </summary>
        public void OnSignIn()
        {
            if (PlayGameManager.instance != null)
                PlayGameManager.instance.OnSignIn();
        }
    }
}

